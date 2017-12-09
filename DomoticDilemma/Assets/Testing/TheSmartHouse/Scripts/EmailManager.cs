using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Email {
	private static string emailPattern = @"\{\{(.+)((?:\n+.+)+)\}\}((?:\n+.+)*)";

	private string subject;
	private string body;
	private DialogueChunk conversation;
	public bool isRead;

	public Email(string allTextInFile) {
		Match m = Regex.Match(allTextInFile, emailPattern);
		if (m.Groups[2].Success)
			subject = m.Groups[2].Value;
		else if (m.Groups[3].Success)
			body = m.Groups[3].Value;
		else if (m.Groups[4].Success)
			conversation = new DialogueChunk(m.Groups[4].Value);
		
		isRead = false;
	}

	public string GetSubject() {
		if (subject != null)
			return subject;
		else return "Email";
	}

	public string GetBody()
	{
		if (body != null)
			return body;
		else return "";
	}

	public bool CheckConversation() {
		if (conversation != null)
			return true;
		else return false;
	}

	public DialogueChunk GetConversation() {
		if (conversation != null)
			return conversation;
		else return null;
	}
}

public class EmailManager : MonoBehaviour {

	private DialogueManager dialogueManage;

	private Email[] allEmails;

	private void Start () {
		dialogueManage = GameObject.FindGameObjectWithTag("DialogueController").GetComponent<DialogueManager>();
	}

	public void InitEmailSystem() {
		allEmails = new Email[10];
		for (int i = 0; i < 10; i++) {
			allEmails[i] = new Email(dialogueManage.emailTextAssets[i].text);
		}
	}
}
