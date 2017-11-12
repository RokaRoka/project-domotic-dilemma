using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour {

	//ref for the smart house
	private SmartHouseManage smartHouseManage;

	private GameObject[] allDoorObjects;

	private bool _playerHere = false;

	private void Awake()
	{
		allDoorObjects = new GameObject[GameObject.FindGameObjectsWithTag("Door").Length];
	}
	
	// Use this for initialization
	void Start ()
	{
		allDoorObjects = GameObject.FindGameObjectsWithTag("Door");
		smartHouseManage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SmartHouseManage>();
	}

	public void PlayerFound()
	{
		_playerHere = true;
		OpenDoors();
	}
	
	public void PlayerLost()
	{
		_playerHere = false;
		CloseDoors();
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