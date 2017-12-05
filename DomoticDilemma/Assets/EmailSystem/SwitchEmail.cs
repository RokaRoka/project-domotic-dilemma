using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmailEventArgs : EventArgs
{
    //Email index should be sent
}

public class SwitchEmail : MonoBehaviour {

    public delegate void ReadEmailEventHandler(object source, EventArgs e);

    public ReadEmailEventHandler ReadEmail;

    public Text EmailBody;
    public Text EmailSubject;

    private string[] emailBodyStrings;
    private string[] emailSubjectStrings;
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Email1() {
        EmailBody.GetComponent<Text>().text = "Email1";
    }
    public void Email2()
    {
        EmailBody.GetComponent<Text>().text = "Email2";
    }
    public void Email3()
    {
        EmailBody.GetComponent<Text>().text = "Email3";
    }

    protected virtual void OnEmailRead()
    {
        if (ReadEmail != null)
            ReadEmail(this, EventArgs.Empty);
    }
}
