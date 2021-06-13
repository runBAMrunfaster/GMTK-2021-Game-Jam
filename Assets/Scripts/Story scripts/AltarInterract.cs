using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AltarInterract : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] BlackBarController blackBars;
    [SerializeField] TextMeshPro altarText;

    //Altar spots
    [SerializeField] GameObject greenAltarSpot;
    [SerializeField] GameObject blueAltarSpot;
    [SerializeField] GameObject goldAltarSpot;

    [SerializeField] AudioSource audioSource;
    [SerializeField] CameraFollow bgm;
    [SerializeField] SoundBank sb;

    [SerializeField] bool storyAltar = true;

    public void PlayHelmCutscene(GameObject helmet)
    {
        int sceneNum = helmet.GetComponent<PickupFirstHelmet>().GetHelmetID();
        switch(sceneNum)
        {
            case 0:
                if(storyAltar)
                {
                    StartCoroutine("GreenHelmetPlacement");
                }
                break;

            case 1:
                if (storyAltar)
                {
                    StartCoroutine("BlueHelmetPlacement");
                }
                break;

            case 2:
                if (storyAltar)
                {
                    StartCoroutine("GoldHelmetPlacement");
                }
                break;
        }
    }

    IEnumerator GreenHelmetPlacement()
    {
        Character2DController playerController = player.GetComponent<Character2DController>();
        TextMeshPro playerText = player.GetComponentInChildren<TextMeshPro>();
        playerController.SetIsPaused(true);
        bgm.FadeOutBGM();


        blackBars.BlackBars(true);

        audioSource.PlayOneShot(sb.transitionSound);
        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(sb.AltarV5);
        altarText.text = "Simon the Green was fleet of foot, but he could not leap away from his demise.";
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV4);
        altarText.text = "Although he fulfilled his duty as a hero, he fell into the inky quagmire all the same";
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV3);
        altarText.text = "A shame he could not jump a third time.";

        yield return new WaitForSeconds(5);
        altarText.text = "";
        audioSource.PlayOneShot(sb.WitchClip1);
        playerText.text = "Harsh.";

        yield return new WaitForSeconds(4);
        audioSource.PlayOneShot(sb.AltarV2);
        altarText.text = "Simon's power is now yours and yours alone. Go forth and collect the others.";
        playerText.text = "";

        yield return new WaitForSeconds(5);
        altarText.text = "";

        bgm.FadeInBGM();
        blackBars.BlackBars(false);
        playerController.SetIsPaused(false);
        //GetComponent<BoxCollider2D>().enabled = false;


        yield return null;
    }

    IEnumerator BlueHelmetPlacement()
    {
        Character2DController playerController = player.GetComponent<Character2DController>();
        TextMeshPro playerText = player.GetComponentInChildren<TextMeshPro>();
        playerController.SetIsPaused(true);
        blackBars.BlackBars(true);
        bgm.FadeOutBGM();

        audioSource.PlayOneShot(sb.transitionSound);
        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(sb.AltarV5);
        altarText.text = "Cecil the Blue was known as one of the cleverest knights of all time";
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV4);
        altarText.text = "He used his magicks to always be in his enemies' blindspots.";
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV3);
        altarText.text = "But when your enemy has no blindspots...";
        audioSource.PlayOneShot(sb.WitchV4);
        playerText.text = "Teleported into a wall, right? Totally teleported into a wall.";
        yield return new WaitForSeconds(5);
        playerText.text = "";
        audioSource.PlayOneShot(sb.AltarV2);
        altarText.text = "...it is easy to lose track of your own.";
        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(sb.WitchClip2);
        playerText.text = "Called it.";

        yield return new WaitForSeconds(5);
        playerText.text = "";
        altarText.text = "";

        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(sb.AltarV1);
        altarText.text = "Cecil's power is now yours and yours alone. Go forth and collect the final soul.";
        yield return new WaitForSeconds(5);

        bgm.FadeInBGM();
        playerText.text = "";
        altarText.text = "";
        blackBars.BlackBars(false);
        playerController.SetIsPaused(false);


        yield return null;
    }

    IEnumerator GoldHelmetPlacement()
    {
        Character2DController playerController = player.GetComponent<Character2DController>();
        TextMeshPro playerText = player.GetComponentInChildren<TextMeshPro>();
        playerController.SetIsPaused(true);
        blackBars.BlackBars(true);
        bgm.FadeOutBGM();

        audioSource.PlayOneShot(sb.transitionSound);
        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(sb.AltarV5);
        altarText.text = "Lesser men scoffed at the simplicity of Alina's power";
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV4);
        altarText.text = "She knew all too well the dangers of complex magics and feats of grace by the grim fates of her forebearers";
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV3);
        altarText.text = "Still, Alina's fate was the same. For it does not matter how high one can jump when ther is nowhere to jump to.";
        yield return new WaitForSeconds(5);
        altarText.text = "";
        audioSource.PlayOneShot(sb.WitchV3);
        playerText.text = "Don't super jump indoors. Got it.";
        yield return new WaitForSeconds(5);
        playerText.text = "";
        audioSource.PlayOneShot(sb.AltarV2);
        altarText.text = "Alina's power is yours, and yours alone.";
        yield return new WaitForSeconds(5);
        altarText.text = "";

        bgm.FadeInBGM();
        blackBars.BlackBars(false);
        playerController.SetIsPaused(false);

        yield return null;
    }

        public void AcceptHelmet(GameObject helmet)
        {
        Debug.Log("AcceptHelmet() has been called!");
            int helmetID = helmet.GetComponent<PickupFirstHelmet>().GetHelmetID();

            switch(helmetID)
            {
                case 0:
                Debug.Log("Putting green helmet in green spot!");
                    helmet.transform.parent = greenAltarSpot.transform;
                    helmet.transform.localPosition = Vector3.zero;
                    if(storyAltar)
                    {
                        helmet.GetComponent<BoxCollider2D>().enabled = false;
                    }                
                    break;

                case 1:
                    helmet.transform.parent = blueAltarSpot.transform;
                    helmet.transform.localPosition = Vector3.zero;
                    if (storyAltar)
                    {
                        helmet.GetComponent<BoxCollider2D>().enabled = false;
                    }
                    break;

                case 2:
                    helmet.transform.parent = goldAltarSpot.transform;
                    helmet.transform.localPosition = Vector3.zero;
                    if (storyAltar)
                    {
                        helmet.GetComponent<BoxCollider2D>().enabled = false;
                    }
                    break;

        }


    }
    
}
