using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class EnemyController : MonoBehaviour
	{
		public GameObject PointToFollow;
		public GameObject Center;
		public float Threshold = 0.6f;
		public float AttackInterval = 1; //1 attack every second

		private List<Renderer> _renderers = new List<Renderer>();
		private ChildAnimatorHelper _childAnimatorHelper;
		private Movement _movement;
		private PointFollower _pointFollower;
		private Direction _direction = Direction.None;
		private float _elapsedTimeAttack;

		private string _lastDirectionToTurn = string.Empty;


		// Start is called before the first frame update
		void Start()
		{
			this.GetComponent<Rigidbody2D>().freezeRotation = true;
			this._childAnimatorHelper = GetComponent<ChildAnimatorHelper>();
			this._movement = new Movement(this.transform, this._childAnimatorHelper);
			this._pointFollower = new PointFollower(this.PointToFollow, this.Center)
			{
				Threshold = this.Threshold
			};
			this.InitializeRenderers();
		}

		// Update is called once per frame
		void Update()
		{
			this._direction = this._pointFollower.GetDirection();
			this.ChangeLayer(this._pointFollower.GetLayerName());

			if (this._direction == Direction.None)
			{
				if (Time.time > this._elapsedTimeAttack)
				{
					// do the attack here
					this._childAnimatorHelper.Animators.ForEach(animator =>
						animator.SetTrigger(Constants.AnimatorParameters.Hit));
					this._elapsedTimeAttack = Time.time + AttackInterval;
				}

				// turning to face PointToFollow
				var deltaY = this.PointToFollow.transform.position.y - this.Center.transform.position.y;
				var deltaX = this.PointToFollow.transform.position.x - this.Center.transform.position.x;

				var directionToTurn = Math.Abs(deltaX) < Math.Abs(deltaY)
					? (deltaY > 0 ? Constants.AnimatorParameters.TurnUp : Constants.AnimatorParameters.TurnDown)
					: (deltaX > 0 ? Constants.AnimatorParameters.TurnRight : Constants.AnimatorParameters.TurnLeft);

				if (this._lastDirectionToTurn != directionToTurn)
				{
					this._childAnimatorHelper.Animators.ForEach(animator => animator.SetTrigger(directionToTurn));
				}

				this._lastDirectionToTurn = directionToTurn;
			}

			this._movement.MoveInDirection(this._direction);
		}

		private void ChangeLayer(string layer)
		{
			foreach (var renderer in _renderers)
			{
				renderer.sortingLayerName = layer;
			}
		}

		private void InitializeRenderers()
		{
			this._renderers.Add(this.GetComponent<Renderer>());
			this._renderers.AddRange(this.GetComponentsInChildren<Renderer>());
		}
	}
}
