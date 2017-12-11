using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EmailComputerInteraction : MonoBehaviour {

	private EmailManager emailManager;

	// Use this for initialization
	void Start () {
		emailManager = GameObject.FindGameObjectWithTag("EmailController").GetComponent<EmailManager>();
	}

	public void PlayerInteract(GameObject player)
	{
		player.GetComponent<PlayerInteract>().SetInteracting(false);
		//Load email UI Objects
		emailManager.ActivateEmailSystem();
	}
	
}
