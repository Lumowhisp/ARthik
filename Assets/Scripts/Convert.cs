using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Net.Security;
using System.Security.Authentication;
using System.Net;
using Newtonsoft.Json;

public class Convert : MonoBehaviour
{
    public TMP_Dropdown currencySelector;
    public Button convertButton;
    public GameObject popupPanel;
    public TMP_Text resultText;

    private string detectedNote;

    void Start()
    {
        // Force TLS 1.2 for secure requests
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        convertButton.onClick.AddListener(OnConvertClick);
        popupPanel.SetActive(false);
        resultText.text = "";

        StartCoroutine(LoadCurrencies()); // Load currency list when app starts
    }

    IEnumerator LoadCurrencies()
    {
        string url = "https://api.exchangerate.host/symbols"; // gives all available currencies
        int maxRetries = 3;
        int attempt = 0;
        bool success = false;

        while (attempt < maxRetries && !success)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);

            // Workaround for SSL/TLS issues on some platforms
            request.certificateHandler = new AcceptAllCertificates();

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"Attempt {attempt + 1} failed to load currencies: {request.error}");
                attempt++;
                yield return new WaitForSeconds(1f); // wait before retry
            }
            else
            {
                string json = request.downloadHandler.text;
                try
                {
                    // Use Newtonsoft.Json to parse JSON properly
                    var apiResponse = JsonConvert.DeserializeObject<SymbolsApiResponse>(json);
                    if (apiResponse == null || apiResponse.symbols == null)
                    {
                        Debug.LogError("Invalid symbols response.");
                        yield break;
                    }

                    // Clear old options and add placeholder and default note options
                    currencySelector.ClearOptions();
                    List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
                    options.Add(new TMP_Dropdown.OptionData("Select Currency"));

                    // Add default note options
                    options.Add(new TMP_Dropdown.OptionData("₹100 - Default Note 100"));
                    options.Add(new TMP_Dropdown.OptionData("₹200 - Default Note 200"));
                    options.Add(new TMP_Dropdown.OptionData("₹500 - Default Note 500"));

                    // Add currencies from API
                    foreach (var kvp in apiResponse.symbols)
                    {
                        options.Add(new TMP_Dropdown.OptionData(kvp.Key + " - " + kvp.Value.description));
                    }

                    currencySelector.AddOptions(options);
                    currencySelector.value = 0;
                    currencySelector.RefreshShownValue();

                    success = true;
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Failed to parse currency list JSON: " + e.Message);
                    yield break;
                }
            }
        }

        if (!success)
        {
            Debug.LogError("Failed to load currencies after multiple attempts.");
            resultText.text = "Failed to load currencies. Please check your internet connection.";
        }
    }

    void OnConvertClick()
    {
        int selectedIndex = currencySelector.value;

        if (selectedIndex == 0)
        {
            popupPanel.SetActive(true);
            resultText.text = "Please select a valid currency.";
            return;
        }

        if (string.IsNullOrEmpty(detectedNote))
        {
            resultText.text = "No detected note value to convert.";
            return;
        }

        // Strip non-numeric characters from detectedNote
        string numericString = System.Text.RegularExpressions.Regex.Replace(detectedNote, @"[^\d.]", "");
        if (string.IsNullOrEmpty(numericString))
        {
            resultText.text = "Invalid detected note value.";
            return;
        }

        if (!float.TryParse(numericString, out float amount))
        {
            resultText.text = "Invalid detected note value.";
            return;
        }

        popupPanel.SetActive(false);
        string selectedOption = currencySelector.options[selectedIndex].text;
        string selectedCurrency = selectedOption.Split(' ')[0]; // Extract currency code (e.g., "USD")

        Debug.Log("Converting to: " + selectedCurrency);

        StartCoroutine(ConvertCurrency(amount, selectedCurrency));
    }

    IEnumerator ConvertCurrency(float amount, string targetCurrency)
    {
        string fromCurrency = "INR";
        string url = $"https://api.exchangerate.host/convert?from={fromCurrency}&to={targetCurrency}&amount={amount}";

        UnityWebRequest request = UnityWebRequest.Get(url);
        request.certificateHandler = new AcceptAllCertificates();

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            resultText.text = "Conversion failed: " + request.error;
        }
        else
        {
            string json = request.downloadHandler.text;
            try
            {
                // Use Newtonsoft.Json to parse JSON
                ConversionResponse response = JsonConvert.DeserializeObject<ConversionResponse>(json);
                if (response == null)
                {
                    resultText.text = "Conversion failed: Invalid response.";
                    yield break;
                }
                resultText.text = $"₹{amount} = {response.result:F2} {targetCurrency}";
            }
            catch (System.Exception e)
            {
                resultText.text = "Conversion failed: " + e.Message;
            }
        }
    }

    [System.Serializable]
    public class ConversionResponse
    {
        public double result;
    }

    [System.Serializable]
    public class SymbolsApiResponse
    {
        public Dictionary<string, CurrencyData> symbols;
    }

    [System.Serializable]
    public class CurrencyData
    {
        public string description;
        public string code;
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }

    public void SetDetectedNoteValue(string noteValue)
    {
        detectedNote = noteValue;
        if (!string.IsNullOrEmpty(detectedNote))
        {
            resultText.text = "Detected ₹" + detectedNote + " — Select currency to convert.";
        }
        else
        {
            resultText.text = "";
        }
    }

    private class AcceptAllCertificates : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            // Accept all certificates
            return true;
        }
    }
}