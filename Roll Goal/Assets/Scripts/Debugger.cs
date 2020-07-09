using System.Collections.Generic;
using UnityEngine;
using ASG;

public class Debugger : Singleton<Debugger>
{
	[Header("Refs")]
	private LevelData level;
	[Header("Cheats")]
	public int ballsRemaining;
	[Space]
	public bool grantBall;
	public bool infiniteBalls;
	public bool autoThrow;
	public bool noBallDecay;
	public bool deleteAll;
	[Space]
	public float speedOverride;
	public float frictionOverride;
	public float bounceOverride;
	[Space]
	public bool goToNextLevel;
	public bool goToPrevLevel;
	[Header("Balls")]
	[CustomAttributes.ReadOnly] public bool ballInHand;
	[CustomAttributes.ReadOnly] public List<GameObject> activeBalls = new List<GameObject>();
	[Header("Level")]
	[CustomAttributes.ReadOnly] public int ballsUsed;
	[CustomAttributes.ReadOnly] public int bounces;
	[HideInInspector] public float distanceInFeet;
	[CustomAttributes.ReadOnly] public string speed = "0 ft/s";
	[CustomAttributes.ReadOnly] public string highestSpeed = "0 ft/s";
	[CustomAttributes.ReadOnly] public string distance = "0 feet (0 miles)";
	[CustomAttributes.ReadOnly] public string highestDistance = "0 feet (0 miles)";
	[CustomAttributes.ReadOnly] public float time;

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
			if (Input.GetKeyDown(KeyCode.Space))
				Cheat_GrantBall();

			if (grantBall)
			{
				if (ballInHand)
					grantBall = false;
				else
					Cheat_GrantBall();
			}

			// Destroy all balls
			if (Input.GetKeyDown(KeyCode.Delete) || Input.GetKeyDown(KeyCode.Backspace) || deleteAll)
			{
				if (activeBalls.Count > 0)
					Cheat_RemoveAllBalls();
				else
					deleteAll = false;
			}

			// Infinite balls
			if (infiniteBalls && level.ballsRemaining < 999)
				level.ballsRemaining = 999;

			// Update balls remaining
			if (level.ballsRemaining != ballsRemaining)
				level.ballsRemaining = ballsRemaining;

			// Auto throw
			if ((autoThrow || Input.GetKey(KeyCode.Keypad0)) && ballInHand)
			{
				GameObject ballHolder = GameObject.Find("Balls");
				foreach (BallController ball in ballHolder.GetComponentsInChildren<BallController>())
				{
					if (!ball.hasLaunched)
					{
						int rng = Random.Range(0, 7);
						float xPos = ball.transform.position.x;
						float yPos = ball.transform.position.y;

						switch (rng)
						{
							case 0:
								xPos -= 1.75f;
								break;
							case 1:
								xPos -= 1.75f;
								yPos -= 2f;
								break;
							case 2:
								xPos -= 1.75f;
								yPos += 2f;
								break;
							case 3:
								xPos += 1.75f;
								break;
							case 4:
								xPos += 1.75f;
								yPos -= 2f;
								break;
							case 5:
								xPos += 1.75f;
								yPos += 2f;
								break;
							case 6:
								yPos -= 2f;
								break;
							case 7:
								yPos += 2f;
								break;
						}

						ball.transform.position = new Vector2(xPos, yPos);
						ball.OnMouseUp();
						autoThrow = false;
						break;
					}
				}
			}
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

	void Cheat_RemoveAllBalls()
	{
		for (int i = 0; i < activeBalls.Count; i++)
		{
			activeBalls[i].GetComponent<BallController>().doDestroy = true;
		}

		string s = activeBalls.Count > 1 ? "s" : "";
		Debug.Log("CHEAT: Removing " + activeBalls.Count + " ball" + s + "...");
		deleteAll = false;
	}
}