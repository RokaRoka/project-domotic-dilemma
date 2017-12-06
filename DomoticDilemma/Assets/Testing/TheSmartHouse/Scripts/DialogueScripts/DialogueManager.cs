using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DialogueChunkName
{
	BASEMENT_1, BASEMENT_2,
    BATHROOM_1, BATHROOM_2_DEATH, BATHROOM_2_LIVE_NOEMAILS, BATHROOM_2_LIVE_EMAILS,

    HALLWAY_1_1, HALLWAY_1_2,
    HALLWAY_2_1,

    KITCHEN_1, KITCHEN_2,
    KITCHEN_3_FRIDGE_DRAWING_1, KITCHEN_3_FRIDGE_DRAWING_2, KITCHEN_3_FRIDGE_DRAWING_3,
    KITCHEN_4_DOOR_PREPUZZLE,

    OFFICE_1,
    OFFICE_EMAIL_1,
    OFFICE_EMAIL_2,
    OFFICE_EMAIL_3,
    OFFICE_EMAIL_4,
    OFFICE_EMAIL_5,
    OFFICE_EMAIL_6,
    OFFICE_EMAIL_7,
    OFFICE_EMAIL_8,
    OFFICE_EMAIL_9,
    OFFICE_EMAIL_10,

    PARENTSROOM_1,
    PARENTSROOM_2_PHOTOGRAPHS,

    TEST
}

public class DialogueManager : MonoBehaviour {

	//reference to Game Controller
	private SmartHouseManage gameManage;
	
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
	private float time_between_dialogue = 0.5f;

	//Ticking var
	public bool isTicking = true;
	
	//Debug text asset
    private int testIndex = 0;
	public TextAsset testText;
	
	private void Awake()
	{
		gameManage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SmartHouseManage>();
		decisionUIObjects[0] = dialogueDecisionHolderUI.transform.GetChild(0).gameObject;
		decisionUIObjects[1] = dialogueDecisionHolderUI.transform.GetChild(1).gameObject;
		
		LoadAllDialogue();
	}

	private void Update()
	{
        ///*
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            testIndex++;
        }
		if (Input.GetKeyDown(KeyCode.T))
		{
			TestDialogueChunk();
		}
        //*/
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
			if (t >= dialogue_show_time + time_between_dialogue)
			{
				t -= dialogue_show_time + time_between_dialogue;
				NextDialogueLine();
			}
		}
	}

	//finish dialogue system
	private void LoadAllDialogue() {
		Object[] dialogueFiles = Resources.LoadAll("Dialogue", typeof(TextAsset));
		allDialogues = new DialogueChunk[dialogueFiles.Length];

		for (int i = 0; i < dialogueFiles.Length; i++) {
            TextAsset textAsset = (TextAsset)dialogueFiles[i];
            allDialogues[i] = new DialogueChunk(textAsset.text);
		}
	}

	public void PlayDialogueChunk(int index)
	{
		currentChunk = allDialogues[index];
		gameManage.SwitchDialogueState(DialogueState.dialogue);
		dialogueLineUI.SetActive(true);
		Debug.Log("Line amount: "+currentChunk.lineAmount);
		NextDialogueLine();
	}

    public void PlayDialogueChunk(DialogueChunkName chunkName)
    {
        currentChunk = allDialogues[(int)chunkName];
        gameManage.SwitchDialogueState(DialogueState.dialogue);
        dialogueLineUI.SetActive(true);
        Debug.Log("Line amount: " + currentChunk.lineAmount);
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
		currentIndex++;
		Debug.Log("Next Index: "+currentIndex);
		//Check if it is too much
		if (currentIndex >= currentChunk.lineAmount)
		{
			Debug.Log("Ending");
			//end if past line amount
			EndDialogueChunk();
		}
		else
		{
			//check next line for decision
			if (currentChunk.CheckLineForDecision(currentIndex))
			{
				//if decision, check if it is one that has already been made
				while (currentChunk.CheckLineForDecision(currentIndex) && currentChunk.CheckIfDecisionMade(currentIndex))
				{
					//then skip if it has
					SkipOtherDecision();
					Debug.Log("Skipped to "+currentIndex);
					if (currentIndex > currentChunk.lineAmount)
						break; //This is to make sure that currentIndex doesn't cause an error
				}
				//If the new line is above
				if (currentIndex > currentChunk.lineAmount)
				{
					Debug.Log("Ending");
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
        currentChunk.isComplete = true;
		currentChunk = null;
		currentIndex = -1;
		dialogueLineUI.SetActive(true);
	}

	private void InitiateDecision() {
		//Change GameState. The resulting three lines should be done in game manager
		gameManage.SwitchDialogueState(DialogueState.decision);
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
		Debug.Log("Searching depth "+depthToSearch);
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
		gameManage.SwitchDialogueState(DialogueState.dialogue);
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
		gameManage.SwitchDialogueState(DialogueState.dialogue);
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
	
    public bool CheckDialogueChunkComplete(DialogueChunkName chunkToCheck)
    {
        return allDialogues[(int)chunkToCheck].isComplete;
    }

	public void TestDialogueChunk()
	{
		PlayDialogueChunk(testIndex);
	}

}
