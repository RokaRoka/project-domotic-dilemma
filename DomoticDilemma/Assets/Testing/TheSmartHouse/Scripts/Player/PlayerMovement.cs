using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //other gameobject refs
    private SmartHouseManage gameManage;
    private GameObject firstPersonCamera;
	
	//rigidbody
	private Rigidbody rb;
	
	//movement buffer
	private bool moving = false;
	private bool jumping = false;
	private Vector3 movement = Vector3.zero;
	private Vector2 mouseMovement = Vector2.zero;
	
	//player grounded variable
	private bool onGround = false;
	
	//player speed in meter/sec
	public float walkSpeed = 1f;
	public float jumpForce = 10f;
	public float Scalar;

    //ticking variable
    private bool isTicking = true;

    private void Awake()
    {
        gameManage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SmartHouseManage>();
        //event subscription
        gameManage.GamePause += OnGamePaused;
        gameManage.PlayerExplore += OnPlayerExploration;
    }

    // Use this for initialization
    private void Start ()
	{
		firstPersonCamera = Camera.main.gameObject;
		
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	private void Update () {
        if (isTicking)
        {
            CheckMouse();
            CheckInput();
            RotateBody();
        }
	}

	private void FixedUpdate()
	{
		if (moving) MoveBody();
		if (jumping) Jump();
	}

	private void CheckInput()
	{
	
		//movement
		if (Input.GetKey(KeyCode.W))
		{
			movement += Vector3.forward;
			moving = true;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			movement += Vector3.back;
			moving = true;
		}
		if (Input.GetKey(KeyCode.D))
		{
			movement += Vector3.right;
			moving = true;
		}
		else if (Input.GetKey((KeyCode.A)))
		{
			movement += Vector3.left;
			moving = true;
		}
		movement.y = 0;
		
		//jumping
		if (Input.GetKeyDown(KeyCode.Space) && onGround)
		{
			jumping = true;
		}
		//crouching
		else if (Input.GetKey(KeyCode.LeftControl))
		{
			Crouch();
		}
		else
		{
			Stand();
		}
	}

	private void CheckMouse()
	{
		//mouse stuff
		mouseMovement.x = Input.GetAxis ("Mouse X");
		mouseMovement.y = Input.GetAxis ("Mouse Y") * -1f;
	}

	private void CheckGround()
	{
		
	}
	
	private void MoveBody()
	{
		Vector3 playerForce;
		Vector3 forwardDirection = firstPersonCamera.transform.forward;
		Vector3 rightDirection = firstPersonCamera.transform.right;
		
		forwardDirection.y = 0;
		rightDirection.y = 0;
		
		forwardDirection.Normalize();
		rightDirection.Normalize();
		
		Debug.DrawRay(firstPersonCamera.transform.position, forwardDirection, Color.yellow, 0.1f);
		Debug.DrawRay(firstPersonCamera.transform.position, rightDirection, Color.yellow, 0.1f);

		playerForce = (forwardDirection * movement.z + rightDirection * movement.x).normalized;
		Debug.DrawRay(firstPersonCamera.transform.position, playerForce, Color.cyan, 0.1f);
		//movement = Vector3.Cross(forwardDirection, movement);
		
		//Debug.Log((walkSpeed * Time.deltaTime * 60f));
		playerForce *= (walkSpeed * Time.deltaTime);
		
		transform.Translate(playerForce, Space.World);
		//rb.AddForce(playerForce, ForceMode.Acceleration);

		movement = Vector3.zero;
		moving = false;
	}

	private void RotateBody()
	{
		firstPersonCamera.transform.Rotate(Vector3.up, mouseMovement.x, Space.World);
		firstPersonCamera.transform.Rotate (Vector3.right, mouseMovement.y, Space.Self);

		//Vector3 newRotation = transform.position;
		//newRotation.y = firstPersonCamera.transform.rotation.eulerAngles.y;

		//transform.rotation = Quaternion.Euler(newRotation);
		
		mouseMovement = Vector3.zero;
	}
	
	private void Jump()
	{
		rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
		jumping = false;
		onGround = false;
	}

	private void Crouch()
	{

		//transform.localScale = new Vector3(1f, .6f, 1f);
		transform.localScale = new Vector3(Scalar, Scalar * .6f, Scalar);
	}

	private void Stand()
	{
		//transform.localScale = Vector3.one;
		transform.localScale = new Vector3(Scalar, Scalar * .95f, Scalar);
	}

    //event callbacks
    private void OnGamePaused(object source, PauseEventArgs args)
    {
        if (args.isPaused)
        {
            Debug.Log("Player is certainly pausing.");
            isTicking = false;
        }
        else
        {
            Debug.Log("Player is certainly NOT pausing.");
            if (gameManage.GetDialogueState() != DialogueState.decision)
                isTicking = true;
        }
    }

    private void OnPlayerExploration(object source, EventArgs e)
    {
        isTicking = true;
    }

    private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.CompareTag("Ground"))
		{
			onGround = true;
		}
	}

	private void OnCollisionExit(Collision other)
	{
		if (other.gameObject.CompareTag("Ground"))
		{
			//onGround = false;
		}
	}
}
