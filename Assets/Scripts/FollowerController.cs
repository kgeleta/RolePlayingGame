using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class FollowerController : MonoBehaviour
	{
		private List<Renderer> _renderers = new List<Renderer>();
		private Movement _movement;
		private PointFollower _pointFollower;
		private Direction _direction = Direction.None;

		public GameObject PointToFollow;
		public GameObject Center;
		public float Threshold = 0.6f;

		// Start is called before the first frame update
		void Start()
		{
			this.GetComponent<Rigidbody2D>().freezeRotation = true;
			this._movement = new Movement(this.transform, GetComponent<ChildAnimatorHelper>());
			this._pointFollower = new PointFollower(this.PointToFollow, this.Center);
			this._pointFollower.Threshold = this.Threshold;
			this.InitializeRenderers();
		}

		// Update is called once per frame
		void Update()
		{
			this._direction = this._pointFollower.GetDirection();
			this.ChangeLayer(this._pointFollower.GetLayerName());
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
