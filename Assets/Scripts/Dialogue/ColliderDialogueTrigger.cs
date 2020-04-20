using UnityEngine;

namespace Assets.Scripts.Dialogue
{
	public class ColliderDialogueTrigger : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(Constants.Tags.Player))
			{
				GetComponent<DialogueScreenplay>().StartDialogue();
				// this way dialogue is triggered only once
				Destroy(gameObject);
			}
		}
	}
}