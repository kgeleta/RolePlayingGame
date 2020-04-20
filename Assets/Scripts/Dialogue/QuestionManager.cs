using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dialogue
{
	public class QuestionManager : MonoBehaviour
	{
		public GameObject QuestionsCanvasObject;
		public GameObject UiCanvasObject;
		public Text QuestionText;
		public InputField AnswerField;

		private readonly Queue<QuestionEntity> _questionsQueue = new Queue<QuestionEntity>();
		private UnityEvent _onValidationFail;
		private UnityEvent _onQuestionsExit;
		private QuestionEntity _currentQuestion;

		public void LoadQuestions(QuestionEntity[] questions, UnityEvent onValidationFail, UnityEvent onQuestionsExit)
		{
			// save event that should be invoked after dialogue is finished
			this._onQuestionsExit = onQuestionsExit;
			this._onValidationFail = onValidationFail;

			// activate canvas
			this.QuestionsCanvasObject.SetActive(true);
			this.UiCanvasObject.SetActive(false);

			// load dialogue to queue
			this._questionsQueue.Clear();
			foreach (var question in questions)
			{
				this._questionsQueue.Enqueue(question);
			}

			// Display first
			this.DisplayNextQuestion();
		}

		public void ValidateAnswerAndDisplayNextQuestion()
		{
			if (!this._currentQuestion.Validate(this.AnswerField.text))
			{
				this.QuestionsCanvasObject.SetActive(false);
				this.UiCanvasObject.SetActive(true);

				this._onValidationFail.Invoke();
				return;
			}

			this.DisplayNextQuestion();
		}

		private void DisplayNextQuestion()
		{
			if (this._questionsQueue.Count == 0)
			{
				this.QuestionsCanvasObject.SetActive(false);
				this.UiCanvasObject.SetActive(true);

				this._onQuestionsExit.Invoke();
				return;
			}

			this.AnswerField.text = string.Empty;
			this._currentQuestion = this._questionsQueue.Dequeue();
			this.QuestionText.text = this._currentQuestion.Question;
		}
	}
}