using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerKick : MonoBehaviour
{
    [Header("Kick Settings")]
    public float kickForce = 15f;
    public float kickRange = 2f;
    public float kickCooldown = 0.5f;

    [Header("References")]
    public Transform ball;
    public GameObject kickVFXPrefab; // assign KickVFX prefab here

    private Rigidbody ballRb;
    private float lastKickTime;
    private Animator animator;
    public AudioSource kickSound;

    void Start()
    {
        ballRb = ball.GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time - lastKickTime >= kickCooldown)
        {
            TryKick();
        }
    }

    void TryKick()
    {
        float distToBall = Vector3.Distance(transform.position, ball.position);

        if (distToBall <= kickRange)
        {
            lastKickTime = Time.time;

            // Spawn kick VFX at ball position
            if (kickVFXPrefab != null)
            {
                GameObject vfx = Instantiate(kickVFXPrefab, ball.position, Quaternion.identity);
                Destroy(vfx, 1f); // Clean up after 1 second
            }

            // Kick in the direction the player is facing
            Vector3 kickDir = transform.forward;
            kickDir.y = 0.2f;
            kickDir.Normalize();

            // Reset ball velocity before kick for consistent force
            ballRb.velocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
            ballRb.AddForce(kickDir * kickForce, ForceMode.Impulse);

            if (animator != null)
                animator.SetTrigger("Kick");

            Debug.Log("Kicked!");
            kickSound.Play();
        }
        else
        {
            Debug.Log("Too far to kick - distance: " + distToBall);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, kickRange);
    }
}