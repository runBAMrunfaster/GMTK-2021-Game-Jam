using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AltarFirstVisit : MonoBehaviour
{
    [SerializeField] int altarDiagPhase = 1;
    //[SerializeField] GameObject altar;
    [SerializeField] TextMeshPro altarText;
    [SerializeField] BlackBarController blackBars;
    [SerializeField] AudioSource audioSource;
    [SerializeField] CameraFollow bgm;


    [SerializeField] SoundBank sb;

    private void Start()
    {
        //altarText = altar.GetComponent<TextMeshPro>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            switch(altarDiagPhase)
            {
                case 1:
                    StartCoroutine("FirstAltarCutscene", other.gameObject);
                    

                    break;

                case 2:
                    int heldHelmet = other.gameObject.GetComponent<Character2DController>().GetHeldHelmetID();

                    if(heldHelmet == 0)
                    {
                        StartCoroutine("SecondAltarCutscene", other.gameObject);
                    }

                    break;

                case 3:

                    break;
            }
        }
    }

    IEnumerator FirstAltarCutscene(GameObject player)
    {
        Character2DController playerController = player.GetComponent<Character2DController>();
        TextMeshPro playerText = player.GetComponentInChildren<TextMeshPro>();
        playerController.SetIsPaused(true);

        blackBars.BlackBars(true);

        bgm.FadeOutBGM();
        audioSource.PlayOneShot(sb.transitionSound);
        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(sb.AltarV4);
        altarText.text = "Do you want power, child?";
        yield return new WaitForSeconds(6);

        altarText.text = "";
        audioSource.PlayOneShot(sb.WitchV1);
        playerText.text = "I didn't become a witch for the cool hats.";
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.WitchV4);
        playerText.text = "Well, not only the cool hats, at least.";
        yield return new WaitForSeconds(5);

        playerText.text = "";
        audioSource.PlayOneShot(sb.AltarV2);
        altarText.text = "Past this point are three portals to three graves.";
        audioSource.PlayOneShot(sb.AltarV3);
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV1);
        altarText.text = "Each grave holds an artifact containing the soul and power of a hero long past.";
        audioSource.PlayOneShot(sb.AltarV5);
        yield return new WaitForSeconds(6);
        audioSource.PlayOneShot(sb.AltarV3);
        altarText.text = "The dead need not this power. It should belong to the living.";
        audioSource.PlayOneShot(sb.AltarV2);
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV4);
        altarText.text = "Bring those souls to this altar and you will claim their power for your own";

        yield return new WaitForSeconds(5);
        altarText.text = "";
        audioSource.PlayOneShot(sb.WitchV2);
        playerText.text = "And there's no chance that the talking altar is going to somehow double-cross me, right?";
        yield return new WaitForSeconds(6);
        playerText.text = "";
        yield return new WaitForSeconds(3);
        audioSource.PlayOneShot(sb.WitchV5);
        playerText.text = "I accept your silent confirmation as fact. To grave robbing!";

        yield return new WaitForSeconds(5);

        blackBars.BlackBars(false);

        bgm.FadeInBGM();
        playerText.text = "";
        playerController.SetIsPaused(false);
        //GetComponent<BoxCollider2D>().enabled = false;
        altarDiagPhase = 2;
        yield return null;
    }

    IEnumerator SecondAltarCutscene(GameObject player)
    {
        Character2DController playerController = player.GetComponent<Character2DController>();
        TextMeshPro playerText = player.GetComponentInChildren<TextMeshPro>();
        playerController.SetIsPaused(true);
        bgm.FadeOutBGM();

        blackBars.BlackBars(true);

        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(sb.AltarV2);
        altarText.text = "You have returned with the power of Simon the Green";
        yield return new WaitForSeconds(6);
        audioSource.PlayOneShot(sb.AltarV5);
        altarText.text = "Place his soul on this altar, child, and we may continue";
        yield return new WaitForSeconds(6);

        blackBars.BlackBars(false);

        bgm.FadeInBGM();
        playerText.text = "";
        playerController.SetIsPaused(false);
        //GetComponent<BoxCollider2D>().enabled = false;
        altarDiagPhase = 3;

        yield return null;
    }

    IEnumerator AlphaSlide (TextMeshPro text)
    {
        Vector4 textColor = text.color;

        text.color = new Vector4(text.color.r, text.color.b, text.color.g, Mathf.Lerp(0, 1, 3));
        yield return null;
    }
}
