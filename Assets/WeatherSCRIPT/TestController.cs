using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;

public class TestController : MonoBehaviour
{
    [ContextMenu("Test Get")]
    public async void TestGet()
    {
        var url = "https://api.agify.io/?name=John";
        using var API_URL = UnityWebRequest.Get(url);
        API_URL.SetRequestHeader("Content-Type", "application/json");
        var SendRequest = API_URL.SendWebRequest();
        while (!SendRequest.isDone)
            await Task.Yield();
        var jsonresponse = API_URL.downloadHandler.text;
        // if (API_URL.result == UnityWebRequest.Result.Success)
        // {
        //     Debug.Log($"Success:{API_URL.downloadHandler.text}");
        // }
        // else
        // {
        //     Debug.Log($"Failed:{API_URL.error}");
        // }
        if (API_URL.result != UnityWebRequest.Result.Success)
            Debug.LogError($"Failed:{API_URL.error}");
        try
        {
            var result = JsonConvert.DeserializeObject<User>(jsonresponse);
            Debug.Log($"Success:{API_URL.downloadHandler.text}");

        }
        catch (Exception ex)
        {
            Debug.LogError($"Could not parse response {jsonresponse}.{ex.Message}");
            
        }

    }
}
