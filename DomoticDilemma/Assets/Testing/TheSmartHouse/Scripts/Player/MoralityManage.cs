using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoralityManage : MonoBehaviour {

    //Positive is courage, negative is fear
    private int moralityScale = 0;

	// Use this for initialization
	void Start () {
		
	}
    
    public void AddValue(int toAdd)
    {
        moralityScale += toAdd;
        Debug.Log(toAdd + " added. Morality now "+moralityScale);
    }

    public void AddFear(int toAdd)
    {
        moralityScale -= toAdd;
    }

    public bool MoralityAboveValue(int value)
    {
        if (value > moralityScale)
            return true;
        else
            return false;
    }

    public bool MoralityBelowValue(int value)
    {
        if (value < moralityScale)
            return true;
        else
            return false;
    }
}
