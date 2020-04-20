using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Dialogue
{
	[Serializable]
	public class DialogueEntity
	{
		public Sprite FaceImage;
		[TextArea(3,10)]
		public string DialogueText;
	}
}