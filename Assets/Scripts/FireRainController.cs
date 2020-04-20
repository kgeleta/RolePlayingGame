using Assets.Scripts;
using UnityEngine;

public class FireRainController : MonoBehaviour, IAttackable
{
	public float fireDelayTime = 2f;
	public float damage = 100f;

	private Animator _animator;
	private SpriteRenderer _redCircleSprite;
	private float _timeToDestroy = 1.5f;
	private IAttackable _player = null;

	void Start()
	{
		this._animator = GetComponentInChildren<Animator>();
		this._redCircleSprite = GetComponent<SpriteRenderer>();
		Invoke(nameof(this.StartFireAnimation), this.fireDelayTime);
	}

	private void StartFireAnimation()
	{
		this._redCircleSprite.enabled = false;
		this._player?.GettingAttacked(this.damage);
		this._animator.SetTrigger(Constants.AnimatorParameters.Start);
		Invoke(nameof(this.SelfDestroy), this._timeToDestroy);
	}

	private void SelfDestroy()
	{
		Destroy(this.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag(Constants.Tags.Player))
		{
			this._player = other.GetComponentInParent<IAttackable>();
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag(Constants.Tags.Player))
		{
			this._player = null;
		}
	}

	public void GettingAttacked(float damageFromEnemy)
	{
		// ignore
	}
}