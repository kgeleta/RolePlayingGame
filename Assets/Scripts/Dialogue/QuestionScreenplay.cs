using Assets.Scripts.Dialogue;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogue
{
	public class QuestionScreenplay : MonoBehaviour
	{
		public QuestionEntity[] Questions;

		/// <summary>
		/// This stores optional list of functions to call when dialogue is finished
		/// </summary>
		public UnityEvent OnQuestionsExit;

		/// <summary>
		/// This stores optional list of functions to call the answer validation fails
		/// </summary>
		public UnityEvent OnValidationFail;

		void Awake()
		{
			if (this.OnQuestionsExit == null)
			{
				this.OnQuestionsExit = new UnityEvent();
			}

			if (this.OnValidationFail == null)
			{
				this.OnValidationFail = new UnityEvent();
			}
		}

		public void StartDialogue()
		{
			FindObjectOfType<QuestionManager>().LoadQuestions(this.Questions, this.OnValidationFail, this.OnQuestionsExit);
		}
	}
}