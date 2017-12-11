using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuinnInteraction : MonoBehaviour {

	private SmartHouseManage gameManage;
	private DialogueManager dialogueManage;
	private MoralityManage moralityManage;

	public DialogueChunkName dialogueToLoad;
	public DialogueChunkName dialogueToLoad2;
	public Animator anim;

	//public Rigidbody rb;

	private bool counting = false;
	private float counter = 0;

	private bool CanEnd = false;
	public static bool morality;

	// Use this for initialization
	void Start()
	{
		gameManage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SmartHouseManage>();
		dialogueManage = GameObject.FindGameObjectWithTag("DialogueController").GetComponent<DialogueManager>();
	}

	// Update is called once per frame
	void Update()
	{

		//morality = moralityManage.MoralityAboveValue;

		if (counting == true)
		{
			counter += Time.deltaTime;
		}

		if(counter >= 30)
		{
			Debug.Log("End");

			//goto credits
		}

		if (CanEnd == true)
		{
			//if (!dialogueManage.CheckDialogueChunkComplete(dialogueToLoad) && gameManage.GetDialogueState() == DialogueState.none)
			//{
				if (Input.GetKeyDown(KeyCode.Mouse0))
				{
					PlayerMovement.walkSpeed = 0;

					counting = true;

					if (morality == true)
					{

						//if (!dialogueManage.CheckDialogueChunkComplete(dialogueToLoad) && gameManage.GetDialogueState() == DialogueState.none)
						//{
						dialogueManage.PlayDialogueChunk(dialogueToLoad);

						anim.SetBool("PositiveMorality", true);
						//}

						
					}

					if (morality == false)
					{

						//if (!dialogueManage.CheckDialogueChunkComplete(dialogueToLoad) && gameManage.GetDialogueState() == DialogueState.none)
						//{
						dialogueManage.PlayDialogueChunk(dialogueToLoad2);
						//}


					}
				}
			//}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			CanEnd = true;
			//if (!dialogueManage.CheckDialogueChunkComplete(dialogueToLoad) && gameManage.GetDialogueState() == DialogueState.none)
			//{
				//dialogueManage.PlayDialogueChunk(dialogueToLoad);
			//}
		}
	}
}