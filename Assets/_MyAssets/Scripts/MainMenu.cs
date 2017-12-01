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
	
	public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
