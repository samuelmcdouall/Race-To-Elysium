using UnityEngine;
using UnityEngine.UI;

public class CGDWelcomeBackText : MonoBehaviour
{
    Text _textBox;

    void Start()
    {
        _textBox = GetComponent<Text>();
        if (CGDGameSettings.PlayingAsGuest)
        {
            _textBox.text = "Welcome " + CGDGameSettings.Username + "!";
        }
        else
        {
            _textBox.text = "Welcome back " + CGDGameSettings.Username + "!";
        }
    }
}
