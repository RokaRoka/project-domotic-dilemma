using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButtonScript : MonoBehaviour {

	public void GoBackToMenu()
	{
		SceneManager.LoadScene("StartScreen");
	}
}
