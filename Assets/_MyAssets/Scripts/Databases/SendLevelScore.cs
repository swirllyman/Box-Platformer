using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendLevelScore
{
    public static IEnumerator SendScores(string username, string levelName, string levelTime, string levelScore)
    {
        Debug.Log("sending score for: " + username);
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("levelnamePost", levelName);
        form.AddField("leveltimePost", levelTime);
        form.AddField("levelscorePost", levelScore);
        WWW www = new WWW("https://shipment.000webhostapp.com/InsertTime.php", form);
        yield return www;
        Debug.Log(www.text);
    }

    //public static IEnumerator SetPowerupForPlayer(string username, string powerup
}
