using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uilimiter : MonoBehaviour
{
   public Slider slider;
   public Slider slider2;
   public Slider slider3;

    private void Update()
    {
        if (NetworkGameManager.Instance.GameStarted)
        {
            slider.enabled = true;
            slider2.enabled = true;
            slider3.enabled = true;
        }
        else
        {
           slider.enabled = false;
            slider2.enabled = false;
            slider3.enabled = false;
        }
    }

}
