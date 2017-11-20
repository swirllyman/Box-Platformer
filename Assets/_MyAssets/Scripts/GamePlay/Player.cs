using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public Renderer myRend;
    public string username;
    PlayerController myController;
    Rigidbody body;
    

	// Use this for initialization
	void Start () {
        myController = GetComponent<PlayerController>();
        body = GetComponent<Rigidbody>();
        SceneManager.sceneLoaded += LevelLoaded;
	}
	

    void LevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Overworld")
        {
            GetComponentInParent<PlayerAndCameraHolder>().myAudioListener.enabled = true;
        }
        if(scene.name.Substring(1, 1) == "-")
        {
            print("Timed Level");
            PauseController(true);
        }
        else
        {
            PauseController(false);
        }
    }
    
    public void PauseController(bool pause)
    {
        myController.enabled = !pause;
        body.velocity = Vector3.zero;
    }

    public void Die()
    {
        body.velocity = Vector3.zero;
        LevelManager.singleton.ResetLevel();
        StartCoroutine(DeathFlash());
    }

    IEnumerator DeathFlash()
    {
        yield return new WaitForSeconds(.05f);
        myRend.enabled = false;
        yield return new WaitForSeconds(.05f);
        myRend.enabled = true;
        yield return new WaitForSeconds(.05f);
        myRend.enabled = false;
        yield return new WaitForSeconds(.05f);
        myRend.enabled = true;
        yield return new WaitForSeconds(.05f);
        myRend.enabled = false;
        yield return new WaitForSeconds(.05f);
        myRend.enabled = true;
        yield return new WaitForSeconds(.05f);
        myRend.enabled = false;
        yield return new WaitForSeconds(.05f);
        myRend.enabled = true;
        yield return new WaitForSeconds(.05f);
        myRend.enabled = false;
        yield return new WaitForSeconds(.05f);
        myRend.enabled = true;
    }
}
