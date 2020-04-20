using Assets;
using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DragonController : MonoBehaviour, IAttackable
{
	public enum AttackPhase
	{
		None,
		Phase1,
		Phase2,
		Phase3,
	}

	public Image HealthFillImage;
	public GameObject PointToFollow;
	public GameObject Center;
	public float AttackInterval = 10f;
	public float AttackDuration = 3f;
	public float MaxHealth = 3_000f;

	private Direction _direction = Direction.None;
	private bool _isFacingSouth = false;
	private Movement _movement;
	private PointFollower _pointFollower;
	private Animator _animator;
	private FireController _fireController;
	private AttackPhase _currentAttackPhase = AttackPhase.None;
	private bool _canAttack = true;
	private float _currentHealth;

	// phase 2
	public GameObject PhoneyDragonPrefab;
	public GameObject FireBreath;

	private PhoneyDragonController _phoneyDragon1;
	private PhoneyDragonController _phoneyDragon2;
	private bool _changeDragonsPosition = false;
	private readonly float _shuffleDragonsInterval = 25f;
	private bool _freezeRotation = false;
	private readonly float _freezeRotationTime = 0.5f;
	private bool _stopFire = false;

	// phase 3
	public GameObject fireRainPrefab;
	public GameObject blockEntryAndDragon;

	private const int MaxFireRainRepeat = 5;
	private int _currentFireRainRepeat = 0;
	private bool _isDead = false;

	private SpriteRenderer _sprite;
	private float _alphaStep = 0.01f;
	private float _currentAlphaLevel = 1f;
	private float _phase3AttackInterval;
	private float _phase3AttackDuration;

	private enum DragonPosition
	{
		Left,
		Center,
		Right,
	}

	void Start()
	{
		this._currentHealth = this.MaxHealth;
		this.UpdateHealthBar();
		this.GetComponent<Rigidbody2D>().freezeRotation = true;
		this._sprite = GetComponent<SpriteRenderer>();
		this._animator = GetComponent<Animator>();
		this._movement = new Movement(this.transform, this._animator);
		this._movement.Speed *= 0.9f;
		this._pointFollower = new PointFollower(this.PointToFollow, this.Center, true);
		this._fireController = GetComponentInChildren<FireController>();
	}

	void Update()
	{
		this.CheckHealthAndUpdateCurrentAttackPhase();

		if (this._isDead)
		{
			this._currentAlphaLevel -= this._alphaStep;
			if (this._currentAlphaLevel <= 0f)
			{
				Destroy(this.blockEntryAndDragon.gameObject);
				Destroy(this.gameObject);
			}

			this._sprite.color = new Color(1f, 1f, 1f, this._currentAlphaLevel);
		}

		switch (this._currentAttackPhase)
		{
			case AttackPhase.None:
				return;
			case AttackPhase.Phase1:
				this.Phase1Attack();
				break;
			case AttackPhase.Phase2:
				this.Phase2Attack();
				break;
			default:
				return;
		}
	}

	#region PHASE1
	public void StartPhase1()
	{
		Debug.Log("Start phase 1");
		this._currentAttackPhase = AttackPhase.Phase1;
	}

	private void Phase1Attack()
	{
		// move only when it's not attacking
		if (!this._fireController.IsAttacking)
		{
			this._direction = this._pointFollower.GetDirection();
			this._movement.MoveInDirection(this._direction);
		}

		// when not moving turn south
		if (this._direction == Direction.None)
		{
			if (!this._isFacingSouth)
			{
				this._animator.SetTrigger(AnimatorHelper.GetAnimatorParameterFromDirection(Direction.South));
				this._isFacingSouth = true;
			}
		}
		else
		{
			this._isFacingSouth = false;
		}

		// when dragon is not moving and can attack is set to true -> attack
		if (this._direction == Direction.None && this._canAttack)
		{
			// attack
			this._canAttack = false;
			this._fireController.StartFire();

			Invoke(nameof(this.CallEndFire), this.AttackDuration);
		}
	}

	private void SetCanAttackTrue()
	{
		this._canAttack = true;
	}

	private void CallEndFire()
	{
		this._fireController.EndFire();
		Invoke(nameof(this.SetCanAttackTrue), this.AttackInterval);
	}
	#endregion

	#region PHASE2
	private void StartPhase2()
	{
		this._movement.MoveInDirection(Direction.None);
		this._animator.SetTrigger(AnimatorHelper.GetAnimatorParameterFromDirection(Direction.South));

		this._currentAttackPhase = AttackPhase.Phase2;
		Invoke(nameof(this.FreezeRotationAndStartFire), 1f);
		InvokeRepeating(nameof(this.ShuffleDragons), 0f, this._shuffleDragonsInterval);
	}

	private void Phase2Attack()
	{
		if (!this._freezeRotation)
		{
			// follow player and update angle
			Vector2 delta = this.FireBreath.transform.position - this.PointToFollow.transform.position;

			float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg - 90f;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			this.FireBreath.transform.rotation = Quaternion.Slerp(this.FireBreath.transform.rotation, q, Time.deltaTime * 5.0f);
		}

		if (this._phoneyDragon1 == null && this._phoneyDragon2 == null)
		{
			this._stopFire = true;
		}

		if (this._changeDragonsPosition && this._phoneyDragon1 == null && this._phoneyDragon2 == null && !this._fireController.IsAttacking)
		{
			Debug.Log("changing dragon position");

			var realDragonPosition = this.MoveRealDragonToRandomPosition();
			this.SpawnPhoneyDragons(realDragonPosition);
			this._stopFire = false;
			Invoke(nameof(this.FreezeRotationAndStartFire), 1f);
			this._changeDragonsPosition = false;
		}
	}

	private DragonPosition MoveRealDragonToRandomPosition()
	{
		DragonPosition newPosition = (DragonPosition) Random.Range(0, 3);
		this.transform.position = GetVectorFromPosition(newPosition);
		return newPosition;
	}

	private void SpawnPhoneyDragons(DragonPosition positionOfRealDragon)
	{
		for (int i = 0; i < 3; i++)
		{
			if (i != (int) positionOfRealDragon)
			{
				var phoneyDragonController = Instantiate(this.PhoneyDragonPrefab, this.GetVectorFromPosition((DragonPosition) i), Quaternion.identity).GetComponent<PhoneyDragonController>();
				phoneyDragonController.PointToFollow = this.PointToFollow.transform;

				if (this._phoneyDragon1 == null)
				{
					this._phoneyDragon1 = phoneyDragonController;
				}
				else
				{
					this._phoneyDragon2 = phoneyDragonController;
				}
			}
		}
	}

	private Vector3 GetVectorFromPosition(DragonPosition position)
	{
		switch (position)
		{
			case DragonPosition.Left:
				return new Vector3(-2f, 1.6f, 0f);
			case DragonPosition.Center:
				return new Vector3(4f, 1.6f, 0f);
			case DragonPosition.Right:
				return new Vector3(10f, 1.6f, 0f);
			default:
				throw new NotImplementedException("There are only 3 valid dragon positions");
		}
	}

	private void ShuffleDragons()
	{
		Debug.Log("Shuffle dragons");
		this._phoneyDragon1?.KillDragon();
		this._phoneyDragon2?.KillDragon();
		this._stopFire = true;

		this._changeDragonsPosition = true;
	}

	private void FreezeRotationAndStartFire()
	{
		if (this._stopFire)
		{
			return;
		}

		this._freezeRotation = true;
		Invoke(nameof(this.StartFire), this._freezeRotationTime);
	}

	private void StartFire()
	{
		this._fireController.StartFire();
		this.AttackDuration = Random.Range(2.0f, 5.0f);
		Invoke(nameof(this.EndFireAttack), this.AttackDuration);
	}

	private void EndFireAttack()
	{
		this._freezeRotation = false;
		this._fireController.EndFire();
		this.AttackInterval = Random.Range(1.0f, 4.0f);
		if (this._stopFire)
		{
			return;
		}

		Invoke(nameof(this.FreezeRotationAndStartFire), this.AttackInterval);
	}
	#endregion

	#region PHASE3

	private void StartPhase3()
	{
		if (this._isDead)
		{
			return;
		}

		this._stopFire = true;
		this.EndFireAttack();

		if (this._phoneyDragon1 != null)
		{
			Destroy(this._phoneyDragon1.gameObject);
		}

		if (this._phoneyDragon2 != null)
		{
			Destroy(this._phoneyDragon2.gameObject);
		}
		
		this._phase3AttackInterval = 7f;
		this._phase3AttackDuration = 0.6f;

		Invoke(nameof(this.SpawnFireRainInChain), 2f);
	}

	private void SpawnFireRainInChain()
	{
		if (this._isDead)
		{
			return;
		}

		Instantiate(this.fireRainPrefab, this.PointToFollow.transform.position, Quaternion.identity);

		float timeInterval = this._phase3AttackDuration;
		if (this._currentFireRainRepeat++ >= MaxFireRainRepeat)
		{
			this._currentFireRainRepeat = 0;
			timeInterval = this._phase3AttackInterval;
		}

		Invoke(nameof(this.SpawnFireRainInChain), timeInterval);
	}

	#endregion

	public void GettingAttacked(float damage)
	{
		this._currentHealth -= damage;
		this.UpdateHealthBar();
	}

	private void CheckHealthAndUpdateCurrentAttackPhase()
	{
		if (this._currentHealth <= 2f * this.MaxHealth / 3f && this._currentAttackPhase == AttackPhase.Phase1)
		{
			this._currentAttackPhase = AttackPhase.Phase2;
			this.StartPhase2();
		}

		if (this._currentHealth <= this.MaxHealth/3f && this._currentAttackPhase == AttackPhase.Phase2)
		{
			this._currentAttackPhase = AttackPhase.Phase3;
			this.StartPhase3();
		}

		if (this._currentHealth <= 0)
		{
			this._isDead = true;
		}
	}

	private void UpdateHealthBar()
	{
		this.HealthFillImage.fillAmount = this._currentHealth / this.MaxHealth;
	}
}