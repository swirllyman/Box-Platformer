using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpAmount;

    Rigidbody body;
    Collider col;
    bool jumpAvailable = false;
    bool onWall = false;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

	
	// Update is called once per frame
	void Update () {
        float direction = Input.GetAxis("Horizontal");
        transform.position += (Vector3.right * direction * speed) * Time.deltaTime;

        if (jumpAvailable || onWall)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }

	}

    void Jump()
    {
        jumpAvailable = false;
        body.velocity = Vector3.zero;
        body.AddForce(Vector3.up * jumpAmount);
    }


    void OnCollisionEnter(Collision c)
    {
        if(c.collider.tag == "Ground" || c.collider.tag == "Wall")
        {
            CheckCollisions();
        }
    }

    void OnCollisionExit(Collision c)
    {
        if (c.collider.tag == "Ground" || c.collider.tag == "Wall")
        {
            CheckCollisions();
        }
    }

    void CheckCollisions()
    {
        jumpAvailable = false;
        onWall = false;
        Collider[] allColliders = Physics.OverlapBox(col.bounds.center, col.bounds.extents);
        foreach(Collider c in allColliders)
        {
            if(c.tag == "Ground")
            {
                jumpAvailable = true;
            }
            else if(c.tag == "Wall")
            {
                onWall = true;
            }
        }
    }
}
