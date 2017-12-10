using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitch : MonoBehaviour {
	private AudioSource music;

	public AudioClip Bathroom;
	public AudioClip Hallway;
	public AudioClip Upstairs;
	public AudioClip Basement;
	public AudioClip Office;
	public AudioClip sciFi;
	public AudioClip starter;

	private bool HW;
	private bool BR;
	private bool BM;

	private bool OF;
	private bool SF;
	private bool UP;
	private bool switching;

	public float FadeTime;
	private float startVolume;


	// Use this for initialization
	void Start () {
		music = GetComponent<AudioSource>();
		startVolume = music.volume;
		music.clip = starter;
	}
	
	// Update is called once per frame
	void Update () {
		if(switching == true)
		{
			music.volume -= startVolume * Time.deltaTime / FadeTime;

			
		}

		if(switching == false)
		{
			music.volume = startVolume;
		}

		if (HW == true && music.volume<=0.2f)
		{
			switching = false;
			music.clip = Hallway;
			music.Play();
			HW = false;
		}

		if (BR == true && music.volume <= 0.2f)
		{
			switching = false;
			music.clip = Bathroom;
			music.Play();
			BR = false;
		}

		if (BM == true && music.volume <= 0.2f)
		{
			switching = false;
			music.clip = Basement;
			music.Play();
			BM = false;
		}

		if (SF == true && music.volume <= 0.2f)
		{
			switching = false;
			music.clip = sciFi;
			music.Play();
			SF = false;
		}

		if (UP == true && music.volume <= 0.2f)
		{
			switching = false;
			music.clip = Upstairs;
			music.Play();
			UP = false;
		}

		if (OF == true && music.volume <= 0.2f)
		{
			switching = false;
			music.clip = Office;
			music.Play();
			OF = false;
		}
	}

	void OnTriggerEnter(Collider col)
	{

		Debug.Log("trigger");
		if (col.gameObject.tag == "Hallway"  && music.clip != Hallway)
		{
			
				switching = true;
				HW = true;
				//music.clip = Hallway;
				//music.Play();
				Debug.Log("switch");
		
		}

		if (col.gameObject.tag == "Bathroom" && music.clip !=Bathroom)
		{

			switching = true;
			BR = true;
			//music.clip = Bathroom;
		}

		if (col.gameObject.tag == "Basement" && music.clip != Basement)
		{

			switching = true;
			BM = true;
			//music.clip = Basement;
		}

		if (col.gameObject.tag == "Control Room" && music.clip != sciFi)
		{
			switching = true;
			SF = true;
			//music.clip = sciFi;
		}

		if (col.gameObject.tag == "Upstairs" && music.clip != Upstairs)
		{
			switching = true;
			UP = true;
			music.volume = .7f;
			//music.clip = Upstairs;
		}

		if (col.gameObject.tag == "Office" && music.clip != Office)
		{
			switching = true;
			OF = true;
			//music.clip = Office;
		}
	}
}
