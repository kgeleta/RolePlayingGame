using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class BeerButton : MonoBehaviour
	{
		public GameObject Shrek;
		public int MaxNumberOfUse = 5;

		private int _numberOfUseLeft;
		private Text _text;
		private PlayerController _player;

		void Start()
		{
			this._text = GetComponentInChildren<Text>();
			this._player = Shrek.GetComponent<PlayerController>();

			this._numberOfUseLeft = this.MaxNumberOfUse;
			this._text.text = this._numberOfUseLeft.ToString();
		}

		public void OnClick()
		{
			if (this._numberOfUseLeft == 0)
			{
				return;
			}

			this._text.text = (--this._numberOfUseLeft).ToString();
			this._player.Heal();
		}

		public void RestockBeer()
		{
			this._numberOfUseLeft = this.MaxNumberOfUse;
			this._text.text = this._numberOfUseLeft.ToString();
		}
	}
}