using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueChunkName
{
	TEST_1, TEST_2
}

public class DialogueManager : MonoBehaviour {

	//references to UI
	public GameObject dialogueLineUI;
	public GameObject dialogueDecisionHolderUI;
	private GameObject[] decisionUIObjects = new GameObject[2];

	//all dialogue chunks
	private DialogueChunk[] allDialogues;

	private DialogueChunk current = null;
	private int currentIndex = -1;
	private float t = 0;
	private float time_between_dialogue = 0.5f;

	private void Awake()
	{
		decisionUIObjects[0] = dialogueDecisionHolderUI.transform.GetChild(0).gameObject;
		decisionUIObjects[1] = dialogueDecisionHolderUI.transform.GetChild(1).gameObject;
	}

	private void Update()
	{
		if (currentIndex >= 0) {
			t += Time.deltaTime;
		}
	}

	//finish dialogue system
	private void LoadAllDialogue() {
		TextAsset[] dialogueFiles = Resources.LoadAll("Dialogue") as TextAsset[];
		allDialogues = new DialogueChunk[dialogueFiles.Length];

		for (int i = 0; i < dialogueFiles.Length; i++) {
			allDialogues[i] = new DialogueChunk(dialogueFiles[i].text);
		}
	}

	public void PlayDialogueChunk(int index)
	{
		current = allDialogues[index];
		//change gamestate in game manager

	}

	private void NextDialogueLine() {
		currentIndex++;
		if (currentIndex > current.lineAmount) {
			EndDialogueChunk();
		} else {
			//check next line for decision
			//if decision, initiate decision
			//if line, display line and play voiceline
		}
	}

	private void EndDialogueChunk() {
		//do effect of Dialogue chunk	
		current = null;
		currentIndex = -1;
	}

	private void InitiateDecision() {
		//change Gamestate in manager
		UpdateDialogueDecisionUI();
	}

	private void Decision1Chosen() {
		//change Gamestate in manager
		//store decision
		//set decision holder UI inactive
	}

	private void Decision2Chosen()
	{
		//change Gamestate in manager
		//store decision
		//set decision holder UI inactive
	}

	private void UpdateDialogueLineUI()
	{
		//change text for DialogueLineUI
	}

	private void UpdateDialogueDecisionUI() {
		//change each text for each decision UI object
	}

}
