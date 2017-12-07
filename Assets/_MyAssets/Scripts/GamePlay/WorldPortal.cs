using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldPortal : MonoBehaviour {

    public int worldNum;
    public Animator anim;
    public ParticleSystem particles;
    public Text portalText;
    public Text starCount;
    [Header("Audio")]
    public AudioSource openCloseAudio;
    public AudioSource idleSounds;
    public AudioClip portalOpenSound;
    public AudioClip portalCloseSound;
    public AudioClip[] idleSoundClips;

    bool portalOpen = false;

    void Start()
    {
        portalText.text = "World " + worldNum;
        if(GameManager.singleton != null)
        {
            //StartCoroutine(GetWorldStars());
            starCount.text = "N/A";
        }
    }

	void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Player")
        {
            openCloseAudio.clip = portalOpenSound;
            openCloseAudio.Play();
            idleSounds.Stop();
            anim.SetBool("Open", true);
            portalOpen = true;
            particles.Stop();
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            openCloseAudio.clip = portalCloseSound;
            openCloseAudio.Play();
            idleSounds.Stop();
            anim.SetBool("Open", false);
            portalOpen = false;
            particles.Play();
        }
    }

    public void PlayIdleSound(int soundNum)
    {
        idleSounds.clip = idleSoundClips[soundNum];
        idleSounds.Play();
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


    #region DB Calls
    IEnumerator GetWorldStars()
    {
        WWWForm form = new WWWForm();
        //print("Checking username: " + GameManager.singleton.localPlayer.username);
        //print("Level: " + worldNum + "-");
        form.AddField("usernamePost", GameManager.singleton.localPlayer.username);
        form.AddField("levelnamePost", worldNum+"-");
        WWW www = new WWW("https://shipment.000webhostapp.com/GetWorldStars.php", form);

        yield return www;
        //print(www.text);
        starCount.text = www.text;
    }
    #endregion
}
