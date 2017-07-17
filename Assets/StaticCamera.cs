using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCamera : MonoBehaviour {
    public GameObject target;
	// Use this for initialization
	void Start () {
        transform.position = new Vector3(0.2f, 0.9f, 0.2f);
        this.transform.LookAt(target.transform);
	}
	
}
