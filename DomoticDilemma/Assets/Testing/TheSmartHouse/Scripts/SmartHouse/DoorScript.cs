using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    private Collider coll;

	//private Animator[] doorAnim;
	private Animator doorAnim;

	private bool doorOpen = false;

	// Use this for initialization
	private void Start () {
        coll = GetComponent<Collider>();
		//doorAnim = GetComponentsInChildren<Animator>();
		doorAnim = GetComponent<Animator>();
		OpenDoor();
	}

    public void OpenDoor()
    {
        if (!doorOpen)
        {
            doorOpen = true;
            coll.enabled = false;
			// doorAnim[0].SetTrigger("OpenDoor");
			//doorAnim[1].SetTrigger("OpenDoor");
			doorAnim.SetTrigger("OpenDoor");
		}     
    }

    public void CloseDoor()
    {
        if (doorOpen)
        {
            doorOpen = false;
            coll.enabled = true;
			//doorAnim[0].SetTrigger("CloseDoor");
			//doorAnim[1].SetTrigger("CloseDoor");
			doorAnim.SetTrigger("CloseDoor");
		}
    }
}
