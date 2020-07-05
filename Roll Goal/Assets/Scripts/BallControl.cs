using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] GameStats stats;
    [SerializeField] LevelData level;
    [SerializeField] Rigidbody2D anchorRb;
    [SerializeField] GameObject ballSprite;
    [SerializeField] Collider2D ballCollider;
    private Rigidbody2D selfRb;
    private SpringJoint2D springJoint;
    [Header("Data")]
    [SerializeField] bool isPressed = false;
    [SerializeField] bool hasLaunched = false;
    [SerializeField] bool doDestroy = false;
    private float maxPull = 2f;
    private float rotSpeed = 7.5f;
    private float shrinkDecrement = 0.05f;
    private float releaseDelay;
    private float destroyDelay = 5f;
    private bool  hasStartedDeath = false;
    private bool hasBallInHand = true;

	void Awake()
	{
        if (selfRb == null)
            selfRb = GetComponent<Rigidbody2D>();
        if (springJoint == null)
            springJoint = GetComponent<SpringJoint2D>();

        ballCollider.isTrigger = true;

        releaseDelay = 1 / (springJoint.frequency * 4);
        hasBallInHand = true;
	}

	void Update()
	{
		// If is pressed and has not been thrown
        if (isPressed && !hasLaunched)
            PullBall();

        // If launched ball
        if (hasLaunched)
            LaunchBall();

        // If destroying ball, shrink it, then destroy it
        if (doDestroy)
            DestroyBall();
	}

	void OnMouseDown()
	{
        // If haas not launched, pull ball
        if (!hasLaunched)
        {
            isPressed = true;
            selfRb.isKinematic = true;
        }
	}

	void OnMouseUp()
	{
		// If has not launched, launch ball
        if (!hasLaunched)
		{
            isPressed = false;
            selfRb.isKinematic = false;
            hasLaunched = true;

            // TODO: Update stats for # of balls thrown
            StartCoroutine(ReleaseBall());
		}
	}

    void PullBall()
	{
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get mouse pos

        // If pulled beyond maxPull, lock ball to edge of maxPull radius
        if (Vector2.Distance(mousePos, anchorRb.position) > maxPull)
            selfRb.position = anchorRb.position + ((mousePos - anchorRb.position).normalized * maxPull);
        // Else if not pulled beyond maxPull, lock ball to mouse pos
        else
            selfRb.position = mousePos;
	}

    void LaunchBall()
	{
        StartCoroutine(CheckForBallMovement());
        ballSprite.transform.Rotate(new Vector3(0, 0, -selfRb.velocity.x / 5) * rotSpeed);
	}

    void DestroyBall()
	{
        // Shrink ball
        Transform ballTrans = ballSprite.transform;
        Vector2 newScale = new Vector2(ballTrans.localScale.x - shrinkDecrement, ballTrans.localScale.y - shrinkDecrement);
        ballTrans.localScale = newScale;

        // Destroy ball if it's completely shrunken
        if (ballTrans.localScale == Vector3.zero)
		{
            FetchBall();
            Destroy(gameObject);
        }
	}

    void FetchBall()
    {
        // if there are no balls already in hand
        foreach (BallControl ball in GameObject.Find("Balls").GetComponentsInChildren<BallControl>())
        {
            if (!ball.hasLaunched)
            {
                hasBallInHand = true;
                return;
            }
        }

        // If there are balls remaining and no ball is in hand, grant a new ball
        if (!hasBallInHand && level.ballsRemaining > 0)
        {
            level.GrantBall();
        }
        else
        {
            // TODO: Trigger failure screen!
            Debug.Log("No more balls remaining! Level ending...");
        }
    }

	IEnumerator ReleaseBall()
	{
        yield return new WaitForSeconds(releaseDelay);
        springJoint.enabled = false;
        anchorRb.gameObject.SetActive(false);
        hasBallInHand = false;
        ballCollider.isTrigger = false;

		//Update balls remainining count
		if (level.ballsRemaining > 0)
		{
			level.ballsRemaining--;
			// TODO: Update ball count in UI
		}

		// If allow multiple balls, spawn another ball if others remain
		if (level.allowMultiple && level.ballsRemaining > 0)
		{
			level.GrantBall();
			hasBallInHand = true;
		}
	}

    IEnumerator CheckForBallMovement()
    {
        yield return new WaitForSeconds(0.1f);

        // If ball is barely moving or not coming along x axis, start process of destroying it
        if (Mathf.Abs(selfRb.velocity.x) < 2 && !hasStartedDeath)
		{
            StartCoroutine(RemoveBall());
            hasStartedDeath = true;
		}

        // If ball has completely stopped moving, immediately destroy it
        if (Mathf.Abs(selfRb.velocity.x) < 0.25f && selfRb.velocity.y == 0 && !doDestroy)
		{
            StopCoroutine(RemoveBall());
            doDestroy = true;
		}
    }

    IEnumerator RemoveBall()
	{
        yield return new WaitForSeconds(destroyDelay);
        doDestroy = true;
        StopCoroutine(RemoveBall());
    }
}