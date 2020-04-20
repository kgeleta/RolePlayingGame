using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
	public class SceneLoader : MonoBehaviour
	{
		public void NextScene()
		{
			var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
			if (currentSceneIndex >= SceneManager.sceneCount)
			{
				return;
			}

			SceneManager.LoadScene(currentSceneIndex + 1);
		}

		public void NextSceneAfterTime(float seconds)
		{
			Invoke(nameof(NextScene), seconds);
		}
	}
}