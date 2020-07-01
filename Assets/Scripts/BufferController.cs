using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferController : MonoBehaviour
{
    private RoboController roboController;
    public TextAsset jsonFile;
    // Start is called before the first frame update
    void Start()
    {
        roboController = GetComponent<RoboController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
