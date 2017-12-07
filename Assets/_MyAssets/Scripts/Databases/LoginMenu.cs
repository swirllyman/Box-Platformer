using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LoginMenu : MonoBehaviour {
    
    public Text feedbackText;
    [Header("Credential Input")]
    public GameObject credentialPane;
    public InputField accountNameInput;
    public InputField passwordInput;
    public Text submitButtonText;

    [Header("Account Button")]
    public GameObject accountButton;
    public Text accountNameText;

    AccountPanel myAccount;
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
        if (myAccount.active)
        {
            CloseAccount();
        }
        credentialPane.SetActive(true);
        accountButton.SetActive(false);
        GameManager.singleton.ResetGameSession();
    }

    public void OpenAccount()
    {
        //print("Open Player Progess Window");
        if(GameManager.singleton.localPlayer != null)
        {
            GameManager.singleton.localPlayer.PauseController(true);
        }
        myAccount.Init();
    }

    public void CloseAccount()
    {
        //print("Close Player Progess Window");
        if (GameManager.singleton.localPlayer != null)
        {
            GameManager.singleton.localPlayer.PauseController(false);
        }
        myAccount.Close();
    }
     
    public void CheckAccountName()
    {
        //print("Checking for account name");
        StartCoroutine(CheckForUsername(accountNameInput.text));
    }

    public void DeleteAccountRecords()
    {
        print("Deleting All Scores for player");
        if (GameManager.singleton.localPlayer != null)
        {
            StartCoroutine(DeleteAccountRecords(GameManager.singleton.localPlayer.username));
        }
    }

    #endregion


    void Start()
    {
        DontDestroyOnLoad(gameObject);
        myAccount = GetComponent<AccountPanel>();
    }

    void Update()
    {
        if(accountNameInput.isFocused && Input.GetKeyDown(KeyCode.Tab))
        {
            accountNameInput.DeactivateInputField();
            passwordInput.ActivateInputField();
        }
        else if (passwordInput.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                passwordInput.DeactivateInputField();
                accountNameInput.ActivateInputField();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) && myAccount.active)
        {
            EventSystem.current.SetSelectedGameObject(null);
            CloseAccount();
        }
    }

    #region DB Calls
    IEnumerator CheckForUsername(string username)
    {
        WWWForm form = new WWWForm();
        //print("Checking for username: " + username);
        form.AddField("usernamePost", username);
        WWW www = new WWW("https://shipment.000webhostapp.com/CheckForUserName.php", form);
        yield return www;
        if (www.text == "Success")
        {
            submitButtonText.text = "Login";
            feedbackText.text = "Account is created. Enter P/W.";
        }
        else
        {
            submitButtonText.text = "Create";
            feedbackText.text = "Create a new Account!";
        }
    }

    IEnumerator LoginToDB(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", username);
        form.AddField("passwordPost", password);
        form.AddField("emailPost", "");

        feedbackText.text = "Authenticating.";
        WWW www = new WWW("https://shipment.000webhostapp.com/InsertUser.php", form);

        yield return www;
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

    IEnumerator DeleteAccountRecords(string theAccountName)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamePost", theAccountName);
        
        WWW www = new WWW("https://shipment.000webhostapp.com/DeleteUserRecords.php", form);

        yield return www;
        if (www.text == "Success")
        {
            print("Scores deleted.");
        }
        else
            print("Scores failed to delete.");
    }
    #endregion
}
