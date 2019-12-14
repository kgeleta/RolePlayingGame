using UnityEngine;

namespace Assets.Scripts
{
	public class Movement : MonoBehaviour
	{

		private Animator _animator;

		// Start is called before the first frame update
		void Start()
		{
			this._animator = GetComponent<Animator>();
		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetButton("B"))
			{
				Debug.Log("Pressed B");
			}
			if (Input.GetButton("A"))
			{
				Debug.Log("Pressed A");
			}
			if (Input.GetButton("Y"))
			{
				Debug.Log("Pressed Y");
			}
			if (Input.GetButton("X"))
			{
				Debug.Log("Pressed X");
			}

			foreach (Transform child in this.transform)
			{
				var animator = child.GetComponent<Animator>();
				animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
				animator.SetFloat("Vertical", Input.GetAxis("Vertical"));
			}

			this._animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
			this._animator.SetFloat("Vertical", Input.GetAxis("Vertical"));


			//			if (Input.GetAxis("Horizontal") > 0.01f)
			//			{
			//				Debug.Log("Right");
			//			}
			//			if (Input.GetAxis("Horizontal") < -0.01f)
			//			{
			//				Debug.Log("Left");
			//			}
			//			if (Input.GetAxis("Vertical") > 0.01f)
			//			{
			//				Debug.Log("Up");
			//			}
			//			if (Input.GetAxis("Vertical") < -0.01f)
			//			{
			//				Debug.Log("down");
			//			}
		}


	}
}
