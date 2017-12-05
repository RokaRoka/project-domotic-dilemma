using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScript : MonoBehaviour {
    private bool load = false;
    private bool creditload = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (load)
        {
            
            StartCoroutine(LoadYourAsyncScene());
        }
        if (creditload)
        {

            StartCoroutine(LoadYourAsyncScene2());
        }
    }
    IEnumerator LoadYourAsyncScene()
    {
       
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene2");

       
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    IEnumerator LoadYourAsyncScene2()
    {

        AsyncOperation asyncLoad1 = SceneManager.LoadSceneAsync("Scene3");


        while (!asyncLoad1.isDone)
        {
            yield return null;
        }
    }
    public void Startbutton()
    {
        load = true;
    }
    public void Creditbutton()
    {
        creditload = true;
    }
}
