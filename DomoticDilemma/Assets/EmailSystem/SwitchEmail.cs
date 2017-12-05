using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchEmail : MonoBehaviour {

    public delegate void ReadEmailEventHandler(object source, EventArgs e);

    public ReadEmailEventHandler ReadEmail;

    public Text Email;
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Email1() {
        Email.GetComponent<Text>().text = "Email1";
    }
    public void Email2()
    {
        Email.GetComponent<Text>().text = "Email2";
    }
    public void Email3()
    {
        Email.GetComponent<Text>().text = "Email3";
    }

    protected virtual void OnEmailRead()
    {
        if (ReadEmail != null)
            ReadEmail(this, EventArgs.Empty);
    }
}
