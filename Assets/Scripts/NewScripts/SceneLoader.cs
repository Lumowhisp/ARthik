using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadARthik()
    {
        SceneManager.LoadScene("ARthik");
    }

    public void LoadCard()
    {
        SceneManager.LoadScene("Card");
    }
}