﻿using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class FollowerMovement : MonoBehaviour
	{
		private const int FrontLayer = 8;
		private const int BackgroundLayer = 4;

		private Renderer _renderer;
		private Movement _movement;
		private Direction _direction = Direction.None;

		public GameObject PointToFollow;
		public GameObject Center;
		public float Threshold = 0.6f;

		// Start is called before the first frame update
		void Start()
		{
			this.GetComponent<Rigidbody2D>().freezeRotation = true;
			this._movement = new Movement(this.transform, GetComponent<ChildAnimatorHelper>());
			this._renderer = GetComponent<Renderer>();
		}

		// Update is called once per frame
		void Update()
		{
			UpdateDirection();
			this._movement.MoveInDirection(this._direction);
		}

		private void UpdateDirection()
		{
			// get distance
			var distance = Vector3.Distance(this.Center.transform.position, this.PointToFollow.transform.position);
			var deltaY = this.PointToFollow.transform.position.y - this.Center.transform.position.y;
			var deltaX = this.PointToFollow.transform.position.x - this.Center.transform.position.x;

			// change order in layer
			this.ChangeOrderInLayer(deltaY);

			// Check if point to follow is in distance greater then threshold
			if (distance <= this.Threshold)
			{
				this._direction = Direction.None;
				return;
			}

			// choose direction

			this._direction = Math.Abs(deltaX) > Math.Abs(deltaY)
				? (deltaX > 0 ? Direction.East : Direction.West)
				: (deltaY > 0 ? Direction.North : Direction.South);
		}

		private void ChangeOrderInLayer(float deltaY)
		{
			this._renderer.sortingOrder = deltaY > 0 ? FrontLayer : BackgroundLayer;
		}
	}
}