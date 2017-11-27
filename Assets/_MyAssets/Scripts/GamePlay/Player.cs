using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public Renderer myRend;
    public string username;
    PlayerAndCameraHolder pAndCHolder;
    PlayerController myController;
    Rigidbody body;
    

	// Use this for initialization
	void Start () {
        pAndCHolder = GetComponentInParent<PlayerAndCameraHolder>();
        myController = GetComponent<PlayerController>();
        body = GetComponent<Rigidbody>();
        SceneManager.sceneLoaded += LevelLoaded;
	}

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= LevelLoaded;
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
    
    //Might get called before Start
    public void PauseController(bool pause)
    {
        if (myController == null)
            myController = GetComponent<PlayerController>();
        myController.enabled = !pause;

        if(body == null)
            body = GetComponent<Rigidbody>();
        body.velocity = Vector3.zero;
    }

    public void Die()
    {
        body.velocity = Vector3.zero;
        LevelManager.singleton.ResetLevel();
        StartCoroutine(DeathFlash());

        if (transform.parent != pAndCHolder.transform)
            transform.parent = pAndCHolder.transform;
    }

    public void ResetParent()
    {
        transform.parent = pAndCHolder.transform;
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
