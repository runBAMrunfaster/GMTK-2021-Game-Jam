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

    public void PlayHelmCutscene(GameObject helmet)
    {
        int sceneNum = helmet.GetComponent<PickupFirstHelmet>().GetHelmetID();
        switch(sceneNum)
        {
            case 0:
                StartCoroutine("GreenHelmetPlacement");

                break;

            case 1:

                break;

            case 2:

                break;
        }
    }

    IEnumerator GreenHelmetPlacement()
    {
        Character2DController playerController = player.GetComponent<Character2DController>();
        TextMeshPro playerText = player.GetComponentInChildren<TextMeshPro>();
        playerController.SetIsPaused(true);


        blackBars.BlackBars(true);

        yield return new WaitForSeconds(2);
        altarText.text = "Simon the Green was fleet of foot, but he could not leap away from his demise.";
        yield return new WaitForSeconds(5);
        altarText.text = "Although he fulfilled his duty as a hero, he fell into the inky quagmire all the same";
        yield return new WaitForSeconds(5);
        altarText.text = "A shame he could not jump a third time.";

        yield return new WaitForSeconds(5);
        altarText.text = "";
        playerText.text = "Harsh.";

        yield return new WaitForSeconds(5);
        altarText.text = "Simon's power is now yours and yours alone. Go forth and collect the others.";

        yield return new WaitForSeconds(5);
        altarText.text = "";

        blackBars.BlackBars(false);
        playerController.SetIsPaused(false);
        //GetComponent<BoxCollider2D>().enabled = false;


        yield return null;
    }

    public void AcceptHelmet(GameObject helmet)
    {
        int helmetID = helmet.GetComponent<PickupFirstHelmet>().GetHelmetID();

        switch(helmetID)
        {
            case 0:
                helmet.transform.parent = greenAltarSpot.transform;
                helmet.transform.localPosition = Vector3.zero;
                helmet.GetComponent<BoxCollider2D>().enabled = false;
                break;

            case 1:

                break;

            case 2:

                break;

        }


    }
    
}
