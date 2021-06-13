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

    [SerializeField] float fadeTime = 5;

    AudioSource audioSource;
    [SerializeField] float musicMaxVolume = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        cameraObject = this.gameObject;
        audioSource.volume = 0;
        StartCoroutine("FadeIn", audioSource);
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(camTarget.transform.position.x - cameraObject.transform.position.x) >= 2 || Mathf.Abs(camTarget.transform.position.y - cameraObject.transform.position.y) >= 2)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(camTarget.transform.position.x, camTarget.transform.position.y, cameraZoom), cameraSnapSpeed * Time.deltaTime);
        }
    }

    public void FadeInBGM()
    {
        StartCoroutine("FadeIn", audioSource);
    }

    public void FadeOutBGM()
    {
        StartCoroutine("FadeOut", audioSource);
    }

    public IEnumerator FadeIn(AudioSource audioSource)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, musicMaxVolume, currentTime / fadeTime);
            yield return null;
        }
        yield break;
    }

    public IEnumerator FadeOut(AudioSource audioSource)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < fadeTime)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, 0.25f, currentTime / fadeTime);
            yield return null;
        }
        yield break;
    }
}
