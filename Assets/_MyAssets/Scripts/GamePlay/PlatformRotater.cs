using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRotater : MonoBehaviour {
    public float rotationSpeed = 180.0f;
	
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
	}
}
