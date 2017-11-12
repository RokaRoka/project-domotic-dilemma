using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeMachineScript : MonoBehaviour {

	//held by player
	private bool isBeingHeld = false;
	
	//collider
	private Collider coll;

	private Rigidbody rb;
	
	// Use this for initialization
	void Start ()
	{
		coll = GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
	}

	public void PlayerInteract(GameObject player)
	{
		isBeingHeld = true;
		coll.enabled = false;
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeAll;
		transform.parent = player.transform.GetChild(0);
		transform.localPosition = Vector3.forward * 2.0f;
	}

	public void PlayerUnInteract()
	{
		isBeingHeld = false;
		coll.enabled = true;
		rb.useGravity = true;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		transform.parent = null;
	}
}
