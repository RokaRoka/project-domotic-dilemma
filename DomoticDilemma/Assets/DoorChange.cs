using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorChange : MonoBehaviour {

	public CameraScript cameraScript;
	public CameraScript cameraScript2;

	public DoorScript doorScript;
	
	// Use this for initialization
	void Start () {
		
			cameraScript.FindingPlayer += OnPlayerFound;

			cameraScript.LosingPlayer += OnPlayerLost;
			

	}

	void Update()
	{
		//doorScript.CloseDoor();
	}
	
	// Update is called once per frame
	
		

	void OnPlayerFound(object source, EventArgs e)
	{
		gameObject.layer = 9;
		doorScript.OpenDoor();
	}

	void OnPlayerLost(object source, EventArgs e)
	{
		doorScript.CloseDoor();
	}
}
