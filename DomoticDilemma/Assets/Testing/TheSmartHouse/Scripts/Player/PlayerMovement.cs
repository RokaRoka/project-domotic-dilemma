using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	//movement buffer
	private bool moving = false;
	private Vector3 movement = Vector3.zero;
	private Vector2 mouseMovement = Vector2.zero;
	
	//Mouse lock
	
	//rigidbody
	private Rigidbody rb;
	
	// Use this for initialization
	void Start ()
	{
		//put this in a game manager later
		Cursor.lockState = CursorLockMode.Locked;
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		CheckMouse();
		CheckInput();
		RotateBody();
		if (moving) MoveBody();
	}

	private void CheckInput()
	{
		
		//movement
		if (Input.GetKeyDown(KeyCode.W))
		{
			movement += Vector3.forward;
			moving = true;
			Debug.Log(movement);
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			movement += Vector3.back;
			moving = true;
			Debug.Log(movement);
		}
		
		//jumping
	}

	private void CheckMouse()
	{
		//mouse stuff
		mouseMovement.x = Input.GetAxis ("Mouse X");
		mouseMovement.y = Input.GetAxis ("Mouse Y") * -1f;
	}

	private void MoveBody()
	{
		Vector3 positionNormalized = transform.position.normalized;
		movement.x *= positionNormalized.x;
		movement.z *= positionNormalized.z;
		//Debug.Log(movement);
		
		transform.Translate(movement, Space.World);
		//rb.AddForce(movement, ForceMode.VelocityChange);

		movement = Vector3.zero;
		moving = false;
	}

	private void RotateBody()
	{
		transform.Rotate(Vector3.up, mouseMovement.x, Space.World);
		transform.Rotate (Vector3.right, mouseMovement.y, Space.Self);
		mouseMovement = Vector3.zero;
	}
	
}
