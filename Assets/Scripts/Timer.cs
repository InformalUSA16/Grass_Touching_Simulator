using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public int secondsLeft = 0;
    public bool takingAway = false;

    private void Start()
    {
        textDisplay.text = "00:" + secondsLeft;
    }
    

    void Update()
    {
        if (takingAway == false && secondsLeft > 0)
        {
            StartCoroutine(TimerTake());
        }
    }


    IEnumerator TimerTake()
    {
        takingAway = true; //starts coroutine
        yield return new WaitForSeconds(1); //waits for a second
        secondsLeft += 1; // adds time by 1
        if (secondsLeft < 10)
        {
            textDisplay.text = "0" + secondsLeft; //if less than 10 adds extra 0 to text
        }
        else
        {
            textDisplay.text = "0" + secondsLeft; //if bigger do not add the extra 0 [fixes bug where it stops at less than 10 seconds]
        }
        takingAway = false;
    }

}
