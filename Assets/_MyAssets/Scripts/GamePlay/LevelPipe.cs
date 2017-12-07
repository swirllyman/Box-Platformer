using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPipe : MonoBehaviour
{

    public int levelNum;
    public int worldNum;
    public Animator anim;

    public Text levelNumText;
    public Text topNameText;
    public Text topTimeTime;
    public Text yourBestText;

    public Image[] stars;

    public AudioClip openAudio;
    public AudioClip closeAudio;
    AudioSource myAudio;

    bool statsOpen = false;

    void Start()
    {
        myAudio = GetComponentInChildren<AudioSource>();
        levelNumText.text = worldNum.ToString("F0") + "-" + levelNum.ToString("F0");

        StartCoroutine(GetTopLevelScore(levelNumText.text));

        string username = "joe";
        if (GameManager.singleton != null)
            username = GameManager.singleton.localPlayer.username;
        StartCoroutine(GetTopPlayerScore(username, levelNumText.text));
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            myAudio.clip = openAudio;
            myAudio.Play();
            anim.SetBool("Open", true);
            statsOpen = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            myAudio.clip = closeAudio;
            myAudio.Play();
            anim.SetBool("Open", false);
            statsOpen = false;
        }
    }


    void Update()
    {
        if (statsOpen)
        {
            if (Input.GetAxisRaw("Vertical") < -.75f)
            {
                SceneController.singleton.LoadLevel(worldNum, levelNum);
                statsOpen = false;
            }
        }
    }

    #region DB calls
    IEnumerator GetTopLevelScore(string levelName)
    {
        WWWForm form = new WWWForm();
        form.AddField("levelnamePost", levelName);
        WWW www = new WWW("https://shipment.000webhostapp.com/GetTopLevelScore.php", form);

        //print("Getting top score for: " + levelName);

        yield return www;
        if (System.String.IsNullOrEmpty(www.text) || www.text == "N/A" || www.text == "|")
        {
            //print("No Scores for this level");
            topTimeTime.text = "N/A";
        }
        else
        {
            //print("Top Level Score gathered "+www.text);
            string[] nameAndTime = www.text.Split('|');
            topNameText.text = nameAndTime[0];
            float topScore = float.Parse(nameAndTime[1], CultureInfo.InvariantCulture.NumberFormat);
            topTimeTime.text = topScore.ToString("F2");
        }
    }


    IEnumerator GetTopPlayerScore(string username, string levelName)
    {
        //print("Getting highest score for "+username+" for level "+levelName);
        if (username == "")
        {
            yourBestText.text = "N/A";
        }
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("levelnamePost", levelName);
        WWW www = new WWW("https://shipment.000webhostapp.com/GetTopPlayerScore.php", form);

        yield return www;


        if (System.String.IsNullOrEmpty(www.text) || www.text == "N/A" || www.text == "|")
        {
            //print("No Scores for this level");
            yourBestText.text = "N/A";
        }
        else
        {
            //print("Top Level Score gathered " + www.text);
            string[] scoreAndTime = www.text.Split('|');
            int score = int.Parse(scoreAndTime[0], CultureInfo.InvariantCulture.NumberFormat);

            for(int i = 0; i < score; i++)
            {
                stars[i].enabled = true;
            }

            float time = float.Parse(scoreAndTime[1], CultureInfo.InvariantCulture.NumberFormat);
            yourBestText.text = time.ToString("F2");
        }
    }

    #endregion
}
