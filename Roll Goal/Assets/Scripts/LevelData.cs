using ASG;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
	[Header("Refs")]
	[SerializeField] ObjectPooler objPooler;
	[SerializeField] Transform ballTrans;
	[Header("Data")]
	public int level = 0;
	public int ballsRemaining = 0;
	public bool allowMultiple = false;
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
		startPos = ballTrans.position;
		objPooler.amountToPool = ballsRemaining - 1;
	}
	
	public void GrantBall()
	{
		// Give player a new ball
		GameObject newBall = objPooler.RequestObj();
		newBall.transform.position = startPos;
		newBall.SetActive(true);
	}
}