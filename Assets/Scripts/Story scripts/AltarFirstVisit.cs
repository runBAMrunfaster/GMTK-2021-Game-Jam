using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AltarFirstVisit : MonoBehaviour
{
    [SerializeField] int altarDiagPhase = 1;
    //[SerializeField] GameObject altar;
    [SerializeField] TextMeshPro altarText;


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

        yield return new WaitForSeconds(2);
        altarText.text = "Do you want power, child?";
        yield return new WaitForSeconds(6);

        altarText.text = "";
        playerText.text = "I didn't become a witch for the cool hats.";
        yield return new WaitForSeconds(5);
        playerText.text = "Well, not only the cool hats, at least.";
        yield return new WaitForSeconds(5);

        playerText.text = "";
        altarText.text = "Past this point are three portals to three graves.";
        yield return new WaitForSeconds(5);
        altarText.text = "Each grave holds an artifact containing the soul and power of a hero long past.";
        yield return new WaitForSeconds(5);
        altarText.text = "The dead need not this power. It should belong to the living.";
        yield return new WaitForSeconds(5);
        altarText.text = "Bring those souls to this altar and you will claim their power for your own";

        yield return new WaitForSeconds(5);
        altarText.text = "";
        playerText.text = "And there's no chance that the talking altar is going to somehow double-cross me, right?";
        yield return new WaitForSeconds(3);
        playerText.text = "";
        yield return new WaitForSeconds(5);
        playerText.text = "I accept your silent confirmation as fact. To grave robbing!";

        yield return new WaitForSeconds(5);


        playerText.text = "";
        playerController.SetIsPaused(false);
        GetComponent<BoxCollider2D>().enabled = false;
        yield return null;
    }

    IEnumerator AlphaSlide (TextMeshPro text)
    {
        Vector4 textColor = text.color;

        text.color = new Vector4(text.color.r, text.color.b, text.color.g, Mathf.Lerp(0, 1, 3));
        yield return null;
    }
}
