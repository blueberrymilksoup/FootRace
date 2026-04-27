using UnityEngine;
using StarterAssets;
using UnityEngine.AI;

public class GameSettings : MonoBehaviour
{
    [Header("References")]
    public ThirdPersonController playerController;
    public NavMeshAgent enemyAgent;
    public AudioSource bgmSource;

    void Start()
    {
        // Apply saved player speed
        if (playerController != null)
        {
            playerController.MoveSpeed = PlayerPrefs.GetFloat("PlayerSpeed", 2f);
            playerController.SprintSpeed = playerController.MoveSpeed + 3.335f;
        }

        // Apply saved enemy speed
        if (enemyAgent != null)
            enemyAgent.speed = PlayerPrefs.GetFloat("EnemySpeed", 3.5f);

        // Apply saved volume
        if (bgmSource != null)
            bgmSource.volume = PlayerPrefs.GetFloat("Volume", 0.75f);
    }
}