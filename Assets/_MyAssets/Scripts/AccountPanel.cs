using System.Collections;
using System.Globalization;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AccountPanel : MonoBehaviour {

    public bool active = false;
    public GameObject accountPanel;
    public string[] levelNames;

    public LevelUI[] levelUIs;
    public Text worldName;

    int worldIdx = 0;

    public void Init()
    {
        accountPanel.SetActive(true);
        active = true;
        worldIdx = 0;
        SetupLevel();
    }

    public void Close()
    {
        accountPanel.SetActive(false);
        active = false;
        worldIdx = 0;
    }

    public void ChangeLevelView(int dir)
    {
        //a % b + b) % b;
        int a = (worldIdx + (1 * dir));

        worldIdx = (a % levelNames.Length + levelNames.Length) % levelNames.Length;
        SetupLevel();
    }

    void SetupLevel()
    {
        worldName.text = levelNames[worldIdx];

        string username = "Default";
        if (GameManager.singleton != null)
        {
            username = GameManager.singleton.localPlayer.username;
            //print("using Default");
        }

        for (int i = 0; i < levelUIs.Length; i++)
        {
            string levelName = (worldIdx + 1) + "-" + (i + 1);
            levelUIs[i].levelName.text = levelName;
            StartCoroutine(GetTopPlayerScore(username, levelName, i));
            StartCoroutine(GetTopLevelScore(levelName, levelUIs[i].bestName, levelUIs[i].bestTime));
        }
    }

    #region DB Calls
    IEnumerator GetTopLevelScore(string levelName, Text nameText, Text timeText)
    {
        WWWForm form = new WWWForm();
        form.AddField("levelnamePost", levelName);
        WWW www = new WWW("https://shipment.000webhostapp.com/GetTopLevelScore.php", form);

        //print("Getting top score for: " + levelName);

        yield return www;
        if (System.String.IsNullOrEmpty(www.text) || www.text == "N/A" || www.text == "|")
        {
            //print("No Scores for this level");
            timeText.text = "N/A";
            nameText.text = "N/A";
        }
        else
        {
            //print("Top Level Score gathered " + www.text);
            string[] nameAndTime = www.text.Split('|');
            nameText.text = nameAndTime[0];
            float topScore = float.Parse(nameAndTime[1], CultureInfo.InvariantCulture.NumberFormat);
            timeText.text = topScore.ToString("F2");
        }
    }


    IEnumerator GetTopPlayerScore(string username, string levelName, int levelNum)
    {
        if (username == "")
        {
            levelUIs[levelNum].levelTime.text = "N/A";
        }
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("levelnamePost", levelName);
        WWW www = new WWW("https://shipment.000webhostapp.com/GetTopPlayerScore.php", form);

        yield return www;


        if (System.String.IsNullOrEmpty(www.text) || www.text == "N/A" || www.text == "|")
        {
            //print("No Scores for this level: "+username + ", "+levelName);
            levelUIs[levelNum].levelTime.text = "N/A";
            for (int i = 0; i < 3; i++)
            {
                levelUIs[levelNum].stars[i].enabled = false;
                levelUIs[levelNum].levelTime.text = "N/A";
            }
        }
        else
        {
            //print("Scores for: " + username + ", " + levelName + ", " + levelNum);
            //print("Top Player Score gathered " + www.text);
            string[] scoreAndTime = www.text.Split('|');
            int score = int.Parse(scoreAndTime[0], CultureInfo.InvariantCulture.NumberFormat);

            for (int i = 0; i < score; i++)
            {
                levelUIs[levelNum].stars[i].enabled = true;
            }

            float time = float.Parse(scoreAndTime[1], CultureInfo.InvariantCulture.NumberFormat);
            levelUIs[levelNum].levelTime.text = time.ToString("F2");
        }
    }

    #endregion
}






[System.Serializable]
public struct LevelUI
{
    public Text levelName;
    public Text levelTime;
    public Text bestTime;
    public Text bestName;
    public Image[] stars;
}
