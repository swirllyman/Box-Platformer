using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingPlatform : MonoBehaviour {

    public Transform[] destinations;
    public float totalLerpTime;
    public float transitionTime;
    Player currentPlayer;

    float currentLerpTime;

    Transform origin;
    Transform destination;

    int currentIdx = 0;
    bool transitioning = false;

    void Start()
    {
        origin = destinations[currentIdx % destinations.Length];
        destination = destinations[(++currentIdx) % destinations.Length];
    }

    void Update()
    {
        if (!transitioning)
        {
            float percentComplete = (Time.time - currentLerpTime) / totalLerpTime;

            //percentComplete = percentComplete*percentComplete*percentComplete
            percentComplete = SmootherOut(percentComplete);
            transform.position = Vector3.Lerp(origin.position, destination.position, percentComplete);

            if (percentComplete >= .999999999f)
            {
                transitioning = true;
                StartCoroutine(SwapAfterDelay());
            }
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            currentPlayer = c.GetComponent<Player>();
            currentPlayer.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            currentPlayer.ResetParent();
            currentPlayer = null;
        }
    }

    float SmootherOut(float t)
    {
        return t * t * t * (t * (6f * t - 15f) + 10f);
    }


    void SwapDestinations()
    {
        transitioning = false;
        currentLerpTime = Time.time;
        origin = destinations[currentIdx % destinations.Length];
        destination = destinations[(++currentIdx) % destinations.Length];
    }

    IEnumerator SwapAfterDelay()
    {
        yield return new WaitForSeconds(transitionTime);
        SwapDestinations();
    }
}
