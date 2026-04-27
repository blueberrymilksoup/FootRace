using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    [Header("Settings")]
    public float respawnTime = 15f;

    private MeshRenderer visual;
    private Collider pickupCollider;
    private bool isActive = true;
    public AudioSource boostSound;

    [Header("VFX")]
    public GameObject collectVFXPrefab;
    public GameObject respawnVFXPrefab;

    void Start()
    {
        visual = GetComponentInChildren<MeshRenderer>();
        pickupCollider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        Debug.Log("Trigger entered by: " + other.name);

        // Search on object, parent, and all parents
        PlayerSpeedBoost boost = other.GetComponent<PlayerSpeedBoost>();
        if (boost == null) boost = other.GetComponentInParent<PlayerSpeedBoost>();
        if (boost == null) boost = FindObjectOfType<PlayerSpeedBoost>();

        if (boost != null)
        {
            boost.ChargeBoost();

            // Spawn collect VFX at pickup position
            if (collectVFXPrefab != null)
            {
                GameObject vfx = Instantiate(collectVFXPrefab, transform.position, Quaternion.identity);
                Destroy(vfx, 3f);
            }

            Debug.Log("Boost picked up!");
            boostSound.Play();
            StartCoroutine(Deactivate());
        }
        else
        {
            Debug.Log("PlayerSpeedBoost not found on: " + other.name);
        }
    }

    System.Collections.IEnumerator Deactivate()
    {
        isActive = false;
        if (visual != null) visual.enabled = false;
        if (pickupCollider != null) pickupCollider.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        isActive = true;
        if (visual != null) visual.enabled = true;
        if (pickupCollider != null) pickupCollider.enabled = true;

        // Spawn respawn VFX at pickup position
        if (respawnVFXPrefab != null)
        {
            GameObject vfx = Instantiate(respawnVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 3f);
        }

        Debug.Log("Boost pad respawned!");
    }

    void Update()
    {
        if (isActive)
        {
            float pulse = Mathf.Sin(Time.time * 3f) * 0.1f;
            transform.localScale = new Vector3(
                transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z
            );
        }
    }
}