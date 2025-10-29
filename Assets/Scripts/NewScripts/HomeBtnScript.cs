using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeBtnScript : MonoBehaviour
{
    [SerializeField] private string sceneName = "WelcomeUI";

    // This one you’ll call from Button OnClick
    public void LoadWelcomeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}