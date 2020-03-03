using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class Movement : MonoBehaviour
	{
		private float _threshold = 0.3f;
		private float _step = 0.04f;
		private Animator _animator;
		private Rigidbody2D _rigidbody2D;

		private Direction _direction = Direction.None;

		// Start is called before the first frame update
		void Start()
		{
			this._animator = GetComponent<Animator>();
			this._rigidbody2D = GetComponent<Rigidbody2D>();

			this._rigidbody2D.freezeRotation = true;
		}

		// Update is called once per frame
		void Update()
		{
			//			if (Input.GetButton("B"))
			//			{
			//				Debug.Log("Pressed B");
			//			}
			//			if (Input.GetButton("A"))
			//			{
			//				Debug.Log("Pressed A");
			//			}
			//			if (Input.GetButton("Y"))
			//			{
			//				Debug.Log("Pressed Y");
			//			}
			//			if (Input.GetButton("X"))
			//			{
			//				Debug.Log("Pressed X");
			//			}

			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");

			if (horizontal > this._threshold)
			{
				this._direction = Direction.East;
			}
			else if (horizontal < -this._threshold)
			{
				this._direction = Direction.West;
			}
			else if (vertical > this._threshold)
			{
				this._direction = Direction.North;
			}
			else if (vertical < -this._threshold)
			{
				this._direction = Direction.South;
			}
			else
			{
				this._direction = Direction.None;
			}

			this.MoveInDirection(this._direction);
		}

		private void MoveInDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.North:
				{
					this.SetAnimatorParameter("Vertical", 1f);
					this.SetAnimatorParameter("Horizontal", 0f);
					this.transform.position += new Vector3(0f, this._step);
					break;
				}

				case Direction.South:
				{
					this.SetAnimatorParameter("Vertical", -1f);
					this.SetAnimatorParameter("Horizontal", 0f);
					this.transform.position += new Vector3(0f, -this._step);
					break;
				}

				case Direction.East:
				{
					this.SetAnimatorParameter("Vertical", 0f);
					this.SetAnimatorParameter("Horizontal", 1f);
					this.transform.position += new Vector3(this._step, 0f);
					break;
				}

				case Direction.West:
				{
					this.SetAnimatorParameter("Vertical", 0f);
					this.SetAnimatorParameter("Horizontal", -1f);
					this.transform.position += new Vector3(-this._step, 0f);
					break;
				}

				case Direction.None:
				{

					this.SetAnimatorParameter("Vertical", 0f);
					this.SetAnimatorParameter("Horizontal", 0f);
					break;
				}
			}
		}

		private void SetAnimatorParameter(string name, float value)
		{
			this._animator.SetFloat(name, value);

			foreach (Transform child in this.transform)
			{
				try
				{
					var animator = child.GetComponent<Animator>();
					animator?.SetFloat(name, value);
				}
				catch (MissingComponentException)
				{
					// ignore
				}
			}

		}
	}

	public enum Direction
	{
		North,
		South,
		East,
		West,
		None
	}
}
