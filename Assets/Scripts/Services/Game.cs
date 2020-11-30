using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class Game 
{
	//<summary> текущий игрок </summary>
	static public PlayerScript player;

	//<summary> уровни </summary>
	public enum LevelType
	{
		Level1 = 1,
		Level2 = 2,
		Level3 = 3,
		Level4 = 4,
		Level5 = 5,
		Level6 = 6,
		Level7 = 7,
		Level8 = 8
	}
}