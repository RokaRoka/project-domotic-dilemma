using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGroupScript : MonoBehaviour {

	//ref for the smart house
	private SmartHouseManage smartHouseManage;

	public GameObject[] allDoorObjects;

	
	// Use this for initialization
	void Start ()
	{
		smartHouseManage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SmartHouseManage>();
	}
	
	public void OpenDoor(int index)
	{
		allDoorObjects[index].GetComponent<DoorScript>().OpenDoor();
	}
	
	public void CloseDoor(int index)
	{
		allDoorObjects[index].GetComponent<DoorScript>().CloseDoor();
	}
	
	public void OpenDoors()
	{
		for (int i = 0; i < allDoorObjects.Length; i++)
		{
			allDoorObjects[i].GetComponent<DoorScript>().OpenDoor();
		}	
	}
	
	public void CloseDoors()
	{
		for (int i = 0; i < allDoorObjects.Length; i++)
		{
			allDoorObjects[i].GetComponent<DoorScript>().CloseDoor();
		}	
	}
}
