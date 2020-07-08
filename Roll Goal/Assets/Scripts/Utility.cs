using UnityEngine;

public static class Utility
{
    public static LevelData GetLevelData()
	{
		return GameObject.Find("Levels").GetComponentInChildren<LevelData>();
	}
}