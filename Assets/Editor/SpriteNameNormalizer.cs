using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
	public class SpriteNameNormalizer
	{
		private const string NewSpriteName = "sprite";

		private const string MetaPatternLocation =
			"C:\\Users\\Leslie\\RolePlayingGame\\Assets\\Sprites\\Characters\\Shrek\\orc.png.meta";

		private static readonly string[,] PositionToNameMapping = new string[1_000, 1_500];

		private static readonly Regex SpriteNameRegex = new Regex(@"^\s*name: (sprite_[0-9]+)$");
		private static readonly Regex PositionXRegex = new Regex(@"^\s*x: ([0-9]+)$");
		private static readonly Regex PositionYRegex = new Regex(@"^\s*y: ([0-9]+)$");
		private static readonly Regex InternalIdRegex = new Regex(@"^\s*internalID: (-?[0-9]+)$");
		private static readonly Regex InternalIdTableRegex = new Regex(@"^\s*213: (-?[0-9]+)$");

		static SpriteNameNormalizer()
		{
			Debug.Log("Static constructor");

			string tempSpriteName = string.Empty;
			int tempX = 0;

			// read line by line
			var lines = File.ReadLines(MetaPatternLocation);
			foreach (var line in lines)
			{
				Match matchSprite = SpriteNameRegex.Match(line);
				Match matchX = PositionXRegex.Match(line);
				Match matchY = PositionYRegex.Match(line);

				// if match name regex -> save name in temp variable
				if (matchSprite.Success)
				{
					tempSpriteName = matchSprite.Groups[1].Value;
				}
				// if match x -> save in temp
				else if (matchX.Success)
				{
					if (!Int32.TryParse(matchX.Groups[1].Value, out tempX))
					{
						Debug.LogError($"Error occurred while parsing {matchX.Groups[1].Value} to Int32");
					}
				}
				// if match y -> add to table[x][y] = name
				else if (matchY.Success)
				{
					if (Int32.TryParse(matchY.Groups[1].Value, out var tempY))
					{
						PositionToNameMapping[tempX, tempY] = tempSpriteName;
					}
					else
					{
						Debug.LogError($"Error occurred while parsing {matchY.Groups[1].Value} to Int32");
					}
				}
			}
		}

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

		[MenuItem("Assets/Normalize sprite name2")]
		public static void NormalizeNameWithPosition()
		{
			var path = GetPathToSelectedObjectMetadata();
			var lines = File.ReadAllLines(path);

			var internalIdToSpriteNameMapping = CreateInternalIdToSpriteNameMapping(lines);

			for (int i = 0; i < lines.Length; i++)
			{
				Match matchInternalIdTable = InternalIdTableRegex.Match(lines[i]);
				Match matchInternalId = InternalIdRegex.Match(lines[i]);

				if (matchInternalIdTable.Success)
				{
					internalIdToSpriteNameMapping.TryGetValue(matchInternalIdTable.Groups[1].Value,
						out var newSpriteName);
					lines[i + 1] = $"    second: {newSpriteName}";
					i += 2;
				}
				else if (matchInternalId.Success)
				{
					internalIdToSpriteNameMapping.TryGetValue(matchInternalId.Groups[1].Value,
						out var newSpriteName);
					lines[i - 15] = $"      name: {newSpriteName}";
					i += 10;
				}
			}

			File.WriteAllLines(path, lines);
			AssetDatabase.Refresh();
		}

		private static Dictionary<string, string> CreateInternalIdToSpriteNameMapping(string[] lines)
		{
			var internalIdToSpriteNameMapping = new Dictionary<string, string>(200);

			for (int i = 0; i < lines.Length; i++)
			{
				Match matchX = PositionXRegex.Match(lines[i]);
				if (matchX.Success)
				{
					if (!Int32.TryParse(matchX.Groups[1].Value, out var x))
					{
						Debug.LogError($"Error occurred while parsing {matchX.Groups[1].Value} to Int32");
					}

					Match matchY = PositionYRegex.Match(lines[i + 1]);
					if (matchY.Success)
					{
						if (!Int32.TryParse(matchY.Groups[1].Value, out var y))
						{
							Debug.LogError($"Error occurred while parsing {matchY.Groups[1].Value} to Int32");
						}

						Match matchInternalId = InternalIdRegex.Match(lines[i + 12]);
						if (matchInternalId.Success)
						{
							internalIdToSpriteNameMapping.Add(matchInternalId.Groups[1].Value, PositionToNameMapping[x, y]);
							i += 13;
						}
					}
				}
			}

			return internalIdToSpriteNameMapping;
		}

		private static string GetPathToSelectedObjectMetadata()
		{
			return $"{Application.dataPath}{AssetDatabase.GetAssetPath(Selection.activeObject).Substring(6)}.meta";
		}
	}
}
