using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementEnding : MonoBehaviour {
	private SmartHouseManage gameManage;
	private DialogueManager dialogueManage;

	public DialogueChunkName dialogueToLoad;

	private bool canEnd;

	// Use this for initialization
	void Start()
	{
		gameManage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SmartHouseManage>();
		dialogueManage = GameObject.FindGameObjectWithTag("DialogueController").GetComponent<DialogueManager>();
	}

	// Update is called once per frame
	void Update()
	{
		if (canEnd == true)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{

				if (!dialogueManage.CheckDialogueChunkComplete(dialogueToLoad) && gameManage.GetDialogueState() == DialogueState.none)
				{
					dialogueManage.PlayDialogueChunk(dialogueToLoad);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{

			canEnd = true;
			
		}
	}

	private void OnTriggerExit(Collider other)
	{
		

			canEnd = false;

		
	}
}