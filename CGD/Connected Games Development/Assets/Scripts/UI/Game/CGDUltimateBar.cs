using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGDUltimateBar : MonoBehaviour // todo rename to generic bar
{
    public Image FillBar;
    public Slider SliderBar;
    public bool HpBar;

    public void SetBar(float value)
    {
        SliderBar.value = value;
        if (HpBar)
        {
            FillBar.color = Color.Lerp(Color.red, Color.green, SliderBar.value / SliderBar.maxValue);
        }
        else
        {
            if (value == 100.0f)
            {
                FillBar.color = Color.green;
            }
            else
            {
                FillBar.color = Color.red;
            }
        }
    }

}
