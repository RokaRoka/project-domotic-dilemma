using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayScript : MonoBehaviour {

    public Text loadTextUI;

    AsyncOperation currentAsyncLoad;
	
	// Update is called once per frame
	void Update () {
        if (currentAsyncLoad != null)
        {
            loadTextUI.text = "Loading... " + currentAsyncLoad.progress + "%";
        }
            
    }
    IEnumerator LoadGameSceneAsync()
    {
        currentAsyncLoad = SceneManager.LoadSceneAsync("Scene2");

        Debug.Log("Woah");

        while (!currentAsyncLoad.isDone)
        {
            yield return null;
        }
    }
    IEnumerator LoadCreditsSceneAsync()
    {

        AsyncOperation asyncLoad1 = SceneManager.LoadSceneAsync("Scene3");


        while (!asyncLoad1.isDone)
        {
            yield return null;
        }
    }

    public void Startbutton()
    {
        StartCoroutine("LoadGameSceneAsync");
    }
    public void Creditbutton()
    {
        StartCoroutine("LoadCreditsSceneAsync");
    }
}
