using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static LevelManager singleton;

    public GameObject playerPrefab;

    public Sprite backgroundImg;
    public Transform[] checkPoints;
    public Transform currentcheckPoint;

    void Awake()
    {
        if (singleton != null)
            Destroy(this);
        singleton = this;
    }

	// Use this for initialization
	void Start () {
		if(GameManager.singleton == null)
        {
            Instantiate(playerPrefab);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
