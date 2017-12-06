using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentsRoom : MonoBehaviour
{
	private bool canOpen = false;

	public CameraScript cameraScript;
	private bool firstTime = true;

public Combine combine;

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
		//if (combine.combined == true)
		//{
			//canOpen = true;
		//}
	}


	void OnPlayerFound(object source, EventArgs e)
	{
		Debug.Log("close");

		if (firstTime)
		{
			doorScript.CloseDoor();
			firstTime = false;
		}
		else
		{
			doorScript.OpenDoor();
		}
	}

	void OnPlayerLost(object source, EventArgs e)
	{
		if (firstTime == true)
		{
			doorScript.OpenDoor();
		}

		else
		{
			doorScript.CloseDoor();
		}

	}
}