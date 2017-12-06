using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuinnRoom : MonoBehaviour
{

	public Combine combine;
	public DoorScript doorScript;

	public bool combined;

	// Use this for initialization
	void Start()
	{
		

	}

	void Update()
	{
		if (combine.combined == true)
		{
			doorScript.OpenDoor();
		}
	}
}