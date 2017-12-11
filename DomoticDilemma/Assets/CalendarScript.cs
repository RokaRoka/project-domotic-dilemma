using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarScript : MonoBehaviour {

	public GameObject basementDoor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayerInteract(GameObject player) {
		basementDoor.GetComponent<BasementDoor>().BasementOpen();
		player.GetComponent<PlayerInteract>().SetInteracting(false);
	}
}
