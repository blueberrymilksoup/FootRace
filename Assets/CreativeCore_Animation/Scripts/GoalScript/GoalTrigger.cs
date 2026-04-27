using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public GoalManager goalManager;
    public string scoringTeam;
    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered trigger: " + other.name + " tag: " + other.tag);

        if (triggered) return;

        if (other.CompareTag("Ball"))
        {
            Debug.Log("GOAL triggered by: " + scoringTeam);
            triggered = true;
            goalManager.GoalScored(scoringTeam);
            Invoke("ResetTrigger", 3f);
        }
    }

    void ResetTrigger()
    {
        triggered = false;
    }
}