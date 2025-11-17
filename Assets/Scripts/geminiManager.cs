using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Diagnostics; // for running Python
using Debug = UnityEngine.Debug;

[RequireComponent(typeof(BillAI))]
public class ARBillCapture : MonoBehaviour
{
    [Header("AR Camera")]
    public Camera arCamera;

    [Header("World-Space UI")]
    public Button captureButton;
    public TextMeshProUGUI explanationText;

    [Header("OCR Settings")]
    public string pythonScriptPath = "EasyOCRScript.py"; // path to your OCR Python script

    private BillAI billAI;

    private void Start()
    {
        billAI = GetComponent<BillAI>();
        if (captureButton != null)
            captureButton.onClick.AddListener(CaptureFrame);
    }

    public void CaptureFrame()
    {
        // Capture AR camera frame
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        arCamera.targetTexture = rt;
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        arCamera.Render();
        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        arCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        // Save image
        string path = Path.Combine(Application.persistentDataPath, "bill.jpg");
        File.WriteAllBytes(path, screenshot.EncodeToJPG());
        Debug.Log("Captured bill image: " + path);

        // Send to EasyOCR and get JSON lines
        string[] lines = RunEasyOCR(path);

        if (lines.Length == 0)
        {
            Debug.LogWarning("No text detected!");
            return;
        }

        // Combine lines into one string for Gemini
        string billText = string.Join(" ", lines);
        Debug.Log("Bill Text: " + billText);

        // Call Gemini
        if (billAI != null)
            billAI.OnBillDetected(billText);
    }

    private string[] RunEasyOCR(string imagePath)
    {
        try
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "python";
            psi.Arguments = $"\"{pythonScriptPath}\" \"{imagePath}\"";
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            Process process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            // Parse JSON output
            var data = JsonUtility.FromJson<BillLines>(output);
            return data.lines ?? new string[0];
        }
        catch (System.Exception e)
        {
            Debug.LogError("EasyOCR Error: " + e.Message);
            return new string[0];
        }
    }

    [System.Serializable]
    private class BillLines
    {
        public string[] lines;
    }

    // Optional: Update TMP text when Gemini responds
    public void ShowExplanation(string text)
    {
        if (explanationText != null)
            explanationText.text = text;
    }
}