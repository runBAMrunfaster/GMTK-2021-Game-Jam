using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whoopsie : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Something went whoopsie!");
        if(other.gameObject.tag == "Player")
        {
            other.transform.position = transform.position;
        }
    }
}
