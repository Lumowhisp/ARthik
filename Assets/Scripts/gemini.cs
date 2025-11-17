using UnityEngine;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class BillAI : MonoBehaviour
{
    private static readonly HttpClient client = new HttpClient();

    [Header("Set your Gemini key here")]
    [SerializeField] private string apiKey = "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"; // replace with your Gemini key

    private string model = "gemini-2.5-flash"; // choose model

    // Call this method when your bill is detected
    public async void OnBillDetected(string billText)
    {
        string prompt = $"Explain this bill in simple terms:\n{billText}";
        string response = await SendRequest(prompt);
        if (!string.IsNullOrEmpty(response))
        {
            Debug.Log("AI Response: " + response);
        }
    }

    private async Task<string> SendRequest(string prompt)
    {
        client.DefaultRequestHeaders.Clear();

        string url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateText?key={apiKey}";

        string json = $"{{\"prompt\": \"{prompt}\", \"temperature\": 0.7}}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync(url, content);
            string result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var parsed = JsonUtility.FromJson<GeminiResponse>(result);
                if (parsed != null && parsed.candidates != null && parsed.candidates.Length > 0 && !string.IsNullOrEmpty(parsed.candidates[0].content))
                    return parsed.candidates[0].content;
                else
                    return result; // fallback raw JSON
            }
            else
            {
                Debug.LogError($"Error {response.StatusCode}: {result}");
                return null;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Exception: " + e.Message);
            return null;
        }
    }

    // Helper classes for Gemini JSON parsing
    [System.Serializable]
    private class GeminiResponse
    {
        public Candidate[] candidates;
    }

    [System.Serializable]
    private class Candidate
    {
        public string content;
    }
}