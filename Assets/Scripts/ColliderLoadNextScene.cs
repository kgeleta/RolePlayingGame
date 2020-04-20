using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColliderLoadNextScene : MonoBehaviour
{
	public bool Active = false;

	public void IsActive(bool value)
	{
		this.Active = value;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag(Constants.Tags.Player) && this.Active)
		{
			Debug.Log("next scene");
			var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadSceneAsync(currentSceneIndex + 1);
		}
	}
}