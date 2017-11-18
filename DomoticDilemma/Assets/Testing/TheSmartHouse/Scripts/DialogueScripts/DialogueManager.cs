using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

	//all dialogue chunks
	private DialogueChunk[] allDialogues;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//finish dialogue system
	private void LoadAllDialogue() {
		TextAsset[] dialogueFiles = Resources.LoadAll("Dialogue") as TextAsset[];
		allDialogues = new DialogueChunk[dialogueFiles.Length];

		for (int i = 0; i < dialogueFiles.Length; i++) {
			allDialogues[i] = new DialogueChunk(dialogueFiles[i].text);
		}
	}

}
