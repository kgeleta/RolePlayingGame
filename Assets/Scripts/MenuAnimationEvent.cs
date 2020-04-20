using UnityEngine;

namespace Assets.Scripts
{
	public class MenuAnimationEvent : MonoBehaviour
	{
		public GameObject TextObject;

		void Start()
		{
			Application.targetFrameRate = 300;
			GetComponent<Animator>().SetTrigger(Constants.AnimatorParameters.Open);
		}

		public void ActivateText()
		{
			this.TextObject.SetActive(true);
		}
	}
}