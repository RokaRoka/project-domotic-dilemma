using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour {

    //other gameobject refs
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

    // Use this for initialization
    private void Start () {
        firstPersonCamera = Camera.main;
        playerCursorUI.GetComponent<Image>().color = originalColor;
	}
	
	// Update is called once per frame
	private void Update () {
        CheckInteractible();
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

    private void SendInteraction(GameObject receiver)
    {
        receiver.SendMessage("PlayerInteract");
    }
}
