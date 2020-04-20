using Assets.Scripts;
using UnityEngine;

public class MusicVolumeManager : MonoBehaviour, IPausable
{
	private AudioSource _audioSource;

	void Start()
	{
		this._audioSource = GetComponent<AudioSource>();
		float volume = FindObjectOfType<Settings>()?.MusicVolume ?? 0.7f;
		this.SetMusicVolume(volume);
	}

	public void SetMusicVolume(float volume)
	{
		this._audioSource.volume = volume;
	}

	public void Pause()
	{
		this._audioSource.Pause();
	}

	public void Resume()
	{
		this._audioSource.UnPause();
	}
}