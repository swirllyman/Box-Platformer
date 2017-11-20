using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedLevel : MonoBehaviour {

    public float levelTime = 0.0f;
    public float currentLevelTimer;

    public Text levelNameText;
    public Text levelTimeText;
    public Text startTimeText;
    public Animation levelStartAnim;
    LevelManager manager;

    bool levelStarted = false;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        manager = GetComponent<LevelManager>();
        levelNameText.text = manager.worldNum + "-" + manager.levelNum;
        SetupTimedLevel();
    }

    public void SetupTimedLevel()
    {
        levelStarted = false;
        LevelManager.singleton.currentPlayer.PauseController(true);
        currentLevelTimer = levelTime;
        levelTimeText.text = currentLevelTimer.ToString("F2");
        levelStartAnim.Play();
    }

    public void Finish()
    {
        levelStarted = false;
    }
    
    public void UpdateTime(int time)
    {
        if (time > 0)
            startTimeText.text = time + "";
        else
        {
            startTimeText.text = "GO!";
            LevelManager.singleton.currentPlayer.PauseController(false);
            levelStarted = true;
        }
    }

    void Update()
    {
        if (levelStarted)
        {
            if(currentLevelTimer > 0.0f)
            {
                currentLevelTimer -= Time.deltaTime;
                levelTimeText.text = currentLevelTimer.ToString("F2");
            }
            else
            {
                LevelManager.singleton.EndLevel(false);
            }
        }
    }
}
