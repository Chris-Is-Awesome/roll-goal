using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASG;

public class GameStats : MonoBehaviour
{
	[Header("Refs")]
	private BallController currBall = null;
	[Header("Stats")]
	// Balls
	[CustomAttributes.ReadOnly] public int ballsUsedThisRound = 0;
	[CustomAttributes.ReadOnly] public int ballsUsedOverall = 0;
	[CustomAttributes.ReadOnly] public int ballBouncesThisRound = 0;
	[CustomAttributes.ReadOnly] public int ballBouncesOverall = 0;
	[CustomAttributes.ReadOnly] public int ballBouncesWithBallsThisRound = 0;
	[CustomAttributes.ReadOnly] public int ballBouncesWithBallsOverall = 0;
	// Speed
	[CustomAttributes.ReadOnly] public string currentBallSpeed = "0.0 ft/s";
	[CustomAttributes.ReadOnly] public string highestSpeedThisRound = "0.0 ft/s";
	[CustomAttributes.ReadOnly] public string highestSpeedOverall = "0.0 ft/s";
	// Distance
	[CustomAttributes.ReadOnly] public string currentBallDistance = "0.0 feet (0.0 miles)";
	[CustomAttributes.ReadOnly] public string distanceThisRound = "0.0 feet (0.0 miles)";
	[CustomAttributes.ReadOnly] public string highestDistanceThisRound = "0.0 feet (0.0 miles)";
	[CustomAttributes.ReadOnly] public string distanceOverall = "0.0 feet (0.0 miles)";
	[CustomAttributes.ReadOnly] public string highestDistanceOverall = "0.0 feet (0.0 miles)";
	// Time
	[CustomAttributes.ReadOnly] public string currentTime = "00:00.000";
	[CustomAttributes.ReadOnly] public string fastestTimeForLevel = "00:00.000";
	[CustomAttributes.ReadOnly] public string totalTimeSpentOnLevel = "0y, 0mo, 0w, 0d, 0h, 0mi, 0s";
	[CustomAttributes.ReadOnly] public string totalTimePlayingGame = "0y, 0mo, 0w, 0d, 0h, 0mi, 0s";
	[Header("Data")]
	private Vector3 lastBallPosition;
	private float distanceInFeet = 0f;
	private float distanceInMiles = 0f;

	void OnEnable()
	{
		GameEvents.onBallThrow += Event_OnBallThrow;
	}

	void Update()
	{
		if (currBall != null)
		{
			Stats_CalculateDistance();
		}
	}

	void Stats_CalculateDistance()
	{
		Rigidbody2D ballRb = currBall.GetComponent<Rigidbody2D>();

		// Measure distance
		if (ballRb.velocity.x != 0)
		{
			float distance = Vector3.Distance(transform.position, lastBallPosition);
			distanceInFeet = distanceInFeet + distance;
			distanceInMiles = distanceInFeet / 5280f;
			currentBallDistance = distanceInFeet.ToString("0.00") + " feet (" + distanceInMiles.ToString("0.00") + " miles)";

			lastBallPosition = currBall.transform.position;
		}

		//distanceInFeet = distanceInFeet++ * 3.28084f;
		//distanceInMiles = distanceInFeet / 5280f;
		//currentDistance = distanceInFeet.ToString("0.00") + " feet (" + distanceInMiles.ToString("0.00") + " miles)";
	}

	void Event_OnBallThrow(BallController ball)
	{
		lastBallPosition = ball.transform.position;

		// MUST BE AT END!
		currBall = ball;
	}

	void Event_OnBallDestroy(BallController ball)
	{
		//
	}
}