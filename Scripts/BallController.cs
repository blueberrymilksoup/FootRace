using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject follow = null;
    
    // Update is called once per frame
    void Update()
    {
        if (follow != null)
        {
            Vector3 offset = new Vector3(0, 0, 1);
            transform.position = follow.transform.position + offset;
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")) // if the player or enemy touch the ball
        {
            follow = collision.gameObject; // it follows that object
        }
        else if(follow != null && collision.gameObject.CompareTag("Goal"))
        {
            Destroy(gameObject); // destroys ball when the ball is following something and touches a goal
        }
    }
}
