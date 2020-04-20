using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class FireController : MonoBehaviour
	{
		public float AttackDamage = 50f;
		public float DamageDealInterval = 0.5f; // every 500 ms deal damage

		public Animator _animator;
		private IAttackable _player = null;
		private bool _playerInAttackRange = false;

		private float _elapsedTimeDealDamage;

		void Start() {}

		void Update()
		{
			if (this._playerInAttackRange && Time.time > this._elapsedTimeDealDamage)
			{
				this._player.GettingAttacked(this.AttackDamage);
				this._elapsedTimeDealDamage = Time.time + this.DamageDealInterval;
			}
		}

		public bool IsAttacking { get; private set; }

		public void SetIsAttackingTrue()
		{
			Debug.Log("isAttacking = true");
			this.IsAttacking = true;
		}

		public void SetIsAttackingFalse()
		{
			this.IsAttacking = false;
			Debug.Log("isAttacking = false");
		}

		public void StartFire()
		{
			this._animator.SetTrigger(Constants.AnimatorParameters.Start);
		}

		public void EndFire()
		{
			this._animator.SetTrigger(Constants.AnimatorParameters.End);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(Constants.Tags.Player))
			{
				if (this._player == null)
				{
					this._player = other.GetComponentInParent<IAttackable>();
				}

				this._playerInAttackRange = true;
			}
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.CompareTag(Constants.Tags.Player))
			{
				this._playerInAttackRange = false;
			}
		}
	}
}