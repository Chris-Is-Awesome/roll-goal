﻿using System;
using UnityEngine;

public static class GameEvents
{
	public static event Action<BallController> OnBallThrow;
	public static void OnBallThrown(BallController ball)
	{
		OnBallThrow?.Invoke(ball);
	}
	public static event Action<BallController> OnBallDestroy;
	public static void OnBallDestroyed(BallController ball)
	{
		OnBallDestroy?.Invoke(ball);
	}
	public static event Action<BallCollision> OnBallBounce;
	public static void OnBallBounced(BallCollision ball)
	{
		OnBallBounce?.Invoke(ball);
	}

	public static event Action<bool, int> OnLevelFinish;
	public static void OnLevelFinished(bool win, int ballsUsed)
	{
		LevelData levelData = Utility.GetLevelData();

		if (!levelData.hasFinished)
		{
			if(win)
			{
					Debug.Log("Level completed! Used " + ballsUsed + " balls.");
				}
			else
			{
				Debug.Log("ohno... you ran out of balls :(");
			}
		}

		levelData.hasFinished = true;

		OnLevelFinish?.Invoke(win, ballsUsed);
	}
}