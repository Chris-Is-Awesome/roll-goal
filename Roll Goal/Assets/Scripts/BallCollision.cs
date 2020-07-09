using UnityEngine;

public class BallCollision : MonoBehaviour
{
	[Header("Refs")]
	[Header("Data")]
	public float frictionStartValue;
	public float bounceStartValue;

	void OnTriggerEnter2D(Collider2D other)
	{
		// If collision with flag, mark level as complete
		if (other.gameObject.CompareTag("Flag"))
		{
			// TODO: Level complete!

			Debug.Log("Completed level!");
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		// If collision with ball, prevent death for a little bit
		if (other.collider.CompareTag("Ball"))
		{
			BallController selfBall = GetComponent<BallController>();
			BallController otherBall = other.gameObject.GetComponent<BallController>();

			selfBall.invincible = true;
			selfBall.hasStartedDeath = false;
			otherBall.invincible = true;
			otherBall.hasStartedDeath = false;
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		// Update bounce count
		if (transform.localScale == Vector3.one)
			Debugger.Instance.bounces++;
	}
}