using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    enum MovementState { init, end, start}
    bool begin;
    bool end;
    bool start;

    public float orthoStartSize;
    public float orthoEndSize;

    public Transform startPos;
    public Transform endPos;
    public float speed;
    public float zoomSpeed;
    private float startTime;
    private float journeyLength;


    public bool tutorialStart = false;

    float energy;


    void Start()
    {
        startTime = Time.time;
        begin = false;
        end = false;
        start = false;
    }

    void Update()
    {
        if (begin)
        {
            Camera.main.orthographicSize = Mathf.Lerp(orthoStartSize, orthoEndSize, (Time.time - startTime) * zoomSpeed);
        }
        if (begin && !end)
        {
            journeyLength = Vector3.Distance(startPos.position, endPos.position);
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPos.position, endPos.position, fracJourney);
            if (transform.position == endPos.position)
            {
                end = true;
            }
        }

        if (start)
        {
            Camera.main.orthographicSize = Camera.main.orthographicSize * ((energy + 6) / 7);
        }
    }

    public void StartTutorialLevel()
    {
        tutorialStart = false;
    }

    void OnGUI()
    {
        if (!begin & !tutorialStart)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height * .75f, 100, 50), "Ready"))
            {
                startTime = Time.time;
                begin = true;
            }
        }
        if (end && !start)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height * .75f, 100, 50), "Start"))
            {
                GameObject.FindGameObjectWithTag("Player").SendMessage("beginGame");
                start = true;
            }
        }
    }
}