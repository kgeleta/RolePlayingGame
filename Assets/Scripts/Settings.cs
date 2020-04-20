using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
	public bool IsBloodActive = true;
	public float MusicVolume = 0.7f;
	public GameObject PauseCanvasGameObject;

	private readonly List<string> _nonPausableSceneNames = new List<string> { "MainMenu", "GameOver" };
	private bool _isPaused = false;
	private string _savedSceneName = String.Empty;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		SceneManager.sceneLoaded += (scene, _) =>
		{
			Debug.Log(scene.name);
			if (scene.name == "GameOver")
			{
				return;
			}

			this._savedSceneName = scene.name;
		};
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !this.IsCurrentSceneNonPausable())
		{
			if (_isPaused)
			{
				this.Resume();
			}
			else
			{
				this.Pause();
			}
		}
	}

	public void SetBloodActive(bool value)
	{
		this.IsBloodActive = value;
	}

	public void SetMusicVolume(float value)
	{
		this.MusicVolume = value;
	}

	public void LoadSavedScene()
	{
		try
		{
			SceneManager.LoadSceneAsync(this._savedSceneName);
		}
		catch (Exception e)
		{
			Debug.LogError($"Couldn't load scene {this._savedSceneName}. Error: {e.Message}");
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void LoadMainMenuScene()
	{
		SceneManager.LoadSceneAsync("MainMenu");
	}

	public void Resume()
	{
		this.PauseCanvasGameObject.SetActive(false);
		foreach (IPausable pausable in FindObjectsOfType<MonoBehaviour>().OfType<IPausable>())
		{
			pausable.Resume();
		}

		Time.timeScale = 1f;
		this._isPaused = false;
	}

	private void Pause()
	{
		this.PauseCanvasGameObject.SetActive(true);
		foreach (IPausable pausable in FindObjectsOfType<MonoBehaviour>().OfType<IPausable>())
		{
			pausable.Pause();
		}

		Time.timeScale = 0f;
		this._isPaused = true;
	}

	private bool IsCurrentSceneNonPausable()
	{
		string sceneName = SceneManager.GetActiveScene().name;
		return this._nonPausableSceneNames.Contains(sceneName);
	}
}