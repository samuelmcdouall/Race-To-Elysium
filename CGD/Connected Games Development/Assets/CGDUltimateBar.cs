using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGDUltimateBar : MonoBehaviour
{
    public Image FillUltBar;
    public Slider SliderUltBar;

    public void SetUltBar(float current_ult_charge)
    {
        SliderUltBar.value = current_ult_charge;
        if (current_ult_charge == 100.0f)
        {
            FillUltBar.color = Color.green;
        }
        else
        {
            FillUltBar.color = Color.red;
        }
    }

}
