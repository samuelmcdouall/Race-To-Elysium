using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CGDLoginRegisterWebRequest : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField UsernameField;
    public InputField PasswordField;
    public InputField GuestField;
    public AudioClip ClickSFX;
    public Text ErrorBox;
    GameObject _audioListenerPosition;
    void Start()
    {
        //string password = "bleh bleh";
        //string encrypyedPassword = EncryptPassword(password);
        //StartCoroutine(Login("testuser6", encrypyedPassword));        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _audioListenerPosition = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void LoginButtonClicked()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        StartCoroutine(Login(UsernameField.text, PasswordField.text));
    }
    public void RegisterButtonClicked()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        StartCoroutine(Register(UsernameField.text, PasswordField.text));
    }
    public void PlayAsGuestButtonClicked()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        CGDGameSettings.Username = GuestField.text;
        SceneManager.LoadScene("MainMenuScene");
    }
    public void QuitButtonClicked()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        Application.Quit();
    }

    IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginusername", username);
        form.AddField("loginpassword", EncryptPassword(password));

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/CGDPHP/Login.php", form))
        {
            yield return webRequest.Send();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
                ErrorBox.text = "Failed to connect. Try playing as guest";
            }
            else
            {
                string returnText = webRequest.downloadHandler.text;
                Debug.Log(returnText);
                if (returnText.Contains("(LS)"))
                {
                    CGDGameSettings.Username = username;
                    CGDGameSettings.PlayingAsGuest = false;
                    SceneManager.LoadScene("MainMenuScene");
                }
                else if (returnText.Contains("(WC)"))
                {
                    ErrorBox.text = "Wrong credentials entered";
                }
                else if (returnText.Contains("(UDNE)"))
                {
                    ErrorBox.text = "Username does not exist";
                }
                else
                {
                    ErrorBox.text = "Error logging in. Please try again";
                }
            }
        }
    }
    IEnumerator Register(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("registerusername", username);
        form.AddField("registerpassword", EncryptPassword(password));

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/CGDPHP/Register.php", form))
        {
            yield return webRequest.Send();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
                ErrorBox.text = "Failed to connect. Try playing as guest";
            }
            else
            {
                string returnText = webRequest.downloadHandler.text;
                Debug.Log(returnText);
                if (returnText.Contains("(NUCS)"))
                {
                    CGDGameSettings.Username = username;
                    CGDGameSettings.PlayingAsGuest = false;
                    SceneManager.LoadScene("MainMenuScene");
                }
                else if (returnText.Contains("(UAT)"))
                {
                    ErrorBox.text = "Username is already taken";
                }
                else
                {
                    ErrorBox.text = "Error creating account. Please try again";
                }
            }
        }
    }

    string EncryptPassword(string plainTextPassword)
    {
        SHA256 hash = SHA256.Create();
        byte[] hashedByteArray = hash.ComputeHash(Encoding.UTF8.GetBytes(plainTextPassword));
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < hashedByteArray.Length; i++)
        {
            stringBuilder.Append(hashedByteArray[i].ToString("x2"));
        }
        return stringBuilder.ToString();
    }
}
