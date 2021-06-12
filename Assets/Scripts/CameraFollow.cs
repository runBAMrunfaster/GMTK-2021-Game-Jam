using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private GameObject cameraObject;
    [SerializeField] float cameraXTolerance = 1;
    [SerializeField] float cameraYTolerance = 1;
    [SerializeField] float cameraSnapSpeed = 1;
    [SerializeField] float cameraZoom = -10;

    [SerializeField] GameObject camTarget;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(camTarget.transform.position.x - cameraObject.transform.position.x) >= 2 || Mathf.Abs(camTarget.transform.position.y - cameraObject.transform.position.y) >= 2)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(camTarget.transform.position.x, camTarget.transform.position.y, cameraZoom), cameraSnapSpeed * Time.deltaTime);
        }
    }
}