using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Dialogue
{
	public class DialogueManager : MonoBehaviour
	{

		public GameObject DialogueCanvasObject;
		public GameObject FaceImageObject;
		public GameObject TextObject;
		public GameObject UiCanvasObject;

		private readonly Queue<DialogueEntity> _dialogueQueue = new Queue<DialogueEntity>();

		private UnityEvent _onDialogueExit;
		
		public void LoadDialogue(DialogueEntity[] dialogue, UnityEvent onDialogueExit)
		{
			// save event that should be invoked after dialogue is finished
			this._onDialogueExit = onDialogueExit;

			// activate canvas
			this.DialogueCanvasObject.SetActive(true);
			this.UiCanvasObject.SetActive(false);

			// load dialogue to queue
			this._dialogueQueue.Clear();
			foreach (var dialogueEntity in dialogue)
			{
				this._dialogueQueue.Enqueue(dialogueEntity);
			}

			// Display first
			this.DisplayDialogue();
		}

		public void DisplayDialogue()
		{
			Debug.Log(nameof(this.DisplayDialogue));
			if (this._dialogueQueue.Count == 0)
			{
				// deactivate canvas
				this.DialogueCanvasObject.SetActive(false);
				this.UiCanvasObject.SetActive(true);
				// invoke event on the end of dialogue
				this._onDialogueExit.Invoke();
				return;
			}

			// set current dialogue
			var currentDialogue = this._dialogueQueue.Dequeue();
			this.FaceImageObject.GetComponent<Image>().sprite = currentDialogue.FaceImage;
			this.TextObject.GetComponent<Text>().text = currentDialogue.DialogueText;
		}

	}
}