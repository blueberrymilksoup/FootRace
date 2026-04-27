using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    private AudioSource audioSource;
    private int num; // decides which camera to switch to
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public AudioSource winAudio;
    public Camera camera1; // camera that follows player 1, select in inspector
    public Camera camera2; // camera that follows player 2, select in inspector
    public float timer = 0; // timer for the speed buff

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent <Rigidbody>();
        camera2.enabled = false; // just to make sure that everything is working correctly
        camera1.enabled = true;
        count = 0;
        num = 0;
        //SetCountText();
        // winTextObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }
    
    private void FixedUpdate()
    {
        Vector3 movement = new Vector3 (movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
        if (Input.GetKeyDown(KeyCode.F)) // when you press f the camera should switch
        {
            num++;
            SwitchCharacter();
        }
        if(timer > 0) // updates the timer when needed aka when player collects a speed buff
        {
            timer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            //count ++;
            timer = 2;
            while(timer > 0)
            {
                speed*=2;
            }
            //SetCountText();
            audioSource.Play();

        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

/*     void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 8)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            Destroy(GameObject.FindGameObjectWithTag("BGM"));
            winAudio.Play();

        }
    } */

    private void SwitchCharacter()
    {
        if (num % 2 == 1)
        {
            camera1.enabled = false;
            camera2.enabled = true;
        }
        else if (num % 2 == 0)
        {
            camera1.enabled = true;
            camera2.enabled = false;
        }
    }

    
}
