using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
	public static event Action<BallController> onBallThrow;
	public static void OnBallThrow(BallController ball)
	{
		onBallThrow?.Invoke(ball);
	}

	public static event Action<bool, int> onLevelFinish;
	public static void OnLevelFinish(bool win, int ballsUsed)
	{
		// TODO: Trigger failure screen!
		Debug.Log("No more balls remaining! Level ending...");

		onLevelFinish?.Invoke(win, ballsUsed);
	}
}