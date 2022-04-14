using UnityEngine;
using UnityEngine.UI;

public class CGDUIBar : MonoBehaviour
{
    public Image FillBar;
    public Slider SliderBar;
    public bool GateHPBar;

    public void SetBar(float value)
    {
        SliderBar.value = value;
        if (GateHPBar)
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
