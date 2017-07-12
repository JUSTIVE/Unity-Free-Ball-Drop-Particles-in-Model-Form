using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCamera : MonoBehaviour {
    public GameObject target;
	// Use this for initialization
	void Start () {
        this.transform.LookAt(target.transform);
	}
	
}
