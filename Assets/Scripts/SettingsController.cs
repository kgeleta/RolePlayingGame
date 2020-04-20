using System;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
	private Settings _settings;

	private void Start()
	{
		this._settings = FindObjectOfType<Settings>();
	}

	public void LoadLastScene()
	{
		if (this._settings == null)
		{
			this.LoadMainMenuScene();
		}
		else
		{
			this._settings.LoadSavedScene();
		}
	}

	public void LoadMainMenuScene()
	{
		SceneManager.LoadSceneAsync("MainMenu");
	}
}