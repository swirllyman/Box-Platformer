using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour {

    public Transform targetToFollow;
    public float followSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = Vector3.Lerp(transform.position, targetToFollow.position, Time.deltaTime * followSpeed);

        if(transform.position.y < 1.75f)
        {
            transform.position = new Vector3(transform.position.x, 1.75f, transform.position.z);
        }
	}
}
