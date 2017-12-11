using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomDoor : MonoBehaviour
{
	public static bool canOpen = false;

	public CameraScript cameraScript;

	public DoorScript doorScript;
	// Use this for initialization
	void Start()
	{
		
		cameraScript.FindingPlayer += OnPlayerFound;
		cameraScript.LosingPlayer += OnPlayerLost;
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.H))
		{
			canOpen = true;
		}
	}


	void OnPlayerFound(object source, EventArgs e)
	{
		doorScript.CloseDoor();
	}

	void OnPlayerLost(object source, EventArgs e)
	{
	if(canOpen == true)
		doorScript.OpenDoor();

	}
}
