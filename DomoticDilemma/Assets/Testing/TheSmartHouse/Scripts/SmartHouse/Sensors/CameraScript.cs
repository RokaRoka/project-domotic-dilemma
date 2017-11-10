using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SensorScript))]

//This script is 1 of 3 specific scripts for sensors. This script sends info to the sensor script.
public class CameraScript : MonoBehaviour {

    //sensor script refernece!
    private SensorScript sensorScript;

    //door to close when out of sight
    public GameObject doorToUse;

    //Camera details

    //where to shoot the ray from
    public GameObject lens;
    private Camera actualCamera;

	//range (units)
	private float _range = 5f;
	//field of view (degrees)
	private float _fov = 40f;
	
	//looking for player
	public bool _lookingForPlayer = true;
	
	//player found bool
	public bool _playerFound = false;
    //layer mask for the player
    private int layerMask = 1 << 8;

    private void Start()
    {
        actualCamera = lens.GetComponent<Camera>();
        sensorScript = GetComponent<SensorScript>();
    }

    // Update is called once per frame
    private void Update ()
	{
		if (_lookingForPlayer && !_playerFound)
		{
			_playerFound = CheckForPlayer();
			if (_playerFound) Debug.Log("Woah! A Player!"); //tell the room script
		}
	}

	private bool CheckForPlayer()
	{
		RaycastHit hit;
        //for now, use three raycasts to check for player

        /*for (int i = 0; i < 3; i++)
		{
            //center angle in iteration
            Vector3 _centerRotation = lens.transform.forward;
			//_angle.y -= _fov / 2f;
			//_angle.y += (_fov/2f) * i;
            
			
			//throw out some debug rays
			Debug.DrawRay(lens.transform.position, _centerRotation * _range, Color.black);
			if (Physics.Raycast(lens.transform.position, _centerRotation, out hit, _range, layerMask))
			{
                return true;
			}
		}
        */
        
        Vector3 _centerRotation = lens.transform.forward;
        Vector3 _collSize = new Vector3(actualCamera.orthographicSize, actualCamera.orthographicSize, actualCamera.orthographicSize);

        //throw out some debug rays
        Debug.DrawRay(lens.transform.position, _centerRotation * _range, Color.black);
        if (Physics.BoxCast(lens.transform.position + lens.transform.forward * _collSize.z, _collSize, _centerRotation, out hit, lens.transform.rotation, _range, layerMask))
        {
            return true;
        }

        return false;
	}
}
