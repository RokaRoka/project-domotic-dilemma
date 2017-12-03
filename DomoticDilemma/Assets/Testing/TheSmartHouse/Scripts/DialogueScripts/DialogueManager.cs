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
	private float dialogue_show_time = 3f;
	private float time_between_dialogue = 3.0f;

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

	private void NextDialogueLine() {
		currentIndex++;
		if (currentIndex >= currentChunk.lineAmount) {
			EndDialogueChunk();
		} else {
			//check next line for decision
			if (currentChunk.CheckLineForDecision(currentIndex))
			{
				if (currentChunk.CheckIfDecisionMade(currentIndex))
				{
					SkipOtherDecision();
					UpdateDialogueDecisionUI();
				}
				else
				{
					InitiateDecision();
				}
			}
			else
			{
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
		gameMange.SwitchDialogueState(DialogueState.decision);
		//change mouse cursor here, but later should be done in game manager
		Cursor.lockState = CursorLockMode.None;
		
		decisionUIObjects[0].SetActive(true);
		decisionUIObjects[1].SetActive(true);

		currentDecisionPoint = currentChunk.GetDecisionPoint(currentIndex);
		
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
