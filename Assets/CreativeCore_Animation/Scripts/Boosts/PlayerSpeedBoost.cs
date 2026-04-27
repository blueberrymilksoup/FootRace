using UnityEngine;
using StarterAssets;
using TMPro;

public class PlayerSpeedBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    public float boostedSpeed = 8f;
    public float boostDuration = 5f;
    public KeyCode activateKey = KeyCode.Q;
    public AudioSource boostSound;

    [Header("UI")]
    public TextMeshProUGUI boostText; // assign in Inspector

    private ThirdPersonController controller;
    private float normalSpeed;
    private float normalSprintSpeed;
    private bool hasBoost = false;
    private bool isBoosting = false;

    void Start()
    {
        controller = GetComponent<ThirdPersonController>();
        normalSpeed = controller.MoveSpeed;
        normalSprintSpeed = controller.SprintSpeed;
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(activateKey) && hasBoost && !isBoosting)
        {
            StartCoroutine(ActivateBoost());
            boostSound.Play();
        }
    }

    public void ChargeBoost()
    {
        hasBoost = true;
        UpdateUI();
    }

    System.Collections.IEnumerator ActivateBoost()
    {
        isBoosting = true;
        hasBoost = false;
        UpdateUI();

        // Apply boost
        controller.MoveSpeed = boostedSpeed;
        controller.SprintSpeed = boostedSpeed + 3f;
        Debug.Log("Boost activated!");

        yield return new WaitForSeconds(boostDuration);

        // Remove boost
        controller.MoveSpeed = normalSpeed;
        controller.SprintSpeed = normalSprintSpeed;
        isBoosting = false;
        Debug.Log("Boost ended!");
        UpdateUI();
    }

    void UpdateUI()
    {
        if (boostText != null)
        {
            if (isBoosting)
                boostText.text = "BOOST ACTIVE!";
            else if (hasBoost)
                boostText.text = "Boost Ready! (Q)";
            else
                boostText.text = "No Boost";
        }
    }
}