using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeMachineScript : MonoBehaviour {

	//held by player
	private bool isBeingHeld = false;
	public GameObject Arrow;
	public GameObject smoke1;
	//public GameObject smoke2;
	//public GameObject smoke3;
	//public GameObject smoke4;
	//public GameObject smoke5;
	//public GameObject smoke6;
	//public GameObject smokePlayer;

	//collider
	private Collider coll;

	private Rigidbody rb;

	private bool fork = false;
	// Use this for initialization
	void Start ()
	{
		coll = GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
		///if(gameObject.tag =="Fork")
		//{
			//fork = true;
		///}
	}

	public void PlayerInteract(GameObject player)
	{
		isBeingHeld = true;
		//coll.enabled = false;
		Arrow.GetComponent<Renderer>().enabled = false;
		rb.useGravity = false;
		rb.constraints = RigidbodyConstraints.FreezeAll;
		transform.parent = player.transform.GetChild(0);
		transform.localPosition = Vector3.forward * 1.3f;
	}

	public void PlayerUnInteract()
	{
		isBeingHeld = false;
		coll.enabled = true;
		rb.useGravity = true;
		Arrow.GetComponent<Renderer>().enabled = true;
		rb.constraints = RigidbodyConstraints.None;
		transform.parent = null;
	}

	private void OnCollisionEnter(Collision collision)
	{
		//if (fork == true)
		//{
			Debug.Log("fork");
			if (collision.gameObject.tag == "Oven")
			{
			Debug.Log("boom");
			smoke1.SetActive(true);
				//smoke2.SetActive(true);
				//smoke3.SetActive(true);
				//smoke4.SetActive(true);
				//smoke5.SetActive(true);
				//smoke6.SetActive(true);
				//smokePlayer.SetActive(true);
			}
		//}
	}
}
