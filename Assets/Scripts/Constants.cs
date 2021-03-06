﻿namespace Assets.Scripts
{
	public static class Constants
	{
		public static class Tags
		{
			public static string Player = "Player";
			public static string Enemy = "Enemy";
		}

		public static class Layers
		{
			public static string BehindShrek = "BehindShrek";
			public static string InFrontOfShrek = "InFrontOfShrek";
		}

		public static class AnimatorParameters
		{
			public static string Interaction = "interaction";
			public static string Horizontal = "Horizontal";
			public static string Vertical = "Vertical";
			public static string Hit = "hit";

			public static string TurnRight = "turn_right";
			public static string TurnLeft = "turn_left";
			public static string TurnUp = "turn_up";
			public static string TurnDown = "turn_down";
			public static string Death = "death";

			// Book animation
			public static string Open = "open";
			public static string Close = "close";

			// Fire animation
			public static string Start = "start";
			public static string End = "end";
		}
	}
}