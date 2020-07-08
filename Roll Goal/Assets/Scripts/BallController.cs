using System.Collections;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] GameStats stats;
    [SerializeField] LevelData level;
    [SerializeField] Debugger debugger;
    [SerializeField] Rigidbody2D anchorRb;
    [SerializeField] Collider2D selfCollider;
    [SerializeField] BallCollision ballCollision;
    private Rigidbody2D selfRb;
    private SpringJoint2D springJoint;
    [Header("Data")]
    [SerializeField] bool isPressed = false;
    [SerializeField] bool hasLaunched = false;
    public bool doDestroy = false;
    private float maxPull = 2f;
    private float rotSpeed = 7.5f;
    private float shrinkDecrement = 0.05f;
    private float releaseDelay;
    private float destroyDelay = 5f;
    private bool  hasStartedDeath = false;

	void Awake()
	{
        GameObject gameMaster = GameObject.Find("GameMaster");

        if (stats == null) stats = gameMaster.GetComponent<GameStats>();
        if (stats == null) stats = gameMaster.AddComponent<GameStats>();
        if (level == null) level = Utility.GetLevelData();
        if (level == null) Debug.LogError("LevelData is null");
        if (debugger == null) debugger = gameMaster.GetComponent<Debugger>();
        if (debugger == null) debugger = gameMaster.AddComponent<Debugger>();
        if (ballCollision == null) ballCollision = GetComponent<BallCollision>();
        if (ballCollision == null) Debug.LogError("ballCollision is null");
        if (anchorRb == null) anchorRb = transform.parent.GetComponent<Rigidbody2D>();
        if (anchorRb == null) Debug.LogError("anchorRb is null");
        if (selfCollider == null) selfCollider = GetComponent<CircleCollider2D>();
        if (selfCollider == null) Debug.LogError("selfCollider is null");
        if (selfRb == null) selfRb = GetComponent<Rigidbody2D>();
        if (selfRb == null) Debug.LogError("selfRb is null");
        if (springJoint == null) springJoint = GetComponent<SpringJoint2D>();
        if (springJoint == null) Debug.LogError("springJoint is null");
	}

	void OnEnable()
	{
        // Set default values
        transform.localScale = Vector3.one;
        selfRb.isKinematic = true;
        selfRb.sharedMaterial = new PhysicsMaterial2D()
        {
            name = "wallBounce",
            friction = ballCollision.frictionStartValue,
            bounciness = ballCollision.bounceStartValue
        };
        selfCollider.isTrigger = true;
        springJoint.enabled = false; springJoint.enabled = true;
        releaseDelay = 1 / (springJoint.frequency * 4);
        isPressed = false;
        hasLaunched = false;
        hasStartedDeath = false;
        doDestroy = false;
        ballCollision.enabled = false; ballCollision.enabled = true;
        debugger.ballInHand = true;
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
        // If has not launched, pull ball
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
        transform.Rotate(new Vector3(0, 0, -selfRb.velocity.x / 5) * rotSpeed);
	}

    void DestroyBall()
	{
        // Shrink ball
        Vector2 newScale = new Vector2(transform.localScale.x - shrinkDecrement, transform.localScale.y - shrinkDecrement);
        transform.localScale = newScale;

        // Destroy ball if it's completely shrunken
        if (transform.localScale == Vector3.zero)
		{
            FetchBall();
            gameObject.SetActive(false);
        }
	}

    void FetchBall()
    {
        // if there are no balls already in hand
        foreach (BallController ball in GameObject.Find("Balls").GetComponentsInChildren<BallController>())
        {
            if (!ball.hasLaunched)
            {
                debugger.ballInHand = true;
                return;
            }
        }

        // If there are balls remaining and no ball is in hand, grant a new ball
        if (!debugger.ballInHand && level.ballsRemaining > 0)
        {
            level.GrantBall();
        }
        // Else if no balls remain and no balls exist, end level
        else if (level.ballsRemaining < 1)
        {
            GameEvents.OnLevelFinish(false, 0);
        }
    }

    IEnumerator ReleaseBall()
    {
        yield return new WaitForSeconds(releaseDelay);
        springJoint.enabled = false;
        debugger.ballInHand = false;
        selfCollider.isTrigger = false;

        // TODO: Update stats for # of balls thrown
        debugger.ballsUsed++;

        //Update balls remainining count
        if (level.ballsRemaining > 0)
		{
			level.ballsRemaining--;
			// TODO: Update ball count in UI
		}

        // Disable anchor if not allowing multiple balls or if allowing multiple balls but no balls remain
        if (!level.allowMultiple || (level.allowMultiple && level.ballsRemaining < 1))
            anchorRb.gameObject.SetActive(false);

        // If allow multiple balls, spawn another ball if others remain
        if (level.allowMultiple && level.ballsRemaining > 0)
		{
			level.GrantBall();
			debugger.ballInHand = true;
		}
	}

    IEnumerator CheckForBallMovement()
    {
        yield return new WaitForSeconds(0.1f);

        // If ball is barely moving or not moving along x axis, start process of destroying it
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