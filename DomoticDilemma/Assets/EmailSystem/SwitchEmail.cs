using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchEmail : MonoBehaviour {

	private GameObject gameManager;
	private GameObject dialogueManager;
	private GameObject emailManager;

    public Text EmailBody;
    public Text EmailSubject;

	private Email[] allEmails = new Email[10];

	private void Start()
	{
		gameManager = GameObject.FindGameObjectWithTag("GameController");
		dialogueManager = GameObject.FindGameObjectWithTag("DialogueController");
		emailManager = GameObject.FindGameObjectWithTag("EmailController");
	}

	public void Email1() {
        EmailBody.GetComponent<Text>().text = "Email1";
    }
    public void Email2()
    {
        EmailBody.GetComponent<Text>().text = "Email2";
    }
    public void Email3()
    {
        EmailBody.GetComponent<Text>().text = "Email3";
    }

	public void LeaveEmailSystem() {
		//UnLoad email scene
		SceneManager.MoveGameObjectToScene(gameManager, SceneManager.GetSceneByName("Testing Art"));
		SceneManager.MoveGameObjectToScene(dialogueManager, SceneManager.GetSceneByName("EmailScene"));
		SceneManager.MoveGameObjectToScene(emailManager, SceneManager.GetSceneByName("EmailScene"));
		SceneManager.UnloadSceneAsync("EmailScene");
	}
}
