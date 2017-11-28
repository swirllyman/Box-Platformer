using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {
    public float destroyTimer = 1.0f;
	void Update () {
        destroyTimer -= Time.deltaTime;
        if(destroyTimer <= 0.0f)
        {
            Destroy(gameObject);
        }
	}
}
