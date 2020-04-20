using Assets.Scripts;

namespace Assets
{
	public class AnimatorHelper
	{
		public static string GetAnimatorParameterFromDirection(Direction direction)
		{
			switch (direction)
			{
				case Direction.North:
					return Constants.AnimatorParameters.TurnUp;
				case Direction.South:
					return Constants.AnimatorParameters.TurnDown;
				case Direction.West:
					return Constants.AnimatorParameters.TurnLeft;
				case Direction.East:
					return Constants.AnimatorParameters.TurnRight;
			}

			return string.Empty;
		}
	}
}