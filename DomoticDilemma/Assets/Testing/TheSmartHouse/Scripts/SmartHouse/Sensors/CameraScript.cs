using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class CameraScript : MonoBehaviour {

	public delegate void FindPlayerEventHandler (object source, EventArgs e);

	public event FindPlayerEventHandler FindingPlayer;
	
	public event FindPlayerEventHandler LosingPlayer;

    //Camera details
    //where to shoot the ray from
    public GameObject lens;
	//public GameObject lens2;
	private Camera actualCamera;

	//range (units)
	public float _range = 1000f;
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
    }

    // Update is called once per frame
    private void Update ()
	{
		if (_lookingForPlayer)
		{
			_playerFound = CheckForPlayer();
			if (_playerFound)
			{
				OnFindPlayer();
			}
			else
			{
				OnLosePlayer();
			}	
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
		Debug.DrawRay(lens.transform.position + (_collSize.x * lens.transform.right) + (_collSize.y * lens.transform.up), _centerRotation * _range, Color.white);
		Debug.DrawRay(lens.transform.position + (_collSize.x * lens.transform.right * -1) + (_collSize.y * lens.transform.up * -1), _centerRotation * _range, Color.white);
		if (Physics.BoxCast(lens.transform.position + lens.transform.forward * _collSize.z, _collSize, _centerRotation, out hit, lens.transform.rotation, _range, layerMask))
        {
			Debug.Log(hit.point + " was the player? and "+hit.transform.name+" Was hit.");
            return true;
        }

		

		return false;
	}
	
	

	protected virtual void OnFindPlayer()
	{
		if (FindingPlayer != null)
			FindingPlayer(this, EventArgs.Empty);
	}
	
	protected virtual void OnLosePlayer()
	{
		if (LosingPlayer != null)
			LosingPlayer(this, EventArgs.Empty);
	}
}
