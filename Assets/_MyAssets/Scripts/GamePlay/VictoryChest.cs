using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class VictoryChest : MonoBehaviour {

    public GameObject particles;
    public Animator canvasAnim;

    Animator anim;
    bool opened = false;
    public Text levelNameText;
    public Text yourTimeText;
    public Text yourBestText;
    public Text newPRText;

    public Image[] stars;
    public float[] levelTimeThresholds;

    void Start()
    {
        anim = GetComponent<Animator>();
        levelNameText.text = LevelManager.singleton.worldNum + "-" + LevelManager.singleton.levelNum;
    }


	void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player" &! opened)
        {
            //c.GetComponent<Player>().LevelFinished();
            anim.SetBool("Open", true);
            particles.SetActive(true);
            EndLevel(c.GetComponent<Player>());
        }
    }


    protected virtual void EndLevel(Player p)
    {
        LevelManager.singleton.EndLevel(true);
        canvasAnim.SetBool("Open", true);

        float levelTotal = LevelManager.singleton.GetLevelTime();

        int levelScore = 0;

        if (levelTotal <= levelTimeThresholds[2])
            levelScore = 3;
        else if (levelTotal <= levelTimeThresholds[1])
            levelScore = 2;
        else if (levelTotal <= levelTimeThresholds[0])
            levelScore = 1;

        for (int i = 0; i < levelScore; i++)
            stars[i].enabled = true;

        yourTimeText.text = "" + levelTotal.ToString("F2");

        string username = "joe";
        if(GameManager.singleton != null)
            username = GameManager.singleton.localPlayer.username;

        StartCoroutine(EndOfLevelScores(username, yourTimeText.text, levelScore.ToString("F2")));

    }

    
    protected virtual IEnumerator EndOfLevelScores(string username, string levelTotal, string levelScore)
    {

        //TODO: NEED A PROPER RETURN TO AVOID RACE CONDITIONS IN OPEN ENVIRONMENTS
        StartCoroutine(SendLevelScore.SendScores(username, levelNameText.text, levelTotal, levelScore));
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(GetTopPlayerScore(username));
    }

    IEnumerator GetTopPlayerScore(string username)
    {
        print("Getting highest score for " + username + " for level " + levelNameText.text);
        if (username == "")
        {
            yourBestText.text = "N/A";
        }
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("levelnamePost", levelNameText.text);
        WWW www = new WWW("https://shipment.000webhostapp.com/GetTopPlayerScore.php", form);

        yield return www;


        if (System.String.IsNullOrEmpty(www.text) || www.text == "N/A" || www.text == "|")
        {
            print("No Scores for this level");
            yourBestText.text = "N/A";
        }
        else
        {
            print("Top Level Score gathered " + www.text);
            string[] scoreAndTime = www.text.Split('|');
            int score = int.Parse(scoreAndTime[0], CultureInfo.InvariantCulture.NumberFormat);

            for (int i = 0; i < score; i++)
            {
                stars[i].enabled = true;
            }

            float time = float.Parse(scoreAndTime[1], CultureInfo.InvariantCulture.NumberFormat);
            yourBestText.text = time.ToString("F2");
        }

        yield return new WaitForSeconds(3.0f);
        yourBestText.transform.parent.gameObject.SetActive(true);
        yourTimeText.transform.parent.gameObject.SetActive(true);

        if(yourTimeText.text == yourBestText.text || yourBestText.text == "N/A")
        {
            yourBestText.text = yourTimeText.text;
            yourBestText.GetComponent<TextShiner>().StartShine();
            yourTimeText.GetComponent<TextShiner>().StartShine();

            newPRText.text = yourBestText.text;
            newPRText.transform.parent.gameObject.SetActive(true);
            newPRText.GetComponent<TextShiner>().StartShine();
        }
    }
}
