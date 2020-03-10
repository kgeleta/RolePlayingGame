using System.Linq.Expressions;
using UnityEngine;

namespace Assets.Scripts
{
	public class PlayerController : MonoBehaviour
	{
		private float _threshold = 0.3f;
		private Movement _movement;
		private Direction _moveDirection = Direction.None;
		private ChildAnimatorHelper _childAnimatorHelper;

		// Start is called before the first frame update
		void Start()
		{
			GetComponent<Rigidbody2D>().freezeRotation = true;
			this._childAnimatorHelper = GetComponent<ChildAnimatorHelper>();
			this._movement = new Movement(this.transform, this._childAnimatorHelper);
		}

		// Update is called once per frame
		void Update()
		{
			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");
			var hitButtonDown = Input.GetButtonDown("A");

			if (hitButtonDown)
			{
				this._childAnimatorHelper.Animators.ForEach(animator => animator.SetTrigger(Constants.AnimatorParameters.Hit));
			}

			if (horizontal > this._threshold)
			{
				this._moveDirection = Direction.East;
			}
			else if (horizontal < -this._threshold)
			{
				this._moveDirection = Direction.West;
			}
			else if (vertical > this._threshold)
			{
				this._moveDirection = Direction.North;
			}
			else if (vertical < -this._threshold)
			{
				this._moveDirection = Direction.South;
			}
			else
			{
				this._moveDirection = Direction.None;
			}

			this._movement.MoveInDirection(this._moveDirection);
		}
	}

}
