using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopper : MonoBehaviour {

    public int target;
    private int frame;
    // Use this for initialization
    void Start()
    {
        frame = 0;
    }

    // Update is called once per frame
    void Update()
    {
        frame++;
        if (frame == target)
            UnityEditor.EditorApplication.isPaused = true;
    }
}
