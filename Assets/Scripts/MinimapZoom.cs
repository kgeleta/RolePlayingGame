using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts
{
	public class MinimapZoom : MonoBehaviour
	{
		public int MaxZoom = -40;
		public int MinZoom = -4;
		public int ZoomStep = 2;

		private Camera _minimapCamera;

		public void Start()
		{
			this._minimapCamera = GetComponent<Camera>();
		}

		public void ZoomIn()
		{
			if (this._minimapCamera.transform.position.z + ZoomStep > MinZoom)
			{
				return;
			}

			this._minimapCamera.transform.position += Vector3.forward * ZoomStep;
		}

		public void ZoomOut()
		{

			if (this._minimapCamera.transform.position.z - ZoomStep < MaxZoom)
			{
				return;
			}

			this._minimapCamera.transform.position -= Vector3.forward * ZoomStep;
		}
	}
}
