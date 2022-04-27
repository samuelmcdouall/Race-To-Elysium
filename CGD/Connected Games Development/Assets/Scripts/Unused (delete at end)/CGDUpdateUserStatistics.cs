using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CGDUpdateUserStatistics : MonoBehaviour
{
    // may not use this, currently done in the player class
    //public static void UpdateStats(string username, bool won, int silver)
    //{
    //    StartCoroutine(UpdateStatsOfDatabase(username, won, silver));
    //}

    //IEnumerator UpdateStats(string username, bool won, int silver)
    //{
    //    WWWForm form = new WWWForm();
    //    form.AddField("usernameupdatestats", username);
    //    form.AddField("wonupdatestats", won.ToString());
    //    form.AddField("silverupdatestats", silver.ToString());

    //    using (UnityWebRequest webRequest = UnityWebRequest.Post("http://localhost/CGDPHP/UpdateStats.php", form))
    //    {
    //        yield return webRequest.Send();
    //        if (webRequest.isNetworkError || webRequest.isHttpError)
    //        {
    //            Debug.Log(webRequest.error);
    //        }
    //        else
    //        {
    //            string jsonData = webRequest.downloadHandler.text;
    //            print("updated stats sent off to db " + jsonData);
    //        }
    //    }
    //}
}
