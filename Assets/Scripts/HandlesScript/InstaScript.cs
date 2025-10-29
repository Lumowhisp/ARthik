using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenInstagram()
    {
        Application.OpenURL("https://www.instagram.com/geek_aditya/");
    }
    public void OpenLinkedin()
    {
        Application.OpenURL("https://www.linkedin.com/in/nerdyaditya/");
    }
    public void OpenGmail()
    {
        Application.OpenURL("mailto:arthik.project21@gmail.com?subject=Hello %20Team ARthik&body=I%20want%20to%20connect%20with%20you!");
    }
    public void OpenPhone()
    {
        #if UNITY_ANDROID || UNITY_IOS
    Application.OpenURL("tel:+917459971978");
#else
    Debug.Log("Phone calls only work on mobile!");
#endif
    }
}
