using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DialogueChunkName
{
	TEST_1, TEST_2
}

public class DialogueManager : MonoBehaviour {

	//reference to Game Controller
	private SmartHouseManage gameMange;
	
	//references to UI
	public GameObject dialogueLineUI;
	public GameObject dialogueDecisionHolderUI;
	private GameObject[] decisionUIObjects = new GameObject[2];

	//all dialogue chunks
	private DialogueChunk[] allDialogues;

	private DialogueChunk currentChunk = null;
	private DecisionPoint currentDecisionPoint = null;
	private int currentIndex = -1;
	private float t = 0;
	private float dialogue_show_time = 2f;
	private float time_between_dialogue = 2.0f;

	//Ticking var
	public bool isTicking = true;
	
	//Debug text asset
	public TextAsset testText;
	
	private void Awake()
	{
		gameMange = GameObject.FindGameObjectWithTag("GameController").GetComponent<SmartHouseManage>();
		decisionUIObjects[0] = dialogueDecisionHolderUI.transform.GetChild(0).gameObject;
		decisionUIObjects[1] = dialogueDecisionHolderUI.transform.GetChild(1).gameObject;
		
		//LoadAllDialogue();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			TestDialogueChunk();
		}
		if (isTicking)
			Tick();
	}

	private void Tick()
	{
		if (currentIndex >= 0) {
			t += Time.deltaTime;
			if (t >= dialogue_show_time)
			{
				UpdateDialogueLineUI("");
			}
			if (t >= time_between_dialogue)
			{
				t -= dialogue_show_time + time_between_dialogue;
				NextDialogueLine();
			}
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
		currentChunk = allDialogues[index];
		gameMange.SwitchDialogueState(DialogueState.dialogue);
		dialogueLineUI.SetActive(true);
		NextDialogueLine();
	}

	/*
	private void NextDialogueLine() {
		currentIndex++;
		if (currentIndex >= currentChunk.lineAmount) {
			EndDialogueChunk();
		} else {
			//check next line for decision
			if (currentChunk.CheckLineForDecision(currentIndex))
			{
				//if decision, check if it is one that has already been made
				while (currentChunk.CheckIfDecisionMade(currentIndex))
				{
					SkipOtherDecision();
				}
				if (currentChunk.CheckLineForDecision(currentIndex))
				{
					InitiateDecision();
				}
				else if (currentIndex >= currentChunk.lineAmount)
				{
					UpdateDialogueLineUI();
				}
				
			}
			else
			{
				//if line, display line and play voiceline
				UpdateDialogueLineUI();
			}
		}
	}
	*/
	
	private void NextDialogueLine()
	{
		Debug.Log("Why");
		currentIndex++;
		//Check if it is too much
		if (currentIndex > currentChunk.lineAmount)
		{
			//end if past line amount
			EndDialogueChunk();
		}
		else
		{
			//check next line for decision
			if (currentChunk.CheckLineForDecision(currentIndex))
			{
				//if decision, check if it is one that has already been made
				while (currentIndex <= currentChunk.lineAmount && currentChunk.CheckIfDecisionMade(currentIndex))
				{
					//then skip if it has
					SkipOtherDecision();
				}
				//If the new line is above
				if (currentIndex > currentChunk.lineAmount)
				{
					//end if past line amount
					EndDialogueChunk();
				}
				else {
					if (currentChunk.CheckLineForDecision(currentIndex))
					{
						//if decision, display decisions and pause time
						InitiateDecision();
					}
					else
					{
						//if line, display line and play voiceline
						UpdateDialogueLineUI();
					}
				}
			}
			else {
				//if line, display line and play voiceline
				UpdateDialogueLineUI();
			}
		}
	}
	
	private void EndDialogueChunk() {
		//do effect of Dialogue chunk	
		currentChunk = null;
		currentIndex = -1;
		dialogueLineUI.SetActive(true);
	}

	private void InitiateDecision() {
		//Change GameState. The resulting three lines should be done in game manager
		gameMange.SwitchDialogueState(DialogueState.decision);
		Cursor.lockState = CursorLockMode.None;
		isTicking = false;
		
		//Set Decision UI active
		decisionUIObjects[0].SetActive(true);
		decisionUIObjects[1].SetActive(true);
		//Set the current decision point
		currentDecisionPoint = currentChunk.GetDecisionPoint(currentIndex);
		//Update the Dialogue decision UI
		UpdateDialogueDecisionUI();
	}

	private void SkipOtherDecision()
	{
		int depthToSearch = currentChunk.GetLineDepth(currentIndex) - 1;
		currentIndex = currentChunk.GetNextLineLowerThanDepth(currentIndex, depthToSearch);
	}

	private void UpdateDialogueLineUI()
	{
		//change text for DialogueLineUI
		dialogueLineUI.GetComponent<Text>().text = currentChunk.GetLineText(currentIndex);
	}
	
	private void UpdateDialogueLineUI(string overwritten)
	{
		//change text for DialogueLineUI
		dialogueLineUI.GetComponent<Text>().text = overwritten;
	}

	private void UpdateDialogueDecisionUI() {
		//change each text for each decision UI object
		decisionUIObjects[0].transform.GetChild(0).GetComponent<Text>().text = currentChunk.GetDecisionText(currentIndex, 0);
		decisionUIObjects[1].transform.GetChild(0).GetComponent<Text>().text = currentChunk.GetDecisionText(currentIndex, 1);
	}

	public void Decision1Chosen() {
		//Change State
		gameMange.SwitchDialogueState(DialogueState.dialogue);
		Cursor.lockState = CursorLockMode.Locked;
		isTicking = true;

		//store decision
		currentDecisionPoint.decisionFulfilled = true;
		currentDecisionPoint = null;
		//Update the actual Dialogue line
		UpdateDialogueLineUI(currentChunk.GetDecisionText(currentIndex, 0));
		//set decision holder UI inactive
		decisionUIObjects[0].SetActive(false);
		decisionUIObjects[1].SetActive(false);
	}

	public void Decision2Chosen()
	{
		//Change State
		gameMange.SwitchDialogueState(DialogueState.dialogue);
		Cursor.lockState = CursorLockMode.Locked;
		isTicking = true;

		//store decision
		currentDecisionPoint.decisionFulfilled = true;
		currentDecisionPoint = null;
		//Update the actual Dialogue line
		UpdateDialogueLineUI(currentChunk.GetDecisionText(currentIndex, 1));
		//set decision holder UI inactive
		decisionUIObjects[0].SetActive(false);
		decisionUIObjects[1].SetActive(false);
	}
	
	public void TestDialogueChunk()
	{
		allDialogues = new DialogueChunk[2];
		allDialogues[0] = new DialogueChunk(testText.text);
		PlayDialogueChunk(0);
	}

}
