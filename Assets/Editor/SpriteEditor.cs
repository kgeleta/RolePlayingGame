using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
	// TODO: this class should change sprites names based on the position
	public class SpriteEditor
	{
		private static string _patternFilePath = $@"{Application.dataPath}/Sprites/Characters/Orc/orc.png.meta";
		private static string _sourceFilePath = $@"{Application.dataPath}/Sprites/Armor/metal_helm_male.png.meta";

		[MenuItem("SpriteEditor/ChangePosition")]
		public static void ChangeSpriteNameBasedOnPosition()
		{
			int lineToChange = 626;
			int numberOfLines = File.ReadLines(_sourceFilePath).Count();
			Dictionary<int, string> memoryLines = new Dictionary<int, string>();
			string currentLine = null;

			using (StreamReader reader = new StreamReader(_patternFilePath))
			{
				for (int i = 1; i <= 4360; ++i)
				{
					currentLine = reader.ReadLine();
					if (i == lineToChange)
					{
						memoryLines.Add(i, currentLine);
						for (int j = 1; j < 4; j++)
						{
							i++;
							currentLine = reader.ReadLine();
							memoryLines.Add(i, currentLine);
						}

						lineToChange += 21;
					}
				}

			}

			string tempFilePath = $@"{Application.dataPath}/Sprites/Characters/Orc/dupa.txt";
			using (StreamReader reader = new StreamReader(_sourceFilePath))
			using (StreamWriter writer = new StreamWriter(tempFilePath))
			{
				for (int i = 1; i <= numberOfLines; ++i)
				{
					currentLine = reader.ReadLine();
					if (memoryLines.ContainsKey(i))
					{
						writer.WriteLine(memoryLines[i]);
					}
					else
					{
						writer.WriteLine(currentLine);
					}
				}
			}
		}
	}
}
