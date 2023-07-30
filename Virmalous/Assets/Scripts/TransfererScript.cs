using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransfererScript : MonoBehaviour
{
	public static GameObject player;
	public static Animator anim;
	
	void Start()
	{
		anim = GetComponent<Animator>();
	}
	
	public static void StartTransfer(GameObject plr)
	{
		player = plr;
		anim.SetBool("EndTransfer", false);
		anim.SetBool("StartTransfer", true);
	}
	
	public static void EndTransfer()
	{
		anim.SetBool("StartTransfer", false);
		anim.SetBool("EndTransfer", true);
	}
	
	public void StartLoad()
	{
		string currentScene = SceneManager.GetActiveScene().name;
		string nextScene = "LEVEL" + (int.Parse(currentScene.Split("LEVEL")[1]) + 1);
		Debug.Log(nextScene);
		StartCoroutine(LoadSceneAsync(nextScene));
	}
	
	private IEnumerator LoadSceneAsync(string sceneToLoad)
	{
		// Create an operation to load the scene asynchronously
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);

		// Disable scene activation to prevent it from being shown prematurely
		asyncOperation.allowSceneActivation = false;

		// Wait until the scene is loaded
		while (!asyncOperation.isDone)
		{
			// Check if the loading progress is 90% or more
			if (asyncOperation.progress >= 0.90f)
			{
				// Enable scene activation to show the loaded scene
				asyncOperation.allowSceneActivation = true;
			}

			yield return null;
		}
	}
}
