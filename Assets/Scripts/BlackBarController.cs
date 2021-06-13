using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackBarController : MonoBehaviour
{
    [SerializeField] GameObject bbUpper;
    [SerializeField] GameObject bbLower;

    [SerializeField] Vector3 bblOn;
    [SerializeField] Vector3 bblOff;

    [SerializeField] Vector3 bbuOn;
    [SerializeField] Vector3 bbuOff;


    public void BlackBars(bool toggle)
    {
        bbUpper.SetActive(toggle);
        bbLower.SetActive(toggle);
    }

}
