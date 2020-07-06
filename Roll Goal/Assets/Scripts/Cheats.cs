using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
	class KeyFuncs
	{
		public List<KeyCode> keys = new List<KeyCode>();
		public Action function;

		public KeyFuncs(List<KeyCode> _keys, Action _function)
		{
			keys = _keys;
			function = _function;
		}
	}

	// Refs
	[SerializeField] LevelData level;
	// Data
	[SerializeField] List<KeyFuncs> keyFuncs = new List<KeyFuncs>();
	// Vars
	public bool infiniteBalls = false;

	void Awake()
	{
		keyFuncs.Add(new KeyFuncs(new List<KeyCode> { KeyCode.Space }, GrantBall));
	}

	void Update()
	{
		for (int i = 0; i < keyFuncs.Count; i++)
		{
			KeyFuncs keyFunc = keyFuncs[i];

			for (int j = 0; j < keyFunc.keys.Count; j++)
			{
				if (Input.GetKeyDown(keyFunc.keys[i]))
				{
					keyFunc.function();
					break;
				}
			}
		}
	}

	void GrantBall()
	{
		if (level.ballInHand)
			Debug.Log("CHEAT: Not spawning ball since you have ball in hand, you doofus!");
		else
		{
			Debug.Log("CHEAT: Granting new ball!");
			level.GrantBall();
		}
	}
}