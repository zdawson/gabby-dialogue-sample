using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script to let an NPC wander in a rectangular zone
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class NPCWander : MonoBehaviour
{
    private const float ARRIVE_THRESHOLD = 0.05f;
    private const float ARRIVE_THRESHOLD_SQ = ARRIVE_THRESHOLD * ARRIVE_THRESHOLD;

    [SerializeField]
    private SpriteRenderer wanderRegion = null;
    [SerializeField]
    private float waitTime = 4.0f;
    [SerializeField]
    private float waitTimeVariance = 2.0f;
    [SerializeField]
    private float walkSpeed = 2.0f;

    private float timeUntilWander;
    private bool isWalking = false;
    private Vector2 targetWanderPosition;

    private GameObject player = null;

    private void Awake()
    {
        timeUntilWander = waitTime + Random.Range(-waitTimeVariance, waitTimeVariance);
    }

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Stop walking when bumping into things
        ResetWander();
    }

    private void Update()
    {
        if (isWalking)
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            Vector2 position = rigidbody.position + rigidbody.centerOfMass;

            // Walk vertically then horizontally to reach target
            Vector2 toTarget = targetWanderPosition - position;
            if (Mathf.Abs(toTarget.y) > ARRIVE_THRESHOLD)
            {
                Vector2 displacement = new Vector2(0.0f, toTarget.y).normalized * walkSpeed * Time.deltaTime;
                rigidbody.position = rigidbody.position + displacement;

                FaceDirection(displacement.normalized);
            }
            else if (Mathf.Abs(toTarget.x) > ARRIVE_THRESHOLD)
            {
                Vector2 displacement = new Vector2(toTarget.x, 0.0f).normalized * walkSpeed * Time.deltaTime;
                rigidbody.position = rigidbody.position + displacement;

                FaceDirection(displacement.normalized);
            }
            else
            {
                rigidbody.position = targetWanderPosition; // Snap to exact wander position
                ResetWander();
            }
        }
        else
        {
            // Face the player when in range
            
            const float distanceLimit = 2.0f;
            Vector2 toPlayer = (player.transform.position - transform.position);
            if (Mathf.Max(Mathf.Abs(toPlayer.x), Mathf.Abs(toPlayer.y)) <= distanceLimit) // Use a square radius
            {
                FaceDirection(toPlayer.normalized);
            }

            if (wanderRegion != null)
            {
                timeUntilWander -= Time.deltaTime;
                if (timeUntilWander <= 0)
                {
                    targetWanderPosition = GetRandomPosition();
                    isWalking = true;
                }
            }
        }
    }

    private void ResetWander()
    {
        isWalking = false;
        timeUntilWander = waitTime + Random.Range(-waitTimeVariance, waitTimeVariance);

        Animator animator = GetComponent<Animator>();
        animator?.SetBool("GoToIdle", true);
    }

    private Vector2 GetRandomPosition()
    {
        if (!wanderRegion)
        {
            return Vector2.zero;
        }

        Bounds regionBounds = wanderRegion.bounds;

        float r = Random.value;
        if (r > 0.3f)
        {
            // Wander on both axes
            return new Vector2(Random.Range(regionBounds.min.x, regionBounds.max.x), Random.Range(regionBounds.min.y, regionBounds.max.y));
        }
        else if (r > 0.3f)
        {
            // Wander on Y
            return new Vector2(transform.localPosition.x, Random.Range(regionBounds.min.y, regionBounds.max.y));
        }
        else
        {
            // Wander on X
            return new Vector2(Random.Range(regionBounds.min.x, regionBounds.max.x), transform.localPosition.y);
        }
    }

    private void FaceDirection(Vector2 direction)
    {
        // Snap to a cardinal direction
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }
        direction = direction.normalized;

        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator?.SetBool("GoToIdle", false);

            string animationDirection = "";
            if (direction.y > 0.05f)
            {
                animationDirection = "Up";
            }
            else if (direction.x > 0.05f)
            {
                animationDirection = "Right";
            }
            else if (direction.x < -0.05f)
            {
                animationDirection = "Left";
            }
            else if (direction.y < -0.05f)
            {
                animationDirection = "Down";
            }

            string animation = isWalking ? "Walk" : "Idle";
            animator.Play($"{animation}{animationDirection}");
        }
    }

}
