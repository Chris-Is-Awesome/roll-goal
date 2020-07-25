using UnityEngine;

public class BallCollision : MonoBehaviour
{
	[Header("Refs")]
	[Header("Data")]
	public float bounceTolerance;
	public float frictionStartValue;
	public float bounceStartValue;
	[SerializeField] Color32 hazardDeathColor;

	void OnEnable()
	{
		Physics2D.velocityThreshold = bounceTolerance;
	}

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

			Rigidbody2D selfRb = GetComponent<Rigidbody2D>();
			Rigidbody2D otherRb = other.gameObject.GetComponent<Rigidbody2D>();

			if ((selfRb.velocity.y > 1f && otherRb.velocity.y > 1f) || (selfRb.velocity.y < -1f && otherRb.velocity.y < -1f))
			{
				GameStats.Instance.ballBouncesWithBallsThisRound += 0.5f;
				GameStats.Instance.ballBouncesWithBallsOverall = PlayerPrefs.GetFloat("stat_bouncesWithBallsOverall") + 0.5f;
				PlayerPrefs.SetFloat("stat_bouncesWithBallsOverall", GameStats.Instance.ballBouncesWithBallsOverall);
			}
		}

		// If collision with hazard, kill ball
		if (other.collider.CompareTag("Hazard"))
		{
			GetComponent<BallController>().doDestroy = true;
			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
			GetComponent<SpriteRenderer>().color = hazardDeathColor;
		}

		// If collision with button, invoke event
		if (other.collider.CompareTag("Button"))
		{
			other.gameObject.GetComponent<LevelEventTrigger>().DoConnectedAction();
			Debug.Log("Button pressed!");
		}
	}

	void OnCollisionExit2D(Collision2D other)
	{
		// Update bounce count
		if (transform.localScale == Vector3.one)
			GameEvents.OnBallBounced(this);
	}
}