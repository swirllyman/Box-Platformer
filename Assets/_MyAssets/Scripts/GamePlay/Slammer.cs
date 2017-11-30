using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slammer : MonoBehaviour {

    enum MovingState { None, Falling, Rising}

    public BreakablePlatform myPlatform;

    public Sprite sleepSprite;
    public Sprite awakeSprite;

    public Transform destinationTransform;
    public float sleepSeconds = 1.0f;
    public float fallSpeed;
    public float riseSpeed;
    public bool autoAttack = false;

    MovingState currentState = MovingState.None;
    bool sleeping = false;
    Vector3 origin;
    SpriteRenderer myRend;
    Transform slammerTransform;
    Collider myCollider;
	// Use this for initialization
	void Start () {
        myCollider = GetComponent<Collider>();
        myRend = GetComponentInChildren<SpriteRenderer>();
        myRend.sprite = sleepSprite;
        slammerTransform = myRend.transform;
        origin = transform.position;
        sleeping = true;

        if(myPlatform != null)
        {
            myPlatform.justDestroyed += MyPlatformBroke;
        }

        if (autoAttack)
            StartCoroutine(AutoAttack());
    }

    void MyPlatformBroke(BreakablePlatform platform)
    {
        RaycastHit hit;
        if(Physics.Raycast(destinationTransform.position, Vector3.down, out hit, 1000.0f))
        {
            destinationTransform.position = new Vector3(destinationTransform.position.x, hit.point.y + .5f, destinationTransform.position.z);
        }
        else
        {
            destinationTransform.position += Vector3.down * 1000000;
        }
        currentState = MovingState.Falling;
    }
	
	void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player" && sleeping)
        {
            StartCoroutine(Slam());
        }
    }

    void Update()
    {
        if(currentState == MovingState.Falling)
        {
            slammerTransform.position += Vector3.down * (Time.deltaTime * fallSpeed);// new Vector3(slammerTransform.position.x, slammerTransform.position.y - Time.deltaTime * fallSpeed, slammerTransform.position.z);
            if(slammerTransform.position.y < destinationTransform.position.y)
            {
                StartCoroutine(HitGround());
            }
        }

        if (currentState == MovingState.Rising)
        {
            slammerTransform.position += Vector3.up * (Time.deltaTime * riseSpeed);
            if (slammerTransform.position.y >= origin.y)
            {
                Reset();
            }
        }
    }

    public void DisableSlammer()
    {
        StopAllCoroutines();
        myRend.gameObject.SetActive(false);
        myRend.enabled = false;
        myCollider.enabled = false;
        currentState = MovingState.None;
    }

    void Reset()
    {
        currentState = MovingState.None;
        myRend.sprite = sleepSprite;
        sleeping = true;
        myCollider.enabled = true;
        if (autoAttack)
            StartCoroutine(AutoAttack());
    }

    IEnumerator AutoAttack()
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(Slam());
    }

    IEnumerator HitGround()
    {

        if(myPlatform != null)
        {
            myPlatform.TakeDamage();
        }

        currentState = MovingState.None;
        yield return new WaitForSeconds(.5f);
        currentState = MovingState.Rising;
    }

    IEnumerator Slam()
    {
        sleeping = false;
        myCollider.enabled = false;
        yield return new WaitForSeconds(sleepSeconds - .5f);
        myRend.sprite = awakeSprite;
        yield return new WaitForSeconds(.5f);
        currentState = MovingState.Falling;
    }
}
