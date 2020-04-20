using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class AttackController : MonoBehaviour
	{
		private readonly List<Collider2D> _collidersInAttackRange = new List<Collider2D>();

		public void Attack(string enemyTag, float damage)
		{
			foreach (var collider in _collidersInAttackRange)
			{
				if (collider.CompareTag(enemyTag))
				{
					var attackable = collider.GetComponentInParent<IAttackable>();
					attackable.GettingAttacked(damage);
				}
			}
		}

		public void UpdatePosition(Direction direction)
		{
			switch (direction)
			{
				case Direction.North:
					this.transform.localPosition = new Vector3(-0.141f, 0.05f);
					break;
				case Direction.South:
					this.transform.localPosition = new Vector3(-0.141f, -0.15f);
					break;
				case Direction.West:
					this.transform.localPosition = new Vector3(-0.3f, -0.06f);
					break;
				case Direction.East:
					this.transform.localPosition = new Vector3(0.03f, -0.06f);
					break;
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (!_collidersInAttackRange.Contains(other)) { _collidersInAttackRange.Add(other); }
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			_collidersInAttackRange.Remove(other);
		}
	}
}