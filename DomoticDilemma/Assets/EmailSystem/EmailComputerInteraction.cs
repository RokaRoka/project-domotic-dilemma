using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EmailComputerInteraction : MonoBehaviour {

	private GameObject gameManager;
	private GameObject dialogueManager;
	private GameObject emailManager;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.FindGameObjectWithTag("GameController");
		dialogueManager = GameObject.FindGameObjectWithTag("DialogueController");
		emailManager = GameObject.FindGameObjectWithTag("EmailController");

	}

	public void PlayerInteract(GameObject player)
	{
		player.GetComponent<PlayerInteract>().SetInteracting(false);
		//Load email scene
		SceneManager.LoadScene("EmailScene");
		gameManager.transform.parent = null;
		dialogueManager.transform.parent = null;
		emailManager.transform.parent = null;
		SceneManager.MoveGameObjectToScene(gameManager, SceneManager.GetSceneByName("EmailScene"));
		SceneManager.MoveGameObjectToScene(dialogueManager, SceneManager.GetSceneByName("EmailScene"));
		SceneManager.MoveGameObjectToScene(emailManager, SceneManager.GetSceneByName("EmailScene"));
	}
}
