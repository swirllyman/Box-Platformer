using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(GameManager.singleton == null)
        {
            //load additively
            SceneManager.LoadScene("AccountCanvasAndManagers", LoadSceneMode.Additive);
        }
        else
        {
            //do nothing
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
