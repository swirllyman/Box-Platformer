using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextShiner : MonoBehaviour {
    public Color[] colors;
    public float totalLerpTime;
    public bool startShining = false;

    int currentIdx = 0;
    Text myText;
    float lerpTime;
    Color currentColor;
    Color destinationColor;

    bool init = false;

    void Start()
    {
        myText = GetComponent<Text>();
        currentColor = colors[currentIdx % colors.Length];
        destinationColor = colors[(++currentIdx) % colors.Length];
        if (startShining)
        {
            init = true;
        }
    }

    void Update()
    {
        if (init)
        {
            float percentComplete = (Time.time - lerpTime) / totalLerpTime;
            myText.color = Color.Lerp(currentColor, destinationColor, percentComplete);

            if (percentComplete >= .999f)
            {
                SwapColors();
            }
        }
    }


    void SwapColors()
    {
        lerpTime = Time.time;
        currentColor = colors[currentIdx % colors.Length];
        destinationColor = colors[(++currentIdx) % colors.Length];
    }

    public void StartShine()
    {
        init = true;
    }
}
