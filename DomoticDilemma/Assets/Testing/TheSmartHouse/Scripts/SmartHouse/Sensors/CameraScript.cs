using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(SensorScript))]

//This script is 1 of 3 specific scripts for sensors. This script sends info to the sensor script.
public class CameraScript : MonoBehaviour {

	//Camera details
	
	//range (units)
	private float _range = 5f;
	//field of view (degrees)
	private float _fov = 40f;
	
	//looking for player
	public bool _lookingForPlayer = true;
	
	//player found bool
	public bool _playerFound = false;
	
	// Update is called once per frame
	private void Update ()
	{
		if (_lookingForPlayer && !_playerFound)
		{
			_playerFound = CheckForPlayer();
			if (_playerFound) ; //tell the room script
		}
	}

	private bool CheckForPlayer()
	{
		RaycastHit hit;
		//for now, use three raycasts to check for player
		
		for (int i = 0; i < 3; i++)
		{
			//angle in iteration
			Vector3 _angle = transform.forward;
			//Debug.Log(_angle);
			//_angle.y -= _fov / 2f;
			//_angle.y += (_fov/2f) * i;
			
			//throw out some debug rays
			Debug.DrawRay(transform.position + transform.forward/2f, _angle, Color.black, _range);
			if (Physics.Raycast(transform.position + transform.forward/2f, transform.rotation.eulerAngles, out hit, _range))
			{
				if (hit.transform.CompareTag("Player"))
				{
					//Player detected!
					return true;
				}
			}
		}

		return false;
	}
}
