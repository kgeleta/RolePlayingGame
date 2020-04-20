using UnityEngine;

namespace Assets.Scripts
{
	public class PhoneyDragonController : MonoBehaviour, IAttackable
	{
		public Transform PointToFollow;
		public Transform FireBreath;

		private float _attackInterval = 10f;
		private float _attackDuration = 3f;
		private float _freezeRotationTime = 0.5f;
		private FireController _fireController;
		private bool _isDead = false;
		private SpriteRenderer _sprite;
		private float _alphaStep = 0.01f;
		private float _currentAlphaLevel = 1f;
		private bool _freezeRotation = false;

		void Start()
		{
			this._fireController = GetComponentInChildren<FireController>();
			this._sprite = GetComponent<SpriteRenderer>();
			Invoke(nameof(this.FreezeRotationAndStartFire), 1.0f);
		}

		void Update()
		{

			if (!this._freezeRotation)
			{
				// follow player and update angle
				Vector2 delta = this.FireBreath.position - this.PointToFollow.position;

				float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg - 90f;
				Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
				this.FireBreath.rotation = Quaternion.Slerp(this.FireBreath.rotation, q, Time.deltaTime * 5.0f);
			}

			if (this._isDead && !this._fireController.IsAttacking)
			{
				this._currentAlphaLevel -= this._alphaStep;
				if (this._currentAlphaLevel <= 0f)
				{
					Destroy(this.gameObject);
				}

				this._sprite.color = new Color(1f, 1f, 1f, this._currentAlphaLevel);
			}
		}

		public void GettingAttacked(float damage)
		{
			Debug.Log("attack");
			this.KillDragon();
			this._fireController.EndFire();
		}

		public void KillDragon()
		{
			this._isDead = true;
		}

		private void FreezeRotationAndStartFire()
		{
			if (this._isDead)
			{
				return;
			}

			this._freezeRotation = true;
			Invoke(nameof(this.StartFire), this._freezeRotationTime);
		}

		private void StartFire()
		{
			this._fireController.StartFire();
			this._attackDuration = Random.Range(2.0f, 5.0f);
			Invoke(nameof(this.EndFireAttack), this._attackDuration);
		}

		private void EndFireAttack()
		{
			this._freezeRotation = false;
			this._fireController.EndFire();
			this._attackInterval = Random.Range(1.0f, 4.0f);
			if (this._isDead)
			{
				return;
			}
			Invoke(nameof(this.FreezeRotationAndStartFire), this._attackInterval);
		}
	}
}