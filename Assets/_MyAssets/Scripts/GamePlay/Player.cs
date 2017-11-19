using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    public Renderer myRend;

    Rigidbody body;
    

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
        SceneManager.sceneLoaded += LevelLoaded;
	}
	

    void LevelLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetPosition();
        if(scene.name == "Overworld")
        {
            GetComponentInParent<PlayerAndCameraHolder>().myAudioListener.enabled = true;
        }
    }
    
    void ResetPosition()
    {
        body.velocity = Vector3.zero;
        transform.position = LevelManager.singleton.currentcheckPoint.transform.position;
        transform.rotation = LevelManager.singleton.currentcheckPoint.transform.rotation;
    }

    public void Die()
    {
        ResetPosition();
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
