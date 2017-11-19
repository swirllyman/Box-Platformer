using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldPortal : MonoBehaviour {

    public int worldNum;
    public Animator anim;
    public ParticleSystem particles;
    public Text portalText;
    bool portalOpen = false;

    void Start()
    {
        portalText.text = "World " + worldNum;
    }

	void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            anim.SetBool("Open", true);
            portalOpen = true;
            particles.Stop();
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            anim.SetBool("Open", false);
            portalOpen = false;
            particles.Play();
        }
    }


    void Update()
    {
        if (portalOpen)
        {
            if(Input.GetAxisRaw("Vertical") > .75f)
            {
                SceneController.singleton.LoadWorld(worldNum);
                portalOpen = false;
            }
        }
    }

}
