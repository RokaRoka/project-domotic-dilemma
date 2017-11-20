using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlState
{
    none,
    exploration,
    pause
}

public enum DialogueState
{
    none,
    dialogue,
    decision
}

public class SmartHouseManage : MonoBehaviour {

    //gamestates
    private ControlState currentControlState;
    private DialogueState currentDialogueState;

	//array of rooms
	public GameObject[] roomGameObjectArray;
	
	//player index
	private int playerPosition = -1;

	private void Start()
	{
		//assign an index number to each room
	}

    private void Update()
    {
        //RoomTestUpdate();
    }

    private void RoomTestUpdate()
    {

    }

    public void SwitchControlState(ControlState newState)
    {
        currentControlState = newState;
    }

    public void SwitchDialogueState(DialogueState newState)
    {
        currentDialogueState = newState;
    }

}
