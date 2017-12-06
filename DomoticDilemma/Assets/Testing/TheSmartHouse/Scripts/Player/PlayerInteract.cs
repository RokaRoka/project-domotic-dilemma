using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour {

    //other gameobject refs
    private SmartHouseManage gameManage;

    public GameObject playerCursorUI;
    private Camera firstPersonCamera;

    private GameObject interactGameObject = null;

    //colors
    private bool interactHighlighted = false;
    private Color originalColor = Color.white;
    private Color interactColor = Color.cyan;

    //raycast vars
    private int layerMask = 1 << 9;
    public float _maxHitRange = 1f;
    public float _interactCooldown = 0.2f;
    private float t_interactTimer = 0;

    private bool interacting = false;

    //ticking variable for events
    private bool isTicking = true;

    private void Awake()
    {
        gameManage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SmartHouseManage>();
        //event subscription
        gameManage.GamePause += OnGamePaused;
    }

    // Use this for initialization
    private void Start () {
        originalColor.a = 0.75f;
        interactColor.a = 1f;
        
        firstPersonCamera = Camera.main;
        playerCursorUI.GetComponent<Image>().color = originalColor;
    }
	
	// Update is called once per frame
	private void Update () {
        if (isTicking)
        {
            if (t_interactTimer < _interactCooldown)
                CooldownTick();
            if (!interacting)
                CheckInteractible();
            CheckInput();
        }
	}

    private void CheckInteractible()
    {

        Debug.DrawRay(firstPersonCamera.transform.position + firstPersonCamera.transform.forward * firstPersonCamera.nearClipPlane, firstPersonCamera.transform.rotation * Vector3.forward * _maxHitRange, Color.black);
        RaycastHit hit;

        if (Physics.Raycast(firstPersonCamera.transform.position, firstPersonCamera.transform.rotation * Vector3.forward, out hit, _maxHitRange, layerMask))
        {
            if (interactHighlighted)
            {
                if (interactGameObject != hit.transform.gameObject)
                {
                    SetNewInteractObject(hit.transform.gameObject);
                }
            }
            else
            {
                SetNewInteractObject(hit.transform.gameObject);
            }
            Debug.Log("This is working?");
            
        } else if (interactHighlighted)
        {
            SetNewInteractObject(null);
        }
    }

    private void CheckInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (t_interactTimer >= _interactCooldown)
            {
                if (!interacting && interactHighlighted)
                    SendInteraction();
                else if (interacting)
                    SendUnInteraction();
                t_interactTimer = 0;
            }
        }
    }

    private void CooldownTick()
    {
       t_interactTimer += Time.deltaTime;
    }
    
    private void SetNewInteractObject(GameObject interact)
    {
        if (interact != null)
        {
            interactGameObject = interact;
            interactHighlighted = true;
        } else
        {
            interactHighlighted = false;
            interactGameObject = null;
        }
        ChangeCursorColor();
    }

    private void SendInteraction()
    {
        interacting = true;
        interactGameObject.SendMessage("PlayerInteract", gameObject);
    }

    private void SendUnInteraction()
    {
        interacting = false;
        interactGameObject.SendMessage("PlayerUnInteract");
    }
    
    private void ChangeCursorColor()
    {
        Color newColor;
        if (interactHighlighted)
        {
            newColor = interactColor;

        } else
        {
            newColor = originalColor;
        }

        playerCursorUI.GetComponent<Image>().color = newColor;
    }

    //event callbacks
    private void OnGamePaused(object source, PauseEventArgs args)
    {
        if (args.isPaused)
        {
            isTicking = false;
        }
        else
        {
            isTicking = true;
        }
    }
}
