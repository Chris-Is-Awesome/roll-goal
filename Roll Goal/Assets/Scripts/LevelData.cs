using ASG;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
	[Header("Refs")]
	[SerializeField] GameObject ballPrefab;
	[SerializeField] Transform ballParent = null;
	[Header("Data")]
	public int level = 0;
	public int ballsRemaining = 0;
	public bool allowMultiple = false;
	[SerializeField] [CustomAttributes.ReadOnly] int ballsHad = 0;
	public Dictionary<Color, int> currKeys = new Dictionary<Color, int>()
	{
		{Color.blue, 0 },
		{Color.green, 0 },
		{Color.red, 0 },
		{Color.yellow, 0 }

	};
	Vector3 startPos;

	void Start()
	{
		ballsHad++;
		Transform ogBall = ballParent.Find("ball");
		ogBall.name += "_1";
		startPos = ogBall.position;
	}
	
	public void GrantBall()
	{
		// Give player a new ball
		GameObject newBall = Instantiate(ballPrefab, ballParent);
		newBall.transform.position = startPos;
		ballsHad++;
		newBall.name = ballPrefab.name + "_" + ballsHad;
	}
}