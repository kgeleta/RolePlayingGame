using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class ChildAnimatorHelper : MonoBehaviour
	{
		public List<Animator> Animators { get; } = new List<Animator>();

		void Start()
		{
			this.Animators.Add(GetComponent<Animator>());
			this.CacheChildAnimators();
		}

		private void CacheChildAnimators()
		{
			foreach (Transform child in this.transform)
			{
				var hasAnimator = child.TryGetComponent<Animator>(out var animator);
				if (hasAnimator)
				{
					this.Animators.Add(animator);
				}

			}

		}
	}
}