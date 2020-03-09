using System;
using UnityEngine;

namespace Assets.Scripts
{
	public class SpriteSwapper : MonoBehaviour
	{
		public string spriteSheetName = "metal_helm_male";

		void LateUpdate()
		{
			var subSprites = Resources.LoadAll<Sprite>(spriteSheetName);
			var renderer = GetComponent<SpriteRenderer>();
			string spriteName = renderer.sprite.name;
			var newSprite = Array.Find(subSprites, item => item.name == spriteName);

			if (newSprite)
			{
				renderer.sprite = newSprite;
			}
		}
	}
}
