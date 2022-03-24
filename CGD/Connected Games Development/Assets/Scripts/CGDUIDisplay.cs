using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGDUIDisplay : MonoBehaviour
{
    public void DisplayUI(float duration)
    {
        gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Invoke("HideUI", duration);
    }

    void HideUI()
    {
        gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
}
