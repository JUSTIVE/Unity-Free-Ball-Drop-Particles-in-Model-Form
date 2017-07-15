using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCamera : MonoBehaviour {
    public GameObject target;
	// Use this for initialization
	void Start () {
        transform.position = new Vector3(4, 8, 4);
        this.transform.LookAt(target.transform);
	}
	
}
