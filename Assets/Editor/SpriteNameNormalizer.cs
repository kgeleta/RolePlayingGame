using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
	public class SpriteNameNormalizer
	{
		private const string NewSpriteName = "sprite";

		[MenuItem("Assets/Normalize sprite name")]
		public static void NormalizeName()
		{
			string oldSpriteName = Selection.activeObject.name;
			string path = GetPathToSelectedObjectMetadata();

			string text = File.ReadAllText(path);
			text = text.Replace(oldSpriteName, NewSpriteName);
			File.WriteAllText(path, text);

			AssetDatabase.Refresh();
		}

		private static string GetPathToSelectedObjectMetadata()
		{
			return $"{Application.dataPath}{AssetDatabase.GetAssetPath(Selection.activeObject).Substring(6)}.meta";
		}
	}
}
