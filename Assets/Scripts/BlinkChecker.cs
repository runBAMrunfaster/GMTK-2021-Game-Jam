using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkChecker : MonoBehaviour
{
    bool canBlink = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D()
    {
        canBlink = false;
    }

    void OnTriggerExit2D()
    {
        canBlink = true;
    }

    public bool GetCanBlink()
    {
        return canBlink;
    }
}
