using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupFirstHelmet : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Color altarColor;
    [SerializeField] Color evilColor;
    bool firstsceneHasPlayed = false;
    bool secondsceneHasPlayed = false;
    bool thirdsceneHasPlayed = false;
    bool fourthsceneHasPlayed = false;
    [SerializeField] int helmetID = 0;

    [SerializeField] BlackBarController blackBars;


    [SerializeField] AudioSource audioSource;
    [SerializeField] CameraFollow bgm;
    [SerializeField] SoundBank sb;




    // Start is called before the first frame update
    void Start()
    {
       
    }

    public int GetHelmetID()
    {
        return helmetID;
    }

    public void PlayCutscene(int scene)
    {
        switch(scene)
        {
            case 0:
                if (!firstsceneHasPlayed)
                {
                    StartCoroutine("FirstHelmetScene");
                }
                break;

            case 1:
                if (!secondsceneHasPlayed)
                {
                    StartCoroutine("SecondHelmetScene");
                }
                break;

            case 2:
                if (!thirdsceneHasPlayed)
                {
                    StartCoroutine("ThirdHelmetScene");
                }
                break;
            case 3:
                if (!fourthsceneHasPlayed)
                {
                    StartCoroutine("FourthHelmetScene");
                }
                break;
        }
    }

    public 

    IEnumerator FirstHelmetScene()
    {
        Character2DController playerController = player.GetComponent<Character2DController>();
        TextMeshPro playerText = player.GetComponentInChildren<TextMeshPro>();
        Color playerTextColor = playerText.color;
        playerController.SetIsPaused(true);
        bgm.FadeOutBGM();
        blackBars.BlackBars(true);


        playerText.color = altarColor;

        audioSource.PlayOneShot(sb.transitionSound);
        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(sb.AltarV1);
        playerText.text = "Simon the Green was a fleet-footed hero";
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV2);
        playerText.text = "It was said that after leaping into the air, he could leap exactly one additional time.";
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV3);
        playerText.text = "You will need this power, as the soul of a hero is a weighty thing.";
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV4);
        playerText.text = "It is unlikely you will be able to fly as you have become accustomed to while carrying a hero's soul.";
        yield return new WaitForSeconds(5);
        playerText.text = "";
        yield return new WaitForSeconds(2);

        playerText.color = playerTextColor;

        audioSource.PlayOneShot(sb.WitchV3);
        playerText.text = "I'm sure there's a moral lesson in this. Guess I'm stuck taking the long way home!";
        yield return new WaitForSeconds(5);
        playerText.text = "";

        blackBars.BlackBars(false);
        bgm.FadeInBGM();
        
        playerController.SetIsPaused(false);
        player.GetComponent<Character2DController>().SetJumpCounterMax(2);
        firstsceneHasPlayed = true;


        yield return null;
    }

    IEnumerator SecondHelmetScene()
    {
        Character2DController playerController = player.GetComponent<Character2DController>();
        TextMeshPro playerText = player.GetComponentInChildren<TextMeshPro>();
        Color playerTextColor = playerText.color;
        playerController.SetIsPaused(true);
        bgm.FadeOutBGM();
        blackBars.BlackBars(true);

        playerText.color = altarColor;

        audioSource.PlayOneShot(sb.transitionSound);
        yield return new WaitForSeconds(2);

        audioSource.PlayOneShot(sb.AltarV1);
        playerText.text = "Cecil the Blue was a knight known to interweave his swordplay with magicks.";
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV2);
        playerText.text = "By chanting spells even as he swung his blade, he could teleport short distances to his enemy's blindspots.";
        yield return new WaitForSeconds(6);
        playerText.text = "";
        yield return new WaitForSeconds(2);

        playerText.color = playerTextColor;
        audioSource.PlayOneShot(sb.WitchV1);
        playerText.text = "That sounds like a great way to teleport yourself into a wall.";
        yield return new WaitForSeconds(5);
        playerText.color = altarColor;
        playerText.text = "Press shift to blink. Blinking will fail if you try to blink inside of an object.";
        playerText.color = playerTextColor;
        yield return new WaitForSeconds(7);


        blackBars.BlackBars(false);
        bgm.FadeInBGM();
        playerText.text = "";
        playerController.SetIsPaused(false);
        playerController.SetBlink(true);
        secondsceneHasPlayed = true;


        yield return null;
    }

    IEnumerator ThirdHelmetScene()
    {
        Character2DController playerController = player.GetComponent<Character2DController>();
        TextMeshPro playerText = player.GetComponentInChildren<TextMeshPro>();
        Color playerTextColor = playerText.color;
        playerController.SetIsPaused(true);
        bgm.FadeOutBGM();
        blackBars.BlackBars(true);

        playerText.color = altarColor;

        audioSource.PlayOneShot(sb.transitionSound);
        yield return new WaitForSeconds(2);
        audioSource.PlayOneShot(sb.AltarV1);
        playerText.text = "Alina the Gold was a hero whose talents were simpler, yet all the more powerful for it.";
        yield return new WaitForSeconds(5);
        audioSource.PlayOneShot(sb.AltarV2);
        playerText.text = "She could not walk on air or warp through time, but by gathering her power she could effect a single, great leap far beyond the capabilities of a mortal man.";
        yield return new WaitForSeconds(7);
        audioSource.PlayOneShot(sb.AltarV3);
        playerText.text = "This power, too, now is yours. Feel the power build....and release!";
        yield return new WaitForSeconds(5);
        playerText.color = altarColor;
        playerText.text = "Hold space to charge a super jump. Release to jump higher than normal.";
        playerText.color = playerTextColor;
        yield return new WaitForSeconds(7);

        blackBars.BlackBars(false);
        bgm.FadeInBGM();
        playerText.color = playerTextColor;
        playerText.text = "";
        playerController.SetIsPaused(false);
        playerController.SetSuperJump(true);
        secondsceneHasPlayed = true;

        yield return null;
    }

    IEnumerator FourthHelmetScene()
    {
        Character2DController playerController = player.GetComponent<Character2DController>();
        TextMeshPro playerText = player.GetComponentInChildren<TextMeshPro>();
        Color playerTextColor = playerText.color;
        playerController.SetIsPaused(true);
        blackBars.BlackBars(true);

        playerText.color = evilColor;
        audioSource.PlayOneShot(sb.AltarV2);
        playerText.text = "Wise you were to foresee this outcome, but foolish to press forward.";
        yield return new WaitForSeconds(5);
        playerText.color = playerTextColor;
        audioSource.PlayOneShot(sb.WitchV5);
        playerText.text = "Rats. Pulled forward by the invisible hand of fate, once again.";
        yield return new WaitForSeconds(5);

        playerText.color = evilColor;
        audioSource.PlayOneShot(sb.AltarV1);
        playerText.text = "With a powerful witch such as yourself as my host, I will once again cover the land in darkness.";
        yield return new WaitForSeconds(5);

        playerText.color = playerTextColor;
        audioSource.PlayOneShot(sb.WitchV5);
        playerText.text = "We'll see about that. I happen to know a thing or two about evil-entity-curse-removal procedues, Mr. Pinkface.";
        yield return new WaitForSeconds(5);

        blackBars.BlackBars(false);
        bgm.FadeInBGM();
        playerText.color = playerTextColor;
        playerText.text = "";
        playerController.SetIsPaused(false);
        playerController.SetSuperJump(true);
        fourthsceneHasPlayed = true;

        yield return null;
    }
}
