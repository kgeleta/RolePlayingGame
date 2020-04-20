using Assets.Scripts;
using UnityEngine;

namespace Assets
{
	public class VillagerFactory : MonoBehaviour
	{
		public GameObject Donkey;
		public GameObject Shrek;
		private bool _isActive = false;

		void Update()
		{
			if (!this._isActive)
			{
				return;
			}

			if (!this.ActiveChildExist())
			{
				if (this.transform.childCount > 0)
				{
					transform.GetChild(0).gameObject.SetActive(true);
				}
				else
				{
					// All villagers are dead
					this.Donkey.transform.position = this.Shrek.transform.position + new Vector3(2f, -2f);
					this.Donkey.gameObject.SetActive(true);
					Destroy(gameObject);
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(Constants.Tags.Player) && !this._isActive)
			{
				this.Donkey.gameObject.SetActive(false);
				this._isActive = true;
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
	}
}