using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GoalManager : MonoBehaviour
{
    [Header("Score")]
    public int playerScore = 0;
    public int enemyScore = 0;
    public TextMeshProUGUI scoreText;


    [Header("Audios")]
    public AudioSource enemyAudio;
    public AudioSource playerAudio;
    public AudioSource gameStart;
    public AudioSource winAudio;
    public AudioSource loseAudio;
    public AudioSource clickSound;

    [Header("VFX")]
    public GameObject playerGoalVFXPrefab;
    public GameObject enemyGoalVFXPrefab;

    [Header("End Game")]
    public TextMeshProUGUI endText;
    public Color green;
    public Color red;
    public GameObject endPanel;

    [Header("Reset Positions")]
    public Transform ball;
    public Transform player;
    public Transform enemy;

    public Vector3 ballStartPos;
    public Vector3 playerStartPos;
    public Vector3 enemyStartPos;

    private bool isResetting = false;

    void Start()
    {
        endPanel.SetActive(false);
        ballStartPos = ball.position;
        playerStartPos = player.position;
        enemyStartPos = enemy.position;
        Debug.Log("Saved positions - Player: " + playerStartPos + " Enemy: " + enemyStartPos);
        UpdateScoreUI();
    }

    public void GoalScored(string scoringTeam)
    {
        if (isResetting) return;
        isResetting = true;

        GameObject vfxToSpawn = null;

        if (scoringTeam == "Player")
        {
            playerScore++;
            vfxToSpawn = playerGoalVFXPrefab;
            if (playerScore < 3) playerAudio.Play(); 
            else if (playerScore == 3) ShowEndPanel("Win"); // when either score = 3 then the game is over -> plays win/lose audio
        }
        else if (scoringTeam == "Enemy")
        {
            enemyScore++;
            vfxToSpawn = enemyGoalVFXPrefab;
            if (enemyScore < 3) enemyAudio.Play();
            else if (enemyScore == 3) ShowEndPanel("Lose");
        }

        UpdateScoreUI();

        if (vfxToSpawn != null)
        {
            GameObject vfx = Instantiate(vfxToSpawn, ball.position, Quaternion.identity);
            Destroy(vfx, 3f);
        }

        StartCoroutine(ResetAfterDelay(2f));
    }

    IEnumerator ResetAfterDelay(float delay)
    {
        // Freeze ball immediately
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.isKinematic = true;

        yield return new WaitForSeconds(delay);

        // Reset ball
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        ball.position = ballStartPos;

        // Disable controllers
        CharacterController playerCC = player.GetComponentInChildren<CharacterController>();
        NavMeshAgent enemyAgent = enemy.GetComponentInChildren<NavMeshAgent>();

        if (playerCC != null) playerCC.enabled = false;
        if (enemyAgent != null) enemyAgent.enabled = false;

        yield return null; // Wait one frame

        if (player != null) player.position = playerStartPos;
        if (enemy != null) enemy.position = enemyStartPos;

        yield return null; // Wait one more frame

        if (playerCC != null) playerCC.enabled = true;
        if (enemyAgent != null) enemyAgent.enabled = true;

        isResetting = false;
        gameStart.Play();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Player: {playerScore}  Enemy: {enemyScore}";
    }

    // end game text change and show ui
    void ShowEndPanel(string condition)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (condition == "Win") 
        {
            endText.color = green;
            winAudio.Play();
        }
        else 
        {
            endText.color = red;
            loseAudio.Play();
        }
        
        endText.text = $"You {condition}!!";
        endPanel.SetActive(true);
        Time.timeScale = 0; // using this to stop the game but idk if it's messing with the button/mouse functionality
    }

    // end game panel buttons
    public void OnMainMenuClicked()
    {
        clickSound.Play();
        SceneManager.LoadScene("MainMenu");
    }
    public void OnExitClicked()
    {
        clickSound.Play();
        Application.Quit();
        Debug.Log("Game Exited");
    }
}