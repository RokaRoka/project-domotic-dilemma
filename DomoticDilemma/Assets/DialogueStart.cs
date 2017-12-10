using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStart : MonoBehaviour {

	private SmartHouseManage gameManage;
	private DialogueManager dialogueManage;

	public DialogueChunkName dialogueToLoad;

	// Use this for initialization
	void Start () {
		gameManage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SmartHouseManage>();
		dialogueManage = GameObject.FindGameObjectWithTag("DialogueController").GetComponent<DialogueManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")) {
			if (!dialogueManage.CheckDialogueChunkComplete(dialogueToLoad) && gameManage.GetDialogueState() == DialogueState.none)
			{
				dialogueManage.PlayDialogueChunk(dialogueToLoad);
			}
		}
	}
}
