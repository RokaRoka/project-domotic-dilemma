using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpstairsDoor : MonoBehaviour
{


	public DoorScript doorScript;



	// Use this for initialization
	void Start()
	{
		

	}

	void Update()
	{
		if (PlayerMovement.card == false)
		{
			doorScript.OpenDoor();
		}
	}
}
