using UnityEngine;

public class CGDGameSettings : MonoBehaviour
{
    public static CGDGameSettings Instance;
    public static int CharacterNum = 1;
    public static int PlayerNum = -1;
    public static float MouseSensitivity = -1.0f;
    public static float MusicVolume = -1.0f;
    public static float SoundVolume = -1.0f;
    public static bool PlayingAsGuest = true;
    public static string Username = "Guest";
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        MouseSensitivity = PlayerPrefs.GetFloat("Sensitivity", 10.5f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        SoundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
        CharacterNum = Random.Range(1, 5);
    }

    void Update()
    {
        //todo probably disable this
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            CharacterNum = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CharacterNum = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CharacterNum = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CharacterNum = 4;
        }
    }
}
