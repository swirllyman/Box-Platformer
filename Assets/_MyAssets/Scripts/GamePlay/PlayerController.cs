using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpAmount;
    public bool doubleJumpAcquired = false;
    public bool onLeftWall = false;
    public bool onRightWall = false;
    [Header("FX")]
    public ParticleSystem leftWallSlideEffect;
    public ParticleSystem rightWallSlideEffect;
    public ParticleSystem groundMovementParticles;
    public ParticleSystem[] doubleJumpBoost;
    public AudioSource running;
    public AudioSource sliding;
    public AudioSource jumping;
    public AudioClip jump;
    public AudioClip boostJump;
    AudioSource myAudio;

    Vector3 myFakeVelocity;
    Vector3 prevPos;
    Ray rayRight;
    Ray rayLeft;
    Ray rayDown;
    RaycastHit hit;
    Rigidbody body;
    //CapsuleCollider col;

    float movementCD = .5f;
    float jumpDirection = 0;

    bool justWallJumped = false;
    bool switchedDirectionInAir = false;
    bool secondJumpAvailable = false;
    bool onGround = false;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        myAudio = GetComponent<AudioSource>();
        //col = GetComponent<CapsuleCollider>();
    }

	
	// Update is called once per frame
	void Update () {

        CheckArea();
        CheckInput();
        CheckFX();

        myFakeVelocity = (transform.position - prevPos) / Time.deltaTime;
        prevPos = transform.position;

        if (justWallJumped)
        {
            if(movementCD > 0)
            {
                movementCD -= Time.deltaTime;
            }
        }
	}

    void CheckFX()
    {
        if (onGround)
        {
            if ((myFakeVelocity.x > 5 || myFakeVelocity.x < -5) &!groundMovementParticles.isPlaying)
            {
                groundMovementParticles.Play();
                running.Play();
            }
            else if ((myFakeVelocity.x < 5 && myFakeVelocity.x > -5) && groundMovementParticles.isPlaying)
            {
                groundMovementParticles.Stop();
                running.Stop();
            }
        }
        if (!onGround)
        {
            if(groundMovementParticles.isPlaying)
            {
                groundMovementParticles.Stop();
                running.Stop();
            }
            if (onLeftWall & !leftWallSlideEffect.isPlaying)
            {
                leftWallSlideEffect.Play();
                sliding.Play();
            }
            else if (!onLeftWall && leftWallSlideEffect.isPlaying)
            {
                leftWallSlideEffect.Stop();
                sliding.Stop();
            }

            if (onRightWall & !rightWallSlideEffect.isPlaying)
            {
                rightWallSlideEffect.Play();
                sliding.Play();
            }
            else if (!onRightWall && rightWallSlideEffect.isPlaying)
            {
                rightWallSlideEffect.Stop();
                sliding.Stop();
            }
        }
        else if (rightWallSlideEffect.isPlaying)
        {
            rightWallSlideEffect.Stop();
            sliding.Stop();
        }
        else if(leftWallSlideEffect.isPlaying)
        {
            leftWallSlideEffect.Stop();
            sliding.Stop();
        }
    }

    void CheckArea()
    {
        onGround = false;
        onRightWall = false;
        onLeftWall = false;

        rayRight = new Ray(transform.position, Vector3.right);
        rayLeft = new Ray(transform.position, -Vector3.right);
        rayDown = new Ray(transform.position, -Vector3.up);

        if(Physics.Raycast(rayRight, out hit, .6f))
        {
            if(hit.collider.tag == "Wall")
            {
                onRightWall = true;
                ResetJump();
            }
        }

        if (Physics.Raycast(rayLeft, out hit, .6f))
        {
            if (hit.collider.tag == "Wall")
            {
                onLeftWall = true;
                ResetJump();
            }
        }

        if (Physics.Raycast(rayDown, out hit, 1.2f))
        {
            if (hit.collider.tag == "Ground")
            {
                onGround = true;
                ResetJump();
            }
        }
    }

    void ResetJump()
    {
        switchedDirectionInAir = false;
        secondJumpAvailable = true;
    }

    void CheckInput()
    {
        float direction = Input.GetAxis("Horizontal");
        CheckInputDirection(direction);

        if (Input.GetButtonDown("Jump"))
        {
            if (onGround)
            {
                Jump(direction);
            }
            else if (onLeftWall || onRightWall)
            {
                JumpOffWall();
            }
            else if (doubleJumpAcquired && secondJumpAvailable)
            {
                SecondJump(direction);
            }
        }
    }

    void CheckInputDirection(float direction)
    {

        //Wall Jump movement CD
        if (justWallJumped && movementCD > 0.0f)
        {
            direction = 0;
        }
        else if (justWallJumped && direction != 0)
        {
            justWallJumped = false;
            body.velocity = Vector3.zero;
        }

        //Inhibit movement when against a wall
        if (direction > 0 && onRightWall)
            direction = 0;
        else if (direction < 0 && onLeftWall)
            direction = 0;

        //Change speed depending on air status
        if (!onGround &! switchedDirectionInAir)
        {
            float currentDirection = direction;
            if (direction != 0)
                currentDirection = direction > 0 ? 1 : -1;
            else
                currentDirection = 0;

            //Check if player reversed direction while in air
            if (currentDirection != 0 && currentDirection != jumpDirection)
                switchedDirectionInAir = true;
        }

        float currentSpeed = switchedDirectionInAir ? (speed / 2) : speed;

        transform.position += (Vector3.right * direction * currentSpeed) * Time.deltaTime;
    }


    void JumpOffWall()
    {
        jumping.clip = jump;
        jumping.Play();
        justWallJumped = true;
        movementCD = .5f;
        body.velocity = Vector3.zero;
        Vector3 jumpVector = Vector3.up;
        if (onRightWall)
        {
            jumpVector += -Vector3.right;
            jumpDirection = -1;
        }
        else
        {
            jumpVector += Vector3.right;
            jumpDirection = 1;
        }
        
        body.AddForce(jumpVector * jumpAmount);

    }

    void Jump(float direction)
    {
        jumping.clip = jump;
        jumping.Play();
        body.velocity = Vector3.zero;
        body.AddForce(Vector3.up * jumpAmount);

        if (direction != 0)
            jumpDirection = direction > 0 ? 1 : -1;
        else
            jumpDirection = 0;
    }

    void SecondJump(float direction)
    {
        jumping.clip = boostJump;
        jumping.Play();
        foreach (ParticleSystem p in doubleJumpBoost)
        {
            p.Play();
        }
        secondJumpAvailable = false;
        body.velocity = Vector3.zero;
        body.AddForce(Vector3.up * jumpAmount);

        if (direction != 0)
            jumpDirection = direction > 0 ? 1 : -1;
        else
            jumpDirection = 0;
    }
}
