﻿using System;
using System.Threading;
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

public class PauseEventArgs : EventArgs {
    public bool isPaused { get; set; }
}

public class SmartHouseManage : MonoBehaviour {

    //gamestates
    private ControlState currentControlState;
    private DialogueState currentDialogueState;

    //PAUSE VARIABLES
    public delegate void GamePauseEventHandler(object source, PauseEventArgs args);

    public event GamePauseEventHandler GamePause;

    //State before pause to return to after pause
    private ControlState controlStateBeforePause = ControlState.none;

    //Cooldown after switching in and out of pause states
    public float pauseInputCooldown = 1.5f;

    //EXPLORATION VARIABLES
    public delegate void PlayerExploreEventHandler(object source, EventArgs args);

    public event PlayerExploreEventHandler PlayerExplore;

    //ticking variable
    private float t = 0;

    private void Start()
    {
        TryExplore();
        SwitchDialogueState(DialogueState.none);
    }

    private void Update()
    {
        Tick();
    }

    private void Tick()
    {
        if (t > 0)
            t -= Time.deltaTime;
    }

    //For checking current state
    public ControlState GetControlState()
    {
        return currentControlState;
    }

    public DialogueState GetDialogueState()
    {
        return currentDialogueState;
    }

    //For changing states based on public functions
    private void SwitchControlState(ControlState newState)
    {
        //If last state was pause, run unpause
        if (currentControlState == ControlState.pause)
            OnGameUnPause();
        //Switch statement for events
        switch (newState)
        {
            case ControlState.none:
                //cutscene
                break;
            case ControlState.exploration:
                OnPlayerExplore();
                break;
            case ControlState.pause:
                OnGamePause();
                break;
            default:
                Debug.LogError("Error: Invalid state in SmartHouseManage: SwitchControlState().");
                break;
        }
        currentControlState = newState;
    }

    private void SwitchDialogueState(DialogueState newState)
    {
        //Switch statement for events
        switch (newState)
        {
            case DialogueState.none:
                //exit dialogue event
                break;
            case DialogueState.dialogue:
                //dialogue event
                break;
            case DialogueState.decision:
                //decision event
                break;
            default:
                Debug.LogError("Error: Invalid state in SmartHouseManage: SwitchDialogueState().");
                break;
        }
        currentDialogueState = newState;
    }

    //Public functions
    public bool TryCutscene()
    {
        SwitchControlState(ControlState.none);
        return true;
    }

    public bool TryExplore()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (t <= 0)
        {
            SwitchControlState(ControlState.exploration);
            return true;
        }
        return false;
    }

    public bool TryPause()
    {
        if (t <= 0)
        {
            Debug.Log("Trying to pause");
            Cursor.lockState = CursorLockMode.Confined;
            controlStateBeforePause = currentControlState;
            SwitchControlState(ControlState.pause);
            return true;
        }
        return false;
    }

    public bool TryUnPause()
    {
        if (t <= 0)
        {
            Debug.Log("Trying to unpause");
            Cursor.lockState = CursorLockMode.Locked;
            SwitchControlState(controlStateBeforePause);
            controlStateBeforePause = ControlState.none;
            return true;
        }
        return false;
    }

    public void EnterDialogue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SwitchDialogueState(DialogueState.dialogue);
    }

    public void ExitDialogue()
    {
        SwitchDialogueState(DialogueState.dialogue);
    }

    public void EnterDecision()
    {
        Cursor.lockState = CursorLockMode.Confined;
        SwitchDialogueState(DialogueState.decision);
    }

    protected virtual void OnGamePause()
    {
        Debug.Log("Log coming from OnGamePause in GameManager");
        if (GamePause != null)
            GamePause(this, new PauseEventArgs {isPaused = true});
        t = pauseInputCooldown;
    }

    protected virtual void OnGameUnPause()
    {
        if (GamePause != null)
            GamePause(this, new PauseEventArgs { isPaused = false });
        t = pauseInputCooldown;
    }

    protected virtual void OnPlayerExplore()
    {
        Debug.Log("Log coming from OnPlayerExplore in GameManager");
        if (PlayerExplore != null)
            PlayerExplore(this, EventArgs.Empty);
    }

    //public delegate void DialogueStateChanged(object source, EventArgs e);

    //public event DialogueStateChanged StateChange;

}
