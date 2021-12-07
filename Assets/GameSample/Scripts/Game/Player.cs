using System.Collections.Generic;
using UnityEngine;

namespace GabbyDialogueSample
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        private const float STOP_THRESHOLD = 0.05f * 0.05f;

        [SerializeField]
        private float walkSpeed = 3.0f;
        [SerializeField]
        private float accelerateTime = 0.3f;
        [SerializeField]
        private float drag = 0.3f;

        private float acceleration = 0.5f;
        private Vector2 facing = Vector2.up;

        private bool disableInput = false;

        private void Awake()
        {
            acceleration = walkSpeed / accelerateTime;
        }

        private void OnEnable()
        {
            GameSampleDialogueSystem.Instance().DialogueStarted += OnDialogueStarted;
            GameSampleDialogueSystem.Instance().DialogueEnded += OnDialogueEnded;
        }

        private void OnDisable()
        {
            GameSampleDialogueSystem.Instance().DialogueStarted -= OnDialogueStarted;
            GameSampleDialogueSystem.Instance().DialogueEnded -= OnDialogueEnded;
        }

        private void FixedUpdate()
        {
            Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
            float deltaTime = Time.fixedDeltaTime;
            Vector2 movement = GetMovementVector();
            Vector2 velocity = rigidbody.velocity;

            if (movement.sqrMagnitude > STOP_THRESHOLD)
            {
                // Allow sliding along walls
                ContactFilter2D contactFilter = new ContactFilter2D();
                contactFilter.SetNormalAngle(0, 180);
                List<ContactPoint2D> contacts = new List<ContactPoint2D>();
                rigidbody.GetContacts(contacts);

                Vector2 avgDirection = Vector2.zero;
                float angleCount = 0.0f;
                for (int i = 0; i < contacts.Count; ++i)
                {
                    ContactPoint2D contact = contacts[i];
                    if (contact.separation > float.Epsilon)
                    {
                        continue;
                    }

                    Vector2 normal = -contact.normal;
                    Vector3 tangent3D = Vector3.Cross(new Vector3(normal.x, normal.y, 0.0f), Vector3.forward);
                    Vector2 tangent = new Vector2(tangent3D.x, tangent3D.y);

                    if (Vector2.Angle(movement, contact.normal) < 90)
                    {
                        continue;
                    }

                    if (Vector2.Angle(movement, tangent) < 46)
                    {
                        avgDirection = avgDirection + tangent;
                        angleCount++;
                    }
                    else if (Vector2.Angle(movement, -tangent) < 46)
                    {
                        avgDirection = avgDirection - tangent;
                        angleCount++;
                    }
                }

                if (angleCount > 0.5f)
                {
                    movement = avgDirection / angleCount;
                }

                // Apply input to velocity
                float speed = velocity.magnitude;
                velocity = movement * (speed + acceleration * deltaTime);
                velocity = Vector2.ClampMagnitude(velocity, walkSpeed);
            }
            else
            {
                velocity *= (1.0f - drag);
                if (velocity.sqrMagnitude <= STOP_THRESHOLD)
                {
                    velocity = Vector2.zero;
                }
            }

            rigidbody.velocity = velocity;
        }

        private void Update()
        {
            if (!disableInput)
            {
                // Interactions
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

                    RaycastHit2D[] hits = Physics2D.RaycastAll(rigidbody.position + rigidbody.centerOfMass, facing, 0.75f);
                    foreach (RaycastHit2D hit in hits)
                    {
                        Interactable interactable = hit.transform.GetComponent<Interactable>();
                        if (interactable != null)
                        {
                            interactable.OnInteract();
                            break;
                        }
                    }
                }
            }

            // Walk animations
            Animator animator = GetComponent<Animator>();
            if (animator)
            {
                Vector2 movement = GetMovementVector();
                if (movement.y > 0.05f)
                {
                    animator.SetBool("GoToIdle", false);
                    animator.Play("WalkUp");
                    facing = Vector2.up;
                }
                else if (movement.x > 0.05f)
                {
                    animator.SetBool("GoToIdle", false);
                    animator.Play("WalkRight");
                    facing = Vector2.right;
                }
                else if (movement.x < -0.05f)
                {
                    animator.SetBool("GoToIdle", false);
                    animator.Play("WalkLeft");
                    facing = Vector2.left;
                }
                else if (movement.y < -0.05f)
                {
                    animator.SetBool("GoToIdle", false);
                    animator.Play("WalkDown");
                    facing = Vector2.down;
                }
                else
                {
                    animator.SetBool("GoToIdle", true);
                }
            }
        }

        private Vector2 GetMovementVector()
        {
            if (disableInput)
            {
                return Vector2.zero;
            }
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        private void OnDialogueStarted()
        {
            disableInput = true;
        }

        private void OnDialogueEnded()
        {
            disableInput = false;
        }
    }
}