using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTopScore {

    //Get top level sore overall
    public static IEnumerator GetTopLevelScore(string levelName)
    {
        WWWForm form = new WWWForm();
        form.AddField("levelnamePost", levelName);
        WWW www = new WWW("http://localhost/box_platformer/GetTopLevelScore.php", form);

        yield return www;
        Debug.Log(www.text);
    }

    //Get top level sore for specific player
    public static IEnumerator GetTopPlayerScore(string username, string levelName)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("levelnamePost", levelName);
        WWW www = new WWW("http://localhost/box_platformer/GetTopPlayerScore.php", form);

        yield return www;
        Debug.Log(www.text);
    }
}
