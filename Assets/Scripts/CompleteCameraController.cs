using UnityEngine;

namespace Assets
{
	public class CompleteCameraController : MonoBehaviour
	{
		private Vector3 _offset;

		public GameObject Player;

		void Start()
		{
			this._offset = transform.position - this.Player.transform.position;
		}

		void LateUpdate()
		{
			transform.position = this.Player.transform.position + this._offset;
		}
	}
}
