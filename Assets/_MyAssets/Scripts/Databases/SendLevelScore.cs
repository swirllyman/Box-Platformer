using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendLevelScore
{
    public static IEnumerator SendScores(string username, string levelName, string levelTime, string levelScore)
    {
        Debug.Log("sending score");
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("levelnamePost", levelName);
        form.AddField("leveltimePost", levelTime);
        form.AddField("levelscorePost", levelScore);
        WWW www = new WWW("http://localhost/box_platformer/InsertTime.php", form);

        yield return www;
        Debug.Log(www.text);
    } 
}
