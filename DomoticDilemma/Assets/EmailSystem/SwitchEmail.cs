using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class SwitchEmail : MonoBehaviour {

	private GameObject gameManager;
	private GameObject dialogueManager;
	private GameObject emailManager;

	private DialogueManager _dialogueManager;

	public Scrollbar verticalScrollbar;
	
    public Text EmailBody;
    public Text EmailSubject;

	private Email currentEmail;
	private Email[] allEmails = new Email[10];
	private Email testEmail = new Email("{{Hi, this is an Email!\nThis is the email body.\n~Sam}}");
	
	private void Start()
	{
		//gameManager = GameObject.FindGameObjectWithTag("GameController");
		//dialogueManager = GameObject.FindGameObjectWithTag("DialogueController");
		//emailManager = GameObject.FindGameObjectWithTag("EmailController");

		//_dialogueManager = dialogueManager.GetComponent<DialogueManager>();

		verticalScrollbar.onValueChanged.AddListener(OnVerticalScrollbarChange);
		
		LoadEmail(testEmail);
		//allEmails = emailManager.GetComponent<EmailManager>().GetEmails();
		
	}

	private void LoadEmail(Email toLoad)
	{
		currentEmail = toLoad;
		EmailSubject.text = toLoad.GetSubject();
		EmailBody.text = toLoad.GetBody();
		
		Debug.Log(toLoad.GetBody());
		
		OnVerticalScrollbarChange(verticalScrollbar.value);
	}
	
	public void Email1()
	{
		LoadEmail(allEmails[0]);
    }
    public void Email2()
    {
	    LoadEmail(allEmails[0]);
    }
    public void Email3()
    {
	    LoadEmail(allEmails[0]);
    }

	public void OnVerticalScrollbarChange(float value)
	{
		Debug.Log("Scrollbar value: "+value);
		if (value == 0 && !currentEmail.isRead)
		{
			currentEmail.isRead = true;
			//play dialogue
			Debug.Log("WOAH");
		}
	}

	public void LeaveEmailSystem() {
		//Switch Camera back
	}
}
