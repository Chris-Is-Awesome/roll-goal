using UnityEngine;

public class BallCollision : MonoBehaviour
{
	[Header("Refs")]
	[Header("Data")]
	public float frictionStartValue = 0f;
	public float bounceStartValue = 0f;

	void OnTriggerEnter2D(Collider2D other)
	{
		// If collision with flag, mark level as complete
		if (other.gameObject.CompareTag("Flag"))
		{
			// TODO: Level complete!

			Debug.Log("Completed level!");
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		// Update bounce count
		if (transform.localScale == Vector3.one)
			Debugger.Instance.bounces++;
	}
}