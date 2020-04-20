using System;
using UnityEngine;

namespace Dialogue
{
	[Serializable]
	public class QuestionEntity
	{
		[TextArea(3,10)]
		public string Question;
		[TextArea(3, 10)]
		public string Answer;

		public QuestionEntity(string question, string answer)
		{
			this.Question = question;
			this.Answer = answer;
		}

		public QuestionEntity(string question) : this(question, string.Empty) {}

		public bool Validate(string userAnswer)
		{
			if (userAnswer == string.Empty)
			{
				return false;
			}

			return this.Answer == string.Empty || userAnswer.Equals(this.Answer, StringComparison.CurrentCultureIgnoreCase);
		}
	}
}