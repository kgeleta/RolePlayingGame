using System.Linq.Expressions;
using UnityEngine;

namespace Assets.Scripts
{
	public class PlayerMovement : MonoBehaviour
	{
		private float _threshold = 0.3f;
		private Movement _movement;
		private Direction _moveDirection = Direction.None;

		// Start is called before the first frame update
		void Start()
		{
			GetComponent<Rigidbody2D>().freezeRotation = true;
			this._movement = new Movement(this.transform, GetComponent<ChildAnimatorHelper>());
		}

		// Update is called once per frame
		void Update()
		{
			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");

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
