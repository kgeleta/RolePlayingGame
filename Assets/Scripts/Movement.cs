using UnityEngine;

namespace Assets.Scripts
{
	public class Movement
	{
		public float Speed = 0.02f;
		private readonly Transform _transform;
		private readonly ChildAnimatorHelper _childAnimatorHelper;

		public Movement(Transform transform, ChildAnimatorHelper childAnimatorHelper)
		{
			this._transform = transform;
			this._childAnimatorHelper = childAnimatorHelper;
		}

		public void MoveInDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.North:
				{
					this.SetAnimatorParameter(AnimationConstants.Vertical, 1f);
					this.SetAnimatorParameter(AnimationConstants.Horizontal, 0f);
					this._transform.position += new Vector3(0f, this.Speed);
					break;
				}

				case Direction.South:
				{
					this.SetAnimatorParameter(AnimationConstants.Vertical, -1f);
					this.SetAnimatorParameter(AnimationConstants.Horizontal, 0f);
					this._transform.position += new Vector3(0f, -this.Speed);
					break;
				}

				case Direction.East:
				{
					this.SetAnimatorParameter(AnimationConstants.Vertical, 0f);
					this.SetAnimatorParameter(AnimationConstants.Horizontal, 1f);
					this._transform.position += new Vector3(this.Speed, 0f);
					break;
				}

				case Direction.West:
				{
					this.SetAnimatorParameter(AnimationConstants.Vertical, 0f);
					this.SetAnimatorParameter(AnimationConstants.Horizontal, -1f);
					this._transform.position += new Vector3(-this.Speed, 0f);
					break;
				}

				case Direction.None:
				{

					this.SetAnimatorParameter(AnimationConstants.Vertical, 0f);
					this.SetAnimatorParameter(AnimationConstants.Horizontal, 0f);
					break;
				}
			}
		}

		private void SetAnimatorParameter(string name, float value)
		{
			this._childAnimatorHelper.Animators.ForEach(animator => animator.SetFloat(name, value));
		}
	}

	public enum Direction
	{
		None,
		North,
		South,
		East,
		West,
	}
}