using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASG;

public class GameStats : Singleton<GameStats>
{
	[Header("Refs")]
	private BallController currBall = null;
	[Header("Stats")]
	// Balls
	[ReadOnlyAttributes.ReadOnly] public int ballsUsedThisRound;
	[ReadOnlyAttributes.ReadOnly] public int ballsUsedOverall;
	[ReadOnlyAttributes.ReadOnly] public int ballBouncesThisRound;
	[ReadOnlyAttributes.ReadOnly] public int ballBouncesOverall;
	[ReadOnlyAttributes.ReadOnly] public float ballBouncesWithBallsThisRound;
	[ReadOnlyAttributes.ReadOnly] public float ballBouncesWithBallsOverall;
	// Speed
	[ReadOnlyAttributes.ReadOnly] public string currentBallSpeed = "0.0 ft/s";
	[ReadOnlyAttributes.ReadOnly] public string highestSpeedThisRound = "0.0 ft/s";
	[ReadOnlyAttributes.ReadOnly] public string highestSpeedOverall = "0.0 ft/s";
	// Distance
	[ReadOnlyAttributes.ReadOnly] public string currentBallDistance = "0.0 feet (0.0 miles)";
	[ReadOnlyAttributes.ReadOnly] public string distanceThisRound = "0.0 feet (0.0 miles)";
	[ReadOnlyAttributes.ReadOnly] public string highestDistanceThisRound = "0.0 feet (0.0 miles)";
	[ReadOnlyAttributes.ReadOnly] public string distanceOverall = "0.0 feet (0.0 miles)";
	// Time
	[ReadOnlyAttributes.ReadOnly] public string currentTime = "00:00.000";
	[ReadOnlyAttributes.ReadOnly] public string fastestTimeForLevel = "00:00.000";
	[ReadOnlyAttributes.ReadOnly] public string totalTimeSpentOnLevel = "0y, 0mo, 0w, 0d, 0h, 0mi, 0s";
	[ReadOnlyAttributes.ReadOnly] public string totalTimePlayingGame = "0y, 0mo, 0w, 0d, 0h, 0mi, 0s";
	[Header("Data")]
	Vector3 lastBallPosition;
	float distanceInFeetCurrent;
	float distanceInMilesCurrent;
	float distanceInFeetRound;
	float distanceInMilesRound;

	void Awake()
	{
		GameEvents.OnBallThrow += Event_OnBallThrow;
		GameEvents.OnBallDestroy += Event_OnBallDestroy;
		GameEvents.OnBallBounce += Event_OnBallBounce;

		// Load saved stats
		ballsUsedOverall = PlayerPrefs.GetInt("stat_ballsUsedOverall");
		ballBouncesOverall = PlayerPrefs.GetInt("stat_bouncesOverall");
		ballBouncesWithBallsOverall = PlayerPrefs.GetFloat("stat_bouncesWithBallsOverall");
	}

	void FixedUpdate()
	{
		if (currBall != null)
		{
			Stats_CalculateBallStats();
		}
	}

	void Stats_CalculateBallStats()
	{
		Rigidbody2D ballRb = currBall.GetComponent<Rigidbody2D>();

		if (ballRb.velocity.x != 0)
		{
			float distance = Vector2.Distance(transform.localPosition, lastBallPosition);

			// Measure speed
			if (transform.position != lastBallPosition)
			{
				currentBallSpeed = distance.ToString("0.00") + " ft/s";
			}

			// Current ball distance
			distanceInFeetCurrent += distance;
			distanceInMilesCurrent = distanceInFeetCurrent / 5280f;
			currentBallDistance = distanceInFeetCurrent.ToString("0.00") + " feet (" + distanceInMilesCurrent.ToString("0.00") + " miles)";

			// Distance this round
			distanceInFeetRound += distance;
			distanceInMilesRound = distanceInFeetRound / 5280f;
			distanceThisRound = distanceInFeetRound.ToString("0.00") + " feet (" + distanceInMilesRound.ToString("0.00") + " miles)";

			lastBallPosition = currBall.transform.localPosition;
		}
		else
		{
			// Speed is 0
			currentBallSpeed = 0f + " ft/s";
		}
	}

	void Event_OnBallThrow(BallController ball)
	{
		// Reset current ball stats
		distanceInFeetCurrent = 0f;
		distanceInMilesCurrent = 0f;
		currentBallDistance = "0.0 feet (0.0 miles)";

		lastBallPosition = ball.transform.localPosition;

		// Update balls thrown stats
		ballsUsedThisRound++;
		ballsUsedOverall = PlayerPrefs.GetInt("stat_ballsUsedOverall") + 1;
		PlayerPrefs.SetInt("stat_ballsUsedOverall", ballsUsedOverall);

		// MUST BE AT END!
		currBall = ball;
	}

	void Event_OnBallDestroy(BallController ball)
	{
		//
	}

	void Event_OnBallBounce(BallCollision ball)
	{
		ballBouncesThisRound++;
		ballBouncesOverall = PlayerPrefs.GetInt("stat_bouncesOverall") + 1;
		PlayerPrefs.SetInt("stat_bouncesOverall", ballBouncesOverall);
	}
}