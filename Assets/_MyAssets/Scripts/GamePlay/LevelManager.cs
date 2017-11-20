using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager singleton;
    [Header ("TestingOnly")]
    public GameObject playerPrefab;
    public Player currentPlayer;

    [Header("Level Specific")]
    public int worldNum;
    public int levelNum;
    public Sprite backgroundImg;
    public Transform[] checkPoints;
    public Transform currentcheckPoint;
    public TimedLevel timedLevel;

    void Awake()
    {
        if (singleton != null)
            Destroy(this);
        singleton = this;
    }

	// Use this for initialization
	void Start () {
		if(GameManager.singleton == null)
        {
            GameObject player = Instantiate(playerPrefab, currentcheckPoint.position, currentcheckPoint.rotation);
            currentPlayer = player.GetComponentInChildren<Player>();
        }
        else
        {
            currentPlayer = GameManager.singleton.localPlayer;
        }
        ResetLevel();
    }

    public void EndLevel(bool victory)
    {
        currentPlayer.PauseController(true);
        if (victory)
        {
            if (SceneController.singleton != null)
            {
                StartCoroutine(LoadBackIntoWorld());
                if (timedLevel != null)
                    timedLevel.Finish();
            }
            else if (timedLevel != null)
            {
                timedLevel.Finish();
                StartCoroutine(CompleteLevelAndReset());
            }
        }
        else
        {
            currentPlayer.Die();
            if (timedLevel != null)
            {
                timedLevel.SetupTimedLevel();
            }
        }
    }

    IEnumerator CompleteLevelAndReset()
    {
        yield return new WaitForSeconds(5.0f);
        ResetLevel();
        timedLevel.SetupTimedLevel();
    }

    IEnumerator LoadBackIntoWorld()
    {
        yield return new WaitForSeconds(10.0f);
        SceneController.singleton.LoadWorld(worldNum);
    }

    public void ResetLevel()
    {
        currentPlayer.transform.position = currentcheckPoint.transform.position;
        currentPlayer.transform.rotation = currentcheckPoint.transform.rotation;

        if (timedLevel)
        {
            timedLevel.SetupTimedLevel();
        }
    }

    public Vector2 GetLevelTimers()
    {
        return new Vector2(timedLevel.currentLevelTimer, timedLevel.levelTime);
    }
}
