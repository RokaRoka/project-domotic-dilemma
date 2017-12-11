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
		loadTextUI.enabled = true;

        currentAsyncLoad = SceneManager.LoadSceneAsync("Art Testing");

        Debug.Log("Woah");

        while (!currentAsyncLoad.isDone)
        {
            yield return null;
        }
    }
    IEnumerator LoadCreditsSceneAsync()
    {
		loadTextUI.enabled = true;

		AsyncOperation asyncLoad1 = SceneManager.LoadSceneAsync("Credits");


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

	public void ExitButton()
	{
		Application.Quit();
	}
}
