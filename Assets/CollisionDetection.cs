using Assets.Scripts;
using UnityEngine;

namespace Assets
{
	public class CollisionDetection : MonoBehaviour
	{
		private Animator _animator;
		private bool _IsPlayerInsideCollider;

		// Start is called before the first frame update
		void Start()
		{
			this._animator = GetComponent<Animator>();
			this._IsPlayerInsideCollider = false;
		}

		// Update is called once per frame
		void Update()
		{
			if (this._IsPlayerInsideCollider)
			{
				if (Input.GetButton("A"))
				{
					this._animator.SetTrigger(Constants.AnimatorParameters.INTERACTION);
				}
			}
		}

		void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.CompareTag(Constants.Tags.PLAYER))
			{
				this._IsPlayerInsideCollider = true;
			}
		}

		void OnCollisionExit2D(Collision2D other)
		{
			if (other.gameObject.CompareTag(Constants.Tags.PLAYER))
			{
				this._IsPlayerInsideCollider = false;
			}
		}
	}
}
