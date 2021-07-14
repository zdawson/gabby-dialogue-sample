using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple script to let an NPC wander in a rectangular zone
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class NPCWander : MonoBehaviour
{
    private const float STOP_THRESHOLD = 0.05f * 0.05f;

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
    private Vector2 targetPos;

    private void Awake()
    {
        timeUntilWander = waitTime + Random.Range(-waitTimeVariance, waitTimeVariance);
    }

    private void Update()
    {
        if (isWalking)
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

            Vector2 nextPos = targetPos;
            rigidbody.position = nextPos;

            if ((targetPos - nextPos).sqrMagnitude <= STOP_THRESHOLD)
            {
                rigidbody.position = targetPos;
                isWalking = false;
                timeUntilWander = waitTime + Random.Range(-waitTimeVariance, waitTimeVariance);
            }
            else
            {
                rigidbody.position = nextPos;
            }
        }
        else
        {
            timeUntilWander -= Time.deltaTime;
            if (timeUntilWander <= 0)
            {
                targetPos = GetRandomPosition();
                isWalking = true;
            }
        }
    }

    private Vector2 GetRandomPosition()
    {
        if (!wanderRegion)
        {
            return Vector2.zero;
        }

        Bounds regionBounds = wanderRegion.bounds;
        if (Random.value > 0.5f)
        {
            return new Vector2(transform.localPosition.x, Random.Range(regionBounds.min.y, regionBounds.max.y));
        }
        else
        {
            return new Vector2(Random.Range(regionBounds.min.x, regionBounds.max.x), transform.localPosition.y);
        }
    }

}
