using UnityEngine;
using Vuforia;
using TMPro;
using UnityEngine.UI;

public class CurrDetector : MonoBehaviour
{
    private ObserverBehaviour observerBehaviour;
    private string targetId;
    private float detectedAmount = 0f;

    public TMP_Text detectedText;        // shows detected INR
    public TMP_InputField currencyInput; // user enters currency code
    public Button convertButton;         // just for testing button
    public TMP_Text resultText;          // can show debug message

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
        if(observerBehaviour != null)
        {
            targetId = observerBehaviour.TargetName;
            observerBehaviour.OnTargetStatusChanged += OnStatusChanged;
        }

        convertButton.onClick.AddListener(OnConvertClicked);
        resultText.text = "";
    }

    private void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if(status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            if(OfflineMapping.NoteToINR.TryGetValue(targetId, out float amount))
            {
                detectedAmount = amount;
                detectedText.text = "Detected: INR " + amount;
            }
            else
            {
                detectedText.text = "Unknown note";
                detectedAmount = 0f;
            }
        }
        else
        {
            detectedText.text = "No note detected";
            detectedAmount = 0f;
        }
    }

    private void OnConvertClicked()
    {
        string targetCurrency = currencyInput.text.ToUpper();

        if(detectedAmount <= 0)
        {
            resultText.text = "Scan a note first!";
            return;
        }

        if(string.IsNullOrEmpty(targetCurrency))
        {
            resultText.text = "Enter currency code!";
            return;
        }

        if(targetCurrency.Length != 3 || !IsAllLetters(targetCurrency))
        {
            resultText.text = "Invalid currency code! Use 3-letter alphabetic code.";
            return;
        }

        StartCoroutine(ApiManager.Instance.Convert(detectedAmount, "INR", targetCurrency, resultText));
    }

    private bool IsAllLetters(string str)
    {
        foreach(char c in str)
        {
            if(!char.IsLetter(c))
                return false;
        }
        return true;
    }

    private void OnDestroy()
    {
        if(observerBehaviour != null)
            observerBehaviour.OnTargetStatusChanged -= OnStatusChanged;
    }
}