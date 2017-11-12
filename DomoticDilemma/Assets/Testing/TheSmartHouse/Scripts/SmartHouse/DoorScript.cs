using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private Collider coll;

    private Animator doorAnim;

    private bool doorOpen = false;

	// Use this for initialization
	void Start () {
        coll = GetComponent<Collider>();
        doorAnim = GetComponentInChildren<Animator>();
        OpenDoor();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenDoor()
    {
        doorOpen = true;
        coll.enabled = false;
        doorAnim.SetTrigger("OpenDoor");        
    }

    public void CloseDoor()
    {
        doorOpen = false;
        coll.enabled = true;
        doorAnim.SetTrigger("CloseDoor");
    }
}
