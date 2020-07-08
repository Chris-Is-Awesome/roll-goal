using System.Collections.Generic;
using UnityEngine;
using ASG;

public class Debugger : Singleton<Debugger>
{
	[Header("Refs")]
	private LevelData level;
	[Header("Cheats")]
	public bool grantBall = false;
	public bool deleteAll = false;
	public bool infiniteBalls = false;
	public bool noBounceDecrement = false;
	public int ballsRemaining = 0;
	[Header("Balls")]
	[CustomAttributes.ReadOnly] public bool ballInHand = false;
	[CustomAttributes.ReadOnly] public List<GameObject> activeBalls = new List<GameObject>();
	[Header("Level")]
	[CustomAttributes.ReadOnly] public int ballsUsed = 0;
	[CustomAttributes.ReadOnly] public int bounces = 0;
	[CustomAttributes.ReadOnly] public float distance = 0f;
	[CustomAttributes.ReadOnly] public float time = 0f;

	void Awake()
	{
		if (Application.isEditor)
			level = Utility.GetLevelData();
	}

	void Update()
	{
		if (Application.isEditor)
		{
			// Grant ball
			if (Input.GetKeyDown(KeyCode.Space) || grantBall)
				Cheat_GrantBall();

			// Destroy all balls
			if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace) || deleteAll)
				RemoveAllBalls();

			// Infinite balls
			if (infiniteBalls && level.ballsRemaining < 999)
				level.ballsRemaining = 999;
		}
	}

	void Cheat_GrantBall()
	{
		if (!ballInHand)
		{
			level.ballsRemaining++;
			level.GrantBall();
			Debug.Log("CHEAT: New ball granted!");
			grantBall = false;
		}
	}

	void RemoveAllBalls()
	{
		for (int i = 0; i < activeBalls.Count; i++)
		{
			activeBalls[i].GetComponent<BallController>().doDestroy = true;
		}

		Debug.Log("CHEAT: Removing all balls...");
		deleteAll = false;
	}
}