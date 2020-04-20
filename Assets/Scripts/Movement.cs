using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class Movement
	{
		public float Speed = 2.5f;
		private readonly Transform _transform;
		private readonly ChildAnimatorHelper _childAnimatorHelper;
		private readonly Animator _animator;

		[Obsolete("This constructor is deprecated. Please use the one with Animator instead")]
		public Movement(Transform transform, ChildAnimatorHelper childAnimatorHelper)
		{
			this._transform = transform;
			this._childAnimatorHelper = childAnimatorHelper;
		}

		public Movement(Transform transform, Animator animator)
		{
			this._transform = transform;
			this._animator = animator;
		}

		public void MoveInDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.North:
				{
					this.SetAnimatorParameter(Constants.AnimatorParameters.Vertical, 1f);
					this.SetAnimatorParameter(Constants.AnimatorParameters.Horizontal, 0f);
					this._transform.position += new Vector3(0f, this.Speed * Time.deltaTime);
					break;
				}

				case Direction.South:
				{
					this.SetAnimatorParameter(Constants.AnimatorParameters.Vertical, -1f);
					this.SetAnimatorParameter(Constants.AnimatorParameters.Horizontal, 0f);
					this._transform.position += new Vector3(0f, -this.Speed * Time.deltaTime);
					break;
				}

				case Direction.East:
				{
					this.SetAnimatorParameter(Constants.AnimatorParameters.Vertical, 0f);
					this.SetAnimatorParameter(Constants.AnimatorParameters.Horizontal, 1f);
					this._transform.position += new Vector3(this.Speed * Time.deltaTime, 0f);
					break;
				}

				case Direction.West:
				{
					this.SetAnimatorParameter(Constants.AnimatorParameters.Vertical, 0f);
					this.SetAnimatorParameter(Constants.AnimatorParameters.Horizontal, -1f);
					this._transform.position += new Vector3(-this.Speed * Time.deltaTime, 0f);
					break;
				}

				case Direction.None:
				{

					this.SetAnimatorParameter(Constants.AnimatorParameters.Vertical, 0f);
					this.SetAnimatorParameter(Constants.AnimatorParameters.Horizontal, 0f);
					break;
				}
			}
		}

		private void SetAnimatorParameter(string name, float value)
		{
			if (this._animator != null)
			{
				this._animator.SetFloat(name, value);
				return;
			}
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