using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelPipe : MonoBehaviour {

    public int levelNum;
    public int worldNum;
    public Animator anim;

    public Text levelNumText;
    public Text topTimeTime;
    public Text playerTopTimeText;

    public Image[] stars;

    bool statsOpen = false;

    void Start()
    {
        levelNumText.text = levelNum.ToString("F0") + "-" + worldNum.ToString("F0");
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            anim.SetBool("Open", true);
            statsOpen = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player")
        {
            anim.SetBool("Open", false);
            statsOpen = false;
        }
    }


    void Update()
    {
        if (statsOpen)
        {
            if (Input.GetAxisRaw("Vertical") < -.75f)
            {
                SceneController.singleton.LoadLevel(worldNum, levelNum);
                statsOpen = false;
            }
        }
    }
}
