using UnityEngine;
using Vuforia;

public class NoteDetector : MonoBehaviour
{
    private ObserverBehaviour observerBehaviour;
    private Convert converterScript;

    void Start()
    {
        // Get reference to Vuforia Image Target (Observer)
        observerBehaviour = GetComponent<ObserverBehaviour>();

        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        // Find your Convert script
        converterScript = FindObjectOfType<Convert>();
    }

    private void OnDestroy()
    {
        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED)
        {
            OnTargetFound();
        }
        else if (status.Status == Status.NO_POSE)
        {
            OnTargetLost();
        }
    }

    void OnTargetFound()
    {
        string targetName = observerBehaviour.TargetName;
        Debug.Log("Detected Target: " + targetName);

        string noteValue = GetNoteValue(targetName);
        if (noteValue != "")
        {
            converterScript.SetDetectedNoteValue(noteValue);
        }
    }

    void OnTargetLost()
    {
        Debug.Log("Target Lost");
    }

    string GetNoteValue(string targetName)
    {
        switch (targetName)
        {
            case "front50":
            case "back50":
                return "50";

            case "front100":
            case "back100":
                return "100";

            case "front200":
            case "back200":
                return "200";

            case "front500":
            case "back500":
                return "500";

            case "front2000":
            case "back2000":
                return "2000";

            default:
                return "";
        }
    }
}