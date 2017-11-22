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

	private DialogueChunk current = null;
	private int currentIndex = -1;
	private float t = 0;
	private float time_between_dialogue = 2.0f;

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
		if (currentIndex >= 0) {
			t += Time.deltaTime;
			if (t >= time_between_dialogue)
			{
				t -= time_between_dialogue;
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
		current = allDialogues[index];
		gameMange.SwitchDialogueState(DialogueState.dialogue);
		dialogueLineUI.SetActive(true);
		NextDialogueLine();
	}

	private void NextDialogueLine() {
		currentIndex++;
		if (currentIndex >= current.lineAmount) {
			EndDialogueChunk();
		} else {
			//check next line for decision
			//if decision, initiate decision
			if (current.CheckLineForDecision(currentIndex))
			{
				InitiateDecision();
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
		current = null;
		currentIndex = -1;
		dialogueLineUI.SetActive(true);
	}

	private void InitiateDecision() {
		gameMange.SwitchDialogueState(DialogueState.decision);
		//change mouse cursor here, but later should be done in game manager
		Cursor.lockState = CursorLockMode.Confined;
		UpdateDialogueDecisionUI();
	}

	private void Decision1Chosen() {
		gameMange.SwitchDialogueState(DialogueState.dialogue);
		//store decision
		//set decision holder UI inactive
	}

	private void Decision2Chosen()
	{
		gameMange.SwitchDialogueState(DialogueState.dialogue);
		//store decision
		//set decision holder UI inactive
	}

	private void UpdateDialogueLineUI()
	{
		//change text for DialogueLineUI
		dialogueLineUI.GetComponent<Text>().text = current.GetLineText(currentIndex);
	}

	private void UpdateDialogueDecisionUI() {
		//change each text for each decision UI object
		
	}

	public void TestDialogueChunk()
	{
		allDialogues = new DialogueChunk[2];
		allDialogues[0] = new DialogueChunk(testText.text);
		PlayDialogueChunk(0);
	}

}
