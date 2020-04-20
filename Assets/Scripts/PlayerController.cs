using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
	public class PlayerController : MonoBehaviour, IAttackable
	{
		public float HealAmount = 100f;
		public float MaxHealth = 1000f;
		public float AttackDamage = 10f;
		public Image HealthBar;

		private float _currentHealth = 100f;
		private float _threshold = 0.3f;
		private Movement _movement;
		private Direction _moveDirection = Direction.None;
		private ChildAnimatorHelper _childAnimatorHelper;
		private AttackController _attackController;
		private bool _isDead = false;
		private bool _catapult = false;
		private bool _catapultScaleUp = true;
		private bool _gameOverSceneLoaded = false;

		// Start is called before the first frame update
		void Start()
		{
			this._currentHealth = MaxHealth;
			this._attackController = GetComponentInChildren<AttackController>();
			GetComponent<Rigidbody2D>().freezeRotation = true;
			this._childAnimatorHelper = GetComponent<ChildAnimatorHelper>();
			this._movement = new Movement(this.transform, this._childAnimatorHelper);
		}

		// Update is called once per frame
		void Update()
		{
			if (this._isDead)
			{
				if (!this._gameOverSceneLoaded)
				{
					Invoke(nameof(this.LoadGameOverScene), 2f);
					this._gameOverSceneLoaded = true;
				}

				return;
			}

			if (this._catapult)
			{
				if (this.transform.localScale.x <= 0.5f)
				{
					this._catapult = false;
					this.GettingAttacked(this.MaxHealth);
					return;
				}

				this.transform.position += Vector3.left * (Time.deltaTime * 1.3f);

				if (this.transform.localScale.x >= 10f)
				{
					this._catapultScaleUp = false;
				}
				float scaleFactor = this._catapultScaleUp ? 1.01f : 0.99f;
				this.transform.localScale *= scaleFactor;
				return;
			}

			var horizontal = Input.GetAxis("Horizontal");
			var vertical = Input.GetAxis("Vertical");
			var hitButtonDown = Input.GetButtonDown("A");

			if (hitButtonDown)
			{
				this.Attack();
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
			this._attackController.UpdatePosition(this._moveDirection);
		}

		public void Heal()
		{
			this._currentHealth += this.HealAmount;
			this.UpdateHealthBar();
		}

		public void RestoreFullHealth()
		{
			this._currentHealth = this.MaxHealth;
			this.UpdateHealthBar();
		}

		public void GettingAttacked(float damage)
		{
			if (this._isDead)
			{
				return;
			}

			this._currentHealth -= damage;
			if (this._currentHealth <= 0f)
			{
				this._currentHealth = 0f;

				this._movement.MoveInDirection(Direction.None);
				this._childAnimatorHelper.Animators.ForEach(animator => animator.SetTrigger(Constants.AnimatorParameters.Death));
				this._isDead = true;
			}

			this.UpdateHealthBar();
		}

		public void Catapult()
		{
			foreach (var boxCollider2D in GetComponentsInChildren<BoxCollider2D>())
			{
				boxCollider2D.isTrigger = true;
			}

			this._catapult = true;
		}

		private void Attack()
		{
			this._childAnimatorHelper.Animators.ForEach(animator => animator.SetTrigger(Constants.AnimatorParameters.Hit));
			this._attackController.Attack(Constants.Tags.Enemy, this.AttackDamage);
		}
		
		private void UpdateHealthBar()
		{
			this.HealthBar.fillAmount = this._currentHealth / this.MaxHealth;
		}

		private void LoadGameOverScene()
		{
			SceneManager.LoadSceneAsync("Scenes/GameOver");
		}
	}

}
