using UnityEngine;

namespace Assets.Scripts
{
	public class Fight : MonoBehaviour
	{
		private ChildAnimatorHelper _childAnimatorHelper;

		void Start()
		{
			this._childAnimatorHelper = GetComponent<ChildAnimatorHelper>();
		}

		void Update()
		{
			var hitButtonDown = Input.GetButtonDown("A");
			if (hitButtonDown)
			{
				this._childAnimatorHelper.Animators.ForEach(animator => animator.SetTrigger(AnimationConstants.Hit));
			}
		}
	}
}