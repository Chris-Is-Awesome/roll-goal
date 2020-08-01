using ASG;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelData : MonoBehaviour
{
	[Header("Refs")]
	[SerializeField] GameObject ballPrefab;
	[SerializeField] Transform ballParent;
	[Header("Data")]
	public int level;
	public int ballsRemaining;
	public bool allowMultiple;
	[SerializeField] bool isDarkLevel;
	[SerializeField] [ConditionalField("isDarkLevel", false, true)] float globalLightIntesity;
	[SerializeField] [ReadOnlyAttributes.ReadOnly] int ballsHad = 0;
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

		// Lighting
		SetLevelLight(GetComponent<Light2D>());
		SetBallLight(ogBall.gameObject);
	}
	
	public void GrantBall()
	{
		// Give player a new ball
		GameObject newBall = Instantiate(ballPrefab, ballParent);
		newBall.transform.position = startPos;
		ballsHad++;
		newBall.name = ballPrefab.name + "_" + ballsHad;

		// Lighting
		SetBallLight(newBall);
	}

	void SetLevelLight(Light2D levelLight)
	{
		// Set light for level
		levelLight.intensity = globalLightIntesity;
	}

	void SetBallLight(GameObject ball)
	{
		// Set ball light if dark level
		Light2D ballLight = ball.GetComponent<Light2D>();
		ballLight.enabled = isDarkLevel;
	}
}