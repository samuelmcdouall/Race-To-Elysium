using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Newtonsoft.Json;

public class CGDMainMenuUserStatistics : MonoBehaviour
{
    Text _stats;
    void Start()
    {
        _stats = GetComponent<Text>();
        if (CGDGameSettings.PlayingAsGuest)
        {
            _stats.text = "";
        }
        else
        {
            StartCoroutine(GetStats(CGDGameSettings.Username));
        }
    }

    IEnumerator GetStats(string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("usernamegetstats", username);

        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/CGDPHP/GetStats.php", form))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                string jsonData = webRequest.downloadHandler.text;
                // Input comes in with square brackets around it, so just remove them to allow the Json converter to read the data
                jsonData = jsonData.Remove(jsonData.Length - 1, 1);
                jsonData = jsonData.Remove(0, 1);
                CGDUserStatistics userStats = JsonConvert.DeserializeObject<CGDUserStatistics>(jsonData);
                string winRate;
                if (userStats.Losses == 0)
                {
                    if (userStats.Wins == 0)
                    {
                        winRate = "-";
                    }
                    else
                    {
                        winRate = "100%";
                    }
                }
                else
                {
                    float winRateFloat = ((float)userStats.Wins / ((float)userStats.Losses + (float)userStats.Wins)) * 100.0f;
                    winRate = winRateFloat.ToString("F2") + "%";
                }
                _stats.text = "Silver: " + userStats.Silver + "\n"
                            + "Wins: " + userStats.Wins + "\n"
                            + "Losses: " + userStats.Losses + "\n"
                            + "Win Rate: " + winRate;
            }
        }
    }
}
