using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ASG
{
	public static class Utility
	{
		#region Scene Stuff

		// Loads the scene via index
		public static void LoadScene(int index, bool async = false, bool additive = false)
		{
			LoadSceneMode loadMode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;

			if (async)
			{
				SceneManager.LoadSceneAsync(index, loadMode);
				return;
			}

			SceneManager.LoadScene(index, loadMode);
		}

		// Loads the scene via name
		public static void LoadScene(string name, bool async = false, bool additive = false)
		{
			LoadSceneMode loadMode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;

			if (async)
			{
				SceneManager.LoadSceneAsync(name, loadMode);
				return;
			}

			SceneManager.LoadScene(name, loadMode);
		}

		// Returns the name of the currently loaded scene
		public static string GetCurrentSceneName()
		{
			return SceneManager.GetActiveScene().name;
		}

		#endregion

		#region Object stuff

		// Returns the specified child from the specified parent (recursive search)
		public static GameObject FindNestedChild()
		{
			return null;
		}

		// Return closest object to another object from a list of objects
		public static GameObject FindClosestObject(GameObject closestToThis, List<GameObject> objectsToCompare)
		{
			GameObject closestObj = null;
			Vector3 pos2 = closestToThis.transform.position;
			float closestDistance = Mathf.Infinity;

			// For each object in list of gos, compare distances
			foreach (GameObject go in objectsToCompare)
			{
				// Get distance
				Vector3 pos1 = go.transform.position;
				float distance = Vector3.Distance(pos1, pos2);

				// If closestToThis is included in list of gos, skip it since obviously it'll be closest since it's on top of itself!
				if (distance <= 0) { continue; }
				else if (distance < closestDistance)
				{
					closestObj = go;
					closestDistance = distance;
				}
			}

			return closestObj;
		}

		//
		public static GameObject FindClosestObjectWithType<T>(GameObject closestToThis) where T : Component
		{
			GameObject closestObj = null;
			Vector3 pos2 = closestToThis.transform.position;
			float closestDistance = Mathf.Infinity;

			// For each object in list of gos, compare distances
			foreach (T go in GameObject.FindObjectsOfType<T>())
			{
				// Get distance
				Vector3 pos1 = go.transform.position;
				float distance = Vector3.Distance(pos1, pos2);

				// If closestToThis is included in list of gos, skip it since obviously it'll be closest since it's on top of itself!
				if (distance <= 0) { continue; }
				else if (distance < closestDistance)
				{
					closestObj = go.gameObject;
					closestDistance = distance;
				}
			}

			return closestObj;
		}

		#endregion
	}
}