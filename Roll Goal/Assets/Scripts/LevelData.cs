using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
	[SerializeField] Transform ballParent;
	[SerializeField] GameObject ballPrefab;
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
	}
	
	public void GrantBall()
	{
		// Give player a new ball
		Instantiate(ballPrefab, startPos, Quaternion.identity).transform.parent = ballParent;
	}
}