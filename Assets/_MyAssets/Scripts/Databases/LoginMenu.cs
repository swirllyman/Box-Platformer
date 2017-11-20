using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenu : MonoBehaviour {
    
    public Text feedbackText;
    [Header("Credential Input")]
    public GameObject credentialPane;
    public InputField accountNameInput;
    public InputField passwordInput;

    [Header("Account Button")]
    public GameObject accountButton;
    public Text accountNameText;

    bool connecting = false;

    #region Canvas Buttons

    public void ClearFeedbackText()
    {
        if(feedbackText.text == "Incorrect Username/PW")
            feedbackText.text = "";
    }
    public void LoginUser()
    {
        if (connecting) return;
        StartCoroutine(LoginToDB(accountNameInput.text, passwordInput.text));
    }

    public void LogOut()
    {
        credentialPane.SetActive(true);
        accountButton.SetActive(false);
    }

    #endregion


    IEnumerator LoginToDB(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);
        form.AddField("emailPost", "");
        WWW www = new WWW("http://localhost/box_platformer/InsertUser.php", form);

        yield return www;
        print(www.text);
        if (www.text == "Success")
        {
            feedbackText.text = "Connecting.";
            StartCoroutine(ConnectingPlayerAccount(username));
        }
        else
            feedbackText.text = www.text;
    }

    IEnumerator ConnectingPlayerAccount(string theAccountName)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(.1f, .25f));
        feedbackText.text = "Complete!";
        yield return new WaitForSeconds(UnityEngine.Random.Range(.4f, .6f));
        feedbackText.text = "";
        
        PlayerStats p = new PlayerStats(theAccountName, 0);
        accountNameText.text = theAccountName;
        GameManager.singleton.SetupLocalPlayer(p);
        credentialPane.SetActive(false);
        accountButton.SetActive(true);

    }
}
