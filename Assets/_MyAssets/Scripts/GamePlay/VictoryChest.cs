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
    public int worldNum;
    public int levelNum;
    public Text levelNameText;
    public Text yourTimeText;
    public Text yourBestText;

    public Image[] stars;

    void Start()
    {
        anim = GetComponent<Animator>();
        levelNameText.text = worldNum + "-" + levelNum;
    }


	void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player" &! opened)
        {
            //c.GetComponent<Player>().LevelFinished();
            anim.SetBool("Open", true);
            particles.SetActive(true);
            EndLevel();
        }
    }


    void EndLevel()
    {
        LevelManager.singleton.EndLevel(true);
        canvasAnim.SetBool("Open", true);

        Vector2 levelTotals = LevelManager.singleton.GetLevelTimers();

        float percentComplete = levelTotals.x / levelTotals.y;

        int levelScore = 0;

        if (percentComplete > .75f)
            levelScore = 3;
        else if (percentComplete > .5f)
            levelScore = 2;
        else if (percentComplete > .25f)
            levelScore = 1;

        for (int i = 0; i < levelScore; i++)
            stars[i].enabled = true;

        yourTimeText.text = "" + levelTotals.x.ToString("F2");

        string username = "joe";
        if(GameManager.singleton != null)
            username = GameManager.singleton.localPlayer.username;

        StartCoroutine(SendLevelScore.SendScores(username, levelNameText.text, levelTotals.x.ToString("F2"), levelScore.ToString("F0")));

        StartCoroutine(GetTopPlayerScore(username));
    }

    IEnumerator GetTopPlayerScore(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("levelnamePost", levelNameText.text);
        WWW www = new WWW("http://localhost/box_platformer/GetTopPlayerScore.php", form);

        yield return www;
        string topPlayerScore = www.text;
        float topScore = float.Parse(topPlayerScore, CultureInfo.InvariantCulture.NumberFormat);

        yourBestText.text = topScore.ToString("F2");
    }
}
