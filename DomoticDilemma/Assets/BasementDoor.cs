using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementDoor : MonoBehaviour
{


	public DoorScript doorScript;



	public void BasementOpen()
	{
		doorScript.OpenDoor();
	}
}