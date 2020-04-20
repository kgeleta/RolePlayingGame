using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class PointFollower
	{
		public float Threshold = 0.6f;

		private readonly GameObject _pointToFollow;
		private readonly GameObject _center;
		private readonly bool _disableVerticalMovement;

		public PointFollower(GameObject pointToFollow, GameObject center) : this(pointToFollow, center, false) {}

		public PointFollower(GameObject pointToFollow, GameObject center, bool disableVerticalMovement)
		{
			_pointToFollow = pointToFollow;
			_center = center;
			this._disableVerticalMovement = disableVerticalMovement;
		}

		public Direction GetDirection()
		{
			// get distance
			var deltaX = this._pointToFollow.transform.position.x - this._center.transform.position.x;
			var distance = this._disableVerticalMovement ? Math.Abs(deltaX) : Vector3.Distance(this._center.transform.position, this._pointToFollow.transform.position);
			var deltaY = this._disableVerticalMovement ? 0f : this._pointToFollow.transform.position.y - this._center.transform.position.y;

			// Check if point to follow is in distance greater then threshold
			if (distance <= this.Threshold)
			{
				return Direction.None;
			}

			// choose direction
			return Math.Abs(deltaX) > Math.Abs(deltaY)
				? (deltaX > 0 ? Direction.East : Direction.West)
				: (deltaY > 0 ? Direction.North : Direction.South);
		}

		public string GetLayerName()
		{
			var deltaY = this._pointToFollow.transform.position.y - this._center.transform.position.y;
			return deltaY > 0 ? Constants.Layers.InFrontOfShrek : Constants.Layers.BehindShrek;
		}
	}
}