using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform ball;
    public Transform targetGoal;
    public NavMeshAgent agent;
    public AudioSource kickSound;

    [Header("Settings")]
    public float kickForce = 8f;
    public float kickRange = 1.5f;
    public float chaseRange = 20f;
    public float kickCooldown = 1f;

    private Rigidbody ballRb;
    private float lastKickTime;
    private Animator animator;

    void Start()
    {
        ballRb = ball.GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 0f;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (ball == null || targetGoal == null) return;
        if (ballRb == null) return;
        if (!agent.enabled || !agent.isOnNavMesh) return;

        float distToBall = Vector3.Distance(transform.position, ball.position);

        if (distToBall <= kickRange)
        {
            if (Time.time - lastKickTime >= kickCooldown)
                KickBall();
        }
        else
        {
            ChaseBall();
        }

        // Update all animator parameters
        if (animator != null)
        {
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);
            animator.SetFloat("MotionSpeed", 1f);
            animator.SetBool("Grounded", true);
            animator.SetBool("FreeFall", false);
            animator.SetBool("Jump", false);
        }
    }

    void ChaseBall()
    {
        Vector3 dirBallToGoal = (targetGoal.position - ball.position).normalized;
        Vector3 approachPos = ball.position - dirBallToGoal * 0.8f;
        agent.isStopped = false;
        agent.SetDestination(approachPos);
    }

    void KickBall()
    {
        if (ballRb.isKinematic) return;

        lastKickTime = Time.time;
        agent.isStopped = true;

        Vector3 kickDir = (targetGoal.position - ball.position).normalized;
        kickDir += new Vector3(Random.Range(-0.2f, 0.2f), 0.1f, Random.Range(-0.1f, 0.1f));
        kickDir.Normalize();

        ballRb.AddForce(kickDir * kickForce, ForceMode.Impulse);
        kickSound.Play();
        Invoke("ResumeChase", kickCooldown);
    }

    void ResumeChase()
    {
        if (agent.enabled)
            agent.isStopped = false;
    }
}