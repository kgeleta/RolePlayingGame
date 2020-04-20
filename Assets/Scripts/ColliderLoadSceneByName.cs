using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
	public class ColliderLoadSceneByName : MonoBehaviour
	{
		public bool Active = false;
		public string SceneName = "scene name";

		public void IsActive(bool value)
		{
			this.Active = value;
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(Constants.Tags.Player) && this.Active)
			{
				Debug.Log($"loading scene {this.SceneName}");
				SceneManager.LoadSceneAsync(this.SceneName);
			}
		}
	}
}