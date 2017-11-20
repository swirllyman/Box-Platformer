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
    }

    public void CreateLocalPlayerPrefab()
    {
        GameObject player = Instantiate(playerPrefab);
        DontDestroyOnLoad(player);
        localPlayer = player.GetComponentInChildren<Player>();
    }

}
