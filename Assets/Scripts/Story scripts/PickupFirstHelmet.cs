using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickupFirstHelmet : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Color altarColor;
    bool firstsceneHasPlayed = false;
    bool secondsceneHasPlayed = false;
    bool thirdsceneHasPlayed = false;
    [SerializeField] int helmetID = 0;

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

                break;

            case 2:

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

        playerText.color = altarColor;

        playerText.text = "Simon the Green was a fleet-footed hero";
        yield return new WaitForSeconds(5);
        playerText.text = "It was said that after leaping into the air, he could leap exactly one additional time.";
        yield return new WaitForSeconds(5);
        playerText.text = "You will need this power, as the soul of a hero is a weighty thing.";
        yield return new WaitForSeconds(5);
        playerText.text = "It is unlikely you will be able to fly as you have become accustomed to while carrying a hero's soul.";
        yield return new WaitForSeconds(5);

        playerText.color = playerTextColor;

        playerText.text = "I'm sure there's a moral lesson in this. Guess I'm stuck taking the long way home!";
        yield return new WaitForSeconds(5);

        playerText.text = "";
        playerController.SetIsPaused(false);
        player.GetComponent<Character2DController>().SetJumpCounterMax(2);
        firstsceneHasPlayed = true;


        yield return null;
    }
}
