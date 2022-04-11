using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CGDWelcomeBackText : MonoBehaviour
{
    Text _textBox;
    // Start is called before the first frame update
    void Start()
    {
        _textBox = GetComponent<Text>();
        if (CGDGameSettings.Username == "Guest")
        {
            _textBox.text = "Welcome back!";
        }
        else
        {
            _textBox.text = "Welcome back " + CGDGameSettings.Username + " !";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
