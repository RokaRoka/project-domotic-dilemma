using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Email {
	private static string emailPattern = @"\{\{(.+)[\n\r]{1,2}((?:.+[\n\r]{1,2})+)\}\}[\n\r]*(.*)";

	private string subject;
	private string body;
	private DialogueChunk conversation;
	public bool isRead;

	public Email()
	{
		Debug.LogError("Empty Email instantiated. Was no text file passed?");
	}
	
	public Email(string allTextInFile) {
		Match m = Regex.Match(allTextInFile, emailPattern);
		if (m.Groups[1].Success)
			subject = m.Groups[1].Value;
		else
			subject = "";
		if (m.Groups[2].Success)
			body = m.Groups[2].Value;
		else
			body = "";
		if (m.Groups[3].Success && m.Groups[3].Value != "")
			conversation = new DialogueChunk(m.Groups[3].Value);
		else
			conversation = null;
		
		isRead = false;
	}

	public string GetSubject() {
		if (subject != null)
			return subject;
		else
			return "Email";
	}

	public string GetBody()
	{
		if (body != null)
			return body;
		else
			return "";
	}

	public bool CheckConversation() {
		if (conversation != null)
			return true;
		else
			return false;
	}

	public DialogueChunk GetConversation()
	{
		if (conversation != null)
			return conversation;
		//return conversation;
		else
			return null;
	}
}

public class EmailManager : MonoBehaviour {

	public Camera playerCamera;
	public Camera emailCamera;

	private SmartHouseManage gameManager;
	private DialogueManager dialogueManage;

	public Scrollbar verticalScrollbar;

	public GameObject ExploreCanvas;
	public GameObject EmailCanvas;
	public Text EmailBody;
	public Text EmailSubject;

	private Email currentEmail;
	private Email[] allEmails = new Email[10];
	private Email testEmail = new Email("{{Hi, this is an Email!\nThis is the email body.\n~Sam}}");

	private void Start () {
		gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SmartHouseManage>();
		dialogueManage = GameObject.FindGameObjectWithTag("DialogueController").GetComponent<DialogueManager>();
		InitEmailSystem();

		verticalScrollbar.onValueChanged.AddListener(OnVerticalScrollbarChange);
	}

	public void InitEmailSystem() {
		for (int i = 0; i < 10; i++) {
			string emailText = dialogueManage.GetEmailText(i);
			allEmails[i] = new Email(emailText);
		}
	}

	public void ActivateEmailSystem() {
		if (gameManager.TryEmail())
		{
			Debug.Log("Switched to email state!");
			SwitchToEmailCamera();
		}
	}

	public void LeaveEmailSystem()
	{
		if (gameManager.TryLookAtEmail()) {
			if (gameManager.TryExplore())
			{
				SwitchToPlayerCamera();
			}
		}
	}

	private void LoadEmail(Email toLoad)
	{
		currentEmail = toLoad;
		EmailSubject.text = toLoad.GetSubject();
		EmailBody.text = toLoad.GetBody();

		Debug.Log(toLoad.GetBody());

		ResetVerticalScrollbar();
		OnVerticalScrollbarChange(verticalScrollbar.value);
	}

	public void Email1()
	{
		if (gameManager.TryLookAtEmail())
			LoadEmail(allEmails[0]);
	}
	public void Email2()
	{
		if (gameManager.TryLookAtEmail())
			LoadEmail(allEmails[1]);
	}
	public void Email3()
	{
		if (gameManager.TryLookAtEmail())
			LoadEmail(allEmails[2]);
	}
	public void Email4()
	{
		if (gameManager.TryLookAtEmail())
			LoadEmail(allEmails[3]);
	}
	public void Email5()
	{
		if (gameManager.TryLookAtEmail())
			LoadEmail(allEmails[4]);
	}
	public void Email6()
	{
		if (gameManager.TryLookAtEmail())
			LoadEmail(allEmails[5]);
	}
	public void Email7()
	{
		if (gameManager.TryLookAtEmail())
			LoadEmail(allEmails[6]);
	}
	public void Email8()
	{
		if (gameManager.TryLookAtEmail())
			LoadEmail(allEmails[7]);
	}
	public void Email9()
	{
		if (gameManager.TryLookAtEmail())
			LoadEmail(allEmails[8]);
	}
	public void Email10()
	{
		if (gameManager.TryLookAtEmail())
			LoadEmail(allEmails[9]);
	}

	private void ResetVerticalScrollbar()
	{
		verticalScrollbar.value = 1;
	}

	public void OnVerticalScrollbarChange(float value)
	{
		Debug.Log("Scrollbar value: " + value);
		if (value == 0 && !currentEmail.isRead)
		{
			currentEmail.isRead = true;
			//dialogueManage.PlayDialogueChunk(currentEmail.GetConversation());
			Debug.Log("WOAH");
		}
	}

	//Camera handlers

	private void SwitchToPlayerCamera()
	{
		ExploreCanvas.SetActive(true);
		EmailCanvas.SetActive(false);
		emailCamera.enabled = false;
		playerCamera.enabled = true;
	}

	private void SwitchToEmailCamera()
	{
		ExploreCanvas.SetActive(false);
		EmailCanvas.SetActive(true);
		playerCamera.enabled = false;
		emailCamera.enabled = true;
		Debug.Log("Switched cameras!");
	}
}
