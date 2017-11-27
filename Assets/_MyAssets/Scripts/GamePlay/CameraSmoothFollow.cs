using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSmoothFollow : MonoBehaviour {

    public Transform targetToFollow;
    public float followSpeed;
    public Transform backgroundImage;

    SpriteRenderer backgroundRend;
	
    void Start()
    {
        SceneManager.sceneLoaded += LevelLoaded;
        backgroundRend = backgroundImage.GetComponentInChildren<SpriteRenderer>();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= LevelLoaded;
    }

    void LevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (LevelManager.singleton != null)
        {
            backgroundRend.sprite = LevelManager.singleton.backgroundImg;
        }
    }

	// Update is called once per frame
	void LateUpdate ()
    {
        if(transform.position.x < 110 && transform.position.x > -85)
            backgroundImage.localPosition = new Vector3(-transform.position.x / 20, backgroundImage.localPosition.y, backgroundImage.localPosition.z);

        transform.position = Vector3.Lerp(transform.position, targetToFollow.position, Time.deltaTime * followSpeed);

        if(transform.position.y < 1.75f)
        {
            transform.position = new Vector3(transform.position.x, 1.75f, transform.position.z);
        }
	}
}
