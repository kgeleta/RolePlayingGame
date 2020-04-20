using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Dialogue
{
	public class DialogueScreenplay : MonoBehaviour
	{
		public DialogueEntity[] Dialogue;

		/// <summary>
		/// This stores optional list of functions to call when dialogue is finished
		/// </summary>
		public UnityEvent OnDialogueExit;

		void Awake()
		{
			if (OnDialogueExit == null)
				OnDialogueExit = new UnityEvent();
		}

		public void StartDialogue()
		{
			FindObjectOfType<DialogueManager>().LoadDialogue(this.Dialogue, this.OnDialogueExit);
		}
	}
}