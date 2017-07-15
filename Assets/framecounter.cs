using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class framecounter : MonoBehaviour
{
    public Text text;
    //values for frame counting
    private float fpsSum = 0.0f;
    private int frameNum = 0;
    private int frameStep = 0;
    private StreamWriter sw;
    public bool showFrame = true;
    // Use this for initialization
    void Start()
    {
        InitText();
        sw = new StreamWriter("frames.txt");
    }
    void InitText()
    {
        if (showFrame)
        {
            if (text != null)
                text.text = "\n";
        }
    }
    // Update is called once per frame
    void Update()
    {
        frameNum++;
        fpsSum += 1.0f / (Time.deltaTime);
        if (frameNum > 200)
        {
            if (frameStep < 100)
            {
                if (text != null)
                {
                    if (showFrame)
                        text.text += "frame = " + fpsSum / 200.0f + "\n";


                }
                sw.WriteLine((fpsSum / 200.0f).ToString());
                frameNum = 0;
                fpsSum = 0;
                frameStep++;
            }
        }
    }
}
