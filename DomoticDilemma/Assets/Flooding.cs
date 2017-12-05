using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flooding : MonoBehaviour {

	private float counter;
	private float Scalar = 10;
	private bool flooding = false;

	public static bool Stop = false; 

	public GameObject Faucet;
	public GameObject BasinOverflow;
	public GameObject CounterOverflow;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F))
		{
			flooding = true;
		}

		if(flooding)
		{
			Flood();
		}

	}

	void Flood()
	{
		if(gameObject.tag == "Faucet")
		{
			Faucet.SetActive(true);
			if (Stop == true)
			{
				Faucet.SetActive(false);
			}
			Debug.Log("true");
		}

		if (gameObject.tag == "Basin")
		{
			gameObject.SetActive(true);

			
			Scalar += .038f;
			

			if (Scalar <= 40)
			{
				gameObject.transform.localScale = new Vector3(Scalar, 1, Scalar);
			}

			Vector3 target;
			target = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z );

			if (target.y <= -17)
			{
				Debug.Log(target.y);
				float speed = 1;
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, target, step);
			}


		}

		if (gameObject.tag == "BasinOverFlow")
		{
			
			counter += .03f;

			if(counter >= 30)
			{
				BasinOverflow.SetActive(true);
			}

			if (Stop == true)
			{
				BasinOverflow.SetActive(false);
			}
		}

		if (gameObject.tag == "Counter Overflow")
		{
			counter += .03f;
			if (counter >= 31)
			{
				CounterOverflow.SetActive(true);
			}

			if(Stop == true)
			{
				CounterOverflow.SetActive(false);
			}
		}

		if (gameObject.tag == "Flood")
		{
			counter += .025f;
			if (counter >= 31)
			{
				Vector3 target;
				target = new Vector3(transform.position.x, transform.position.y + 12f, transform.position.z);

				if (target.y <= -10)
				{
					float speed = 5;
					float step = speed * Time.deltaTime;
					transform.position = Vector3.MoveTowards(transform.position, target, step);
				}
				if(target.y >= -10)
				{
					Stop = true;
				}
			}
		}
	}
}
