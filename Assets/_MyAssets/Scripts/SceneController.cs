using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour {

    public static SceneController singleton;

    public World[] worlds;
    public string settingsLevel;
    public string accountLevel;

    void Awake()
    {
        if (singleton != null)
        {
            Destroy(this);
        }
        singleton = this;
    }

    /// <summary>
    /// Use to load to specific level
    /// </summary>
    /// <param name="worldNum">World idx</param>
    /// <param name="levelNum">Level idx</param>
    public void LoadLevel(int worldNum = 0, int levelNum = 0)
    {
        StartLoadingSequence(worlds[worldNum].levels[levelNum - 1].levelName);
    }

    public void LoadWorld(int worldNum)
    {
        StartLoadingSequence(worlds[worldNum].worldName);
    }

    public void LoadOverworld()
    {
        StartLoadingSequence(worlds[0].worldName);
    }

    public void LoadMainMenu()
    {
        StartLoadingSequence("Main");
    }

    void StartLoadingSequence(string sceneToLoad, bool resetPlayer = true)
    {
        if (resetPlayer)
        {
            if (GameManager.singleton.localPlayer != null)
                GameManager.singleton.localPlayer.ResetParent();
        }
        SceneManager.LoadScene(sceneToLoad);
    }

    #region Buttons

    public void InitialLoad()
    {
        GameManager.singleton.CreateLocalPlayerPrefab();
        StartLoadingSequence(worlds[0].worldName, false);
    }

    public void LoadSettings()
    {
        StartLoadingSequence(settingsLevel);
    }

    public void LoadAccount()
    {
        StartLoadingSequence(accountLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    
    #endregion
}

[System.Serializable]
public struct World
{
    public string worldName;
    public Level[] levels;

}


[System.Serializable]
public struct Level
{
    public string levelName;
}
