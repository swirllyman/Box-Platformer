﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

	void OnTriggerEnter(Collider c) { 
        if(c.tag == "Player")
        {
            c.GetComponent<Player>().Die();
        }
        else if(c.tag == "Slammer")
        {
            c.GetComponentInParent<Slammer>().DisableSlammer();
        }
    }
}
