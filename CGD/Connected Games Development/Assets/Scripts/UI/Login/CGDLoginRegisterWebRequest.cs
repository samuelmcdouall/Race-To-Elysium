using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

public class CGDLoginRegisterWebRequest : MonoBehaviour
{
    public InputField UsernameField;
    public InputField PasswordField;
    public InputField GuestField;
    public AudioClip ClickSFX;
    public Text ErrorBox;
    GameObject _audioListenerPosition;

    void Start()
    {    
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _audioListenerPosition = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void OnLoginButtonClicked()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        StartCoroutine(Login(UsernameField.text, PasswordField.text));
    }
    public void OnRegisterButtonClicked()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        StartCoroutine(RegisterAndLogin(UsernameField.text, PasswordField.text));
    }
    public void OnPlayAsGuestButtonClicked()
    {
        AudioSource.PlayClipAtPoint(ClickSFX, _audioListenerPosition.transform.position, CGDGameSettings.SoundVolume);
        CGDGameSettings.Username = GuestField.text;
        SceneManager.LoadScene("MainMenuScene");
    }
    public void OnQuitButtonClicked()
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
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
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
    IEnumerator RegisterAndLogin(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("registerusername", username);
        form.AddField("registerpassword", EncryptPassword(password));

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/CGDPHP/Register.php", form))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
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
