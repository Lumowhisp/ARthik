using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class ApiManager : MonoBehaviour
{
    public static ApiManager Instance;
    public string apiKey = "731c322c84e2149dbebb57ee"; // Replace with your ExchangeRate-API key

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator Convert(float amount, string from, string to, TMP_Text resultText)
    {
        string url = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{from}";

        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                string json = req.downloadHandler.text;
                float rate = ParseRate(json, to);
                float converted = amount * rate;

                // Extract last update time
                string lastUpdate = ParseLastUpdate(json);

                resultText.text = $"{amount} {from} = {converted:F2} {to}\nLast Updated: {FormatTime(lastUpdate)}";
            }
            else
            {
                resultText.text = "Error fetching data";
                Debug.LogError(req.error);
            }
        }
    }

    private float ParseRate(string json, string to)
    {
        // Manual JSON parse for simplicity
        string key = $"\"{to}\":";
        int start = json.IndexOf(key) + key.Length;
        int end = json.IndexOf(",", start);
        if (end < 0) end = json.IndexOf("}", start);
        string num = json.Substring(start, end - start);
        float.TryParse(num, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float rate);
        return rate;
    }

    private string ParseLastUpdate(string json)
    {
        string key = "\"time_last_update_utc\":\"";
        int start = json.IndexOf(key) + key.Length;
        int end = json.IndexOf("\"", start);
        string dateStr = json.Substring(start, end - start);
        return dateStr;
    }

    private string FormatTime(string utcTime)
    {
        if (System.DateTime.TryParse(utcTime, out var utc))
        {
            var local = utc.ToLocalTime();
            return local.ToString("dd MMM, hh:mm tt");
        }
        return utcTime;
    }
}