using UnityEngine;

public class BallCollision : MonoBehaviour
{
	[Header("Refs")]
	private PhysicsMaterial2D wallMat;
	[SerializeField] CircleCollider2D ballCollider;
	[Header("Data")]
	[SerializeField] float bounceStartValue;
	[SerializeField] [Range(0, 0.1f)] float bounceDecrement;
	[SerializeField] [Range(0, 0.001f)] float frictionIncrement;

	void Awake()
	{
		// Create material
		wallMat = new PhysicsMaterial2D
		{
			name = "wallBounce",
			bounciness = bounceStartValue,
			friction = 0f
		};
		GetComponent<Rigidbody2D>().sharedMaterial = wallMat;

		// Default material
		wallMat.bounciness = bounceStartValue;
		wallMat.friction = 0f;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		// If collision with flag, mark level as complete
		if (other.gameObject.tag == "Flag")
		{
			// TODO: Level complete!

			Debug.Log("Completed level!");
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		// Decrement bounciness and increment friction with every bounce
		wallMat.bounciness -= bounceDecrement;
		wallMat.friction += frictionIncrement;

		// Reset collider to make bouncing change take effect (Workaround for Unity bug)
		if (transform.localScale.x == 1)
		{
			ballCollider.isTrigger = true;
			ballCollider.isTrigger = false;
		}
	}
}