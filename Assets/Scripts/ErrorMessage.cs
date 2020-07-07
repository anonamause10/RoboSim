using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    public Text text;
    private string displayText;
    private float alphaVal;
    private float alphaDamp = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        alphaVal = Mathf.SmoothDamp(alphaVal,0,ref alphaDamp, 1.5f);
        Color col = text.color; col.a = alphaVal;
        text.color = col;
    }

    public void setDisplayText(string textToDisplay){
        displayText = textToDisplay;
        text.text = displayText;
        alphaVal = 1;
    }

}
