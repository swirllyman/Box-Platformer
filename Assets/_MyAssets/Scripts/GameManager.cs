using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager singleton;

    public GameObject playerPrefab;

    public Player localPlayer;
    PlayerStats localPlayerStats;

    void Awake()
    {
        if(singleton != null)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(gameObject);
        singleton = this;
    }

    public void SetupLocalPlayer(PlayerStats newPlayer)
    {
        localPlayerStats = newPlayer;
        if (localPlayer != null)
        {
            localPlayer.username = localPlayerStats.accountName;
        }
        else
        {
            SceneController.singleton.InitialLoad();
        }
    }

    public void CreateLocalPlayerPrefab()
    {
        GameObject player = Instantiate(playerPrefab);
        DontDestroyOnLoad(player);
        localPlayer = player.GetComponentInChildren<Player>();

        //Started game without signing in
        if (localPlayerStats == null)
        {
            localPlayer.username = "Guest";
        }
        else
        {
            localPlayer.username = localPlayerStats.accountName;
        }
    }

    public void ResetGameSession()
    {
        Destroy(localPlayer.transform.parent.gameObject);
        SceneController.singleton.LoadMainMenu();
    }

}
