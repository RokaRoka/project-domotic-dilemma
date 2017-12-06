using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combine : MonoBehaviour {
	public GameObject Decoy;
	public GameObject item;
	public bool combined = false;
	private bool canCombine;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	if(canCombine == true)
	{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				gameObject.SetActive(false);
				item.SetActive(false);
				Instantiate(Decoy, transform.position, Quaternion.identity);
				Debug.Log("spawn");
			}
	}
		if(gameObject.tag == "decoy")
		{
			if(transform.position.x >= -1050 && transform.position.x < -1400 )
			{
				if (transform.position.y >= 250 && transform.position.y < 350)
				{
					if (transform.position.z >= -2100 && transform.position.z < -1700)
					{
						combined = true;
					}
				}
			}
		}
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "jacket")
		{
			canCombine = true;
			Debug.Log("hit");
		}

		if (col.gameObject.tag == "broom")
		{
			canCombine = true;
			
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		canCombine = false;
	}
}
