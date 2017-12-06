using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatePlayerControls : MonoBehaviour {

    //other gameobject refs
    private SmartHouseManage gameManage;

    private void Awake()
    {
        gameManage = GameObject.FindGameObjectWithTag("GameController").GetComponent<SmartHouseManage>();
    }
	
	// Update is called once per frame
	private void Update () {
        CheckInput();
	}

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameManage.GetControlState() != ControlState.pause)
                gameManage.TryPause();
            else
                gameManage.TryUnPause();
        }
    }
}
