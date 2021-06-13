using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportInteract : MonoBehaviour
{
    [SerializeField] GameObject destination;
    
    public GameObject GetPortalDest()
    {
        return destination;
    }
}
