using Assets.Scripts.Dialogue;
using UnityEngine;

namespace Assets.Scripts
{
	public class KnightFactory : MonoBehaviour
	{
		public GameObject DialogueAfterTournamentObject;
		public GameObject Music;
		public GameObject Donkey;

		void Update()
		{
			if (!this.ActiveChildExist())
			{
				if (this.transform.childCount > 0)
				{
					transform.GetChild(0).gameObject.SetActive(true);
				}
				else
				{
					// All knights are dead
					Invoke(nameof(ActivateDialogue), 3.0f);
					this.Music.GetComponent<AudioSource>().Pause();
					this.Donkey.gameObject.SetActive(true);
					gameObject.SetActive(false);
				}
			}
		}

		private bool ActiveChildExist()
		{
			foreach (Transform child in transform)
			{
				if (child.gameObject.activeInHierarchy)
				{
					return true;
				}
			}

			return false;
		}

		private void ActivateDialogue()
		{
			this.DialogueAfterTournamentObject.GetComponent<DialogueScreenplay>().StartDialogue();
			Destroy(gameObject);
		}
	}
}