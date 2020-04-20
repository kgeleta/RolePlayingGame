using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
	public class EnemyController : MonoBehaviour, IAttackable
	{
		public float AttackDamage = 10f;
		public float MaxHealth = 100f;
		public Image HealthBar;
		public GameObject PointToFollow;
		public GameObject Center;
		public float Threshold = 0.6f;
		public float AttackInterval = 1; //1 attack every second

		private AttackController _attackController;
		private List<Renderer> _renderers = new List<Renderer>();
		private ChildAnimatorHelper _childAnimatorHelper;
		private Movement _movement;
		private PointFollower _pointFollower;
		private Direction _direction = Direction.None;
		private float _elapsedTimeAttack;
		private float _currentHealthPoints;
		private Direction _lastDirectionToTurn = Direction.None;
		private bool _isDead = false;

		// Start is called before the first frame update
		void Start()
		{
			this._currentHealthPoints = MaxHealth;
			this._attackController = GetComponentInChildren<AttackController>();
			this.GetComponent<Rigidbody2D>().freezeRotation = true;
			this._childAnimatorHelper = GetComponent<ChildAnimatorHelper>();
			this._movement = new Movement(this.transform, this._childAnimatorHelper);
			this._movement.Speed *= 0.9f;
			this._pointFollower = new PointFollower(this.PointToFollow, this.Center)
			{
				Threshold = this.Threshold
			};
			this.InitializeRenderers();
		}

		// Update is called once per frame
		void Update()
		{

			if (this._isDead)
			{
				this.ChangeLayer(Constants.Layers.BehindShrek);

				var colliders = GetComponentsInChildren<BoxCollider2D>();
				foreach (var boxCollider2D in colliders)
				{
					boxCollider2D.enabled = false;
				}

				GetComponentInChildren<Canvas>().enabled = false;

				return;
			}

			this._direction = this._pointFollower.GetDirection();
			this._attackController.UpdatePosition(this._direction);
			this.ChangeLayer(this._pointFollower.GetLayerName());

			if (this._direction == Direction.None)
			{
				if (Time.time > this._elapsedTimeAttack)
				{
					// do the attack here
					this._childAnimatorHelper.Animators.ForEach(animator =>
						animator.SetTrigger(Constants.AnimatorParameters.Hit));
					this._attackController.Attack(Constants.Tags.Player, this.AttackDamage);
					this._elapsedTimeAttack = Time.time + AttackInterval;
				}

				// turning to face PointToFollow
				var deltaY = this.PointToFollow.transform.position.y - this.Center.transform.position.y;
				var deltaX = this.PointToFollow.transform.position.x - this.Center.transform.position.x;

				var directionToTurn = Math.Abs(deltaX) < Math.Abs(deltaY)
					? (deltaY > 0 ? Direction.North : Direction.South)
					: (deltaX > 0 ? Direction.East : Direction.West);

				if (this._lastDirectionToTurn != directionToTurn)
				{
					this._childAnimatorHelper.Animators.ForEach(animator => animator.SetTrigger(AnimatorHelper.GetAnimatorParameterFromDirection(directionToTurn)));
					this._attackController.UpdatePosition(directionToTurn);
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

		public void GettingAttacked(float damage)
		{
			if (this._isDead)
			{
				return;
			}

			this._currentHealthPoints -= damage;
			if (this._currentHealthPoints < 0f)
			{
				this._currentHealthPoints = 0f;

				this._movement.MoveInDirection(Direction.None);
				this._childAnimatorHelper.Animators.ForEach(animator => animator.SetTrigger(Constants.AnimatorParameters.Death));
				this._isDead = true;

				// this should be called only once
				Invoke(nameof(ReplaceWithBlood), 1.5f);
			}

			this.UpdateHealthBar();
		}

		private void UpdateHealthBar()
		{
			this.HealthBar.fillAmount = this._currentHealthPoints / this.MaxHealth;
		}

		private void ReplaceWithBlood()
		{
			bool loadBlood = FindObjectOfType<Settings>()?.IsBloodActive ?? true;
			if (loadBlood)
			{
				var myPrefab = Resources.Load<GameObject>($"prefabs/blood_{Random.Range(0, 10)}");
				Debug.Log($"loading blood: {myPrefab.name}");
				Instantiate(myPrefab, this.transform.position, Quaternion.identity);
			}

			Destroy(gameObject);
		}
	}
}
