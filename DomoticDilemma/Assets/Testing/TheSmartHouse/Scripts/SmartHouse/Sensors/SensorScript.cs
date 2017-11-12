using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script talks to the room about the player. It receives info from another script.
public class SensorScript : MonoBehaviour {

	//room ref
	private RoomScript parentRoomScript;

	private bool _playerFound = false;
	
	// Use this for initialization
	void Start ()
	{
		parentRoomScript = transform.parent.GetComponent<RoomScript>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayerFound()
    {
	    _playerFound = true;
	    parentRoomScript.PlayerFound();
    }

	public void PlayerLost()
	{
		_playerFound = false;
		parentRoomScript.PlayerLost();
	}
}
