using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ForceKeyboardNew : MonoBehaviour
{
    public TMP_InputField inputField;
    private TouchScreenKeyboard keyboard;

    void Update()
    {
        // Touchscreen.current is part of the New Input System
        if (Touchscreen.current != null && inputField.isFocused)
        {
            // If keyboard not already visible, open it
            if (keyboard == null || !keyboard.active)
            {
                keyboard = TouchScreenKeyboard.Open(inputField.text, TouchScreenKeyboardType.Default);
            }

            // Update text as user types
            if (keyboard != null && inputField.text != keyboard.text)
            {
                inputField.text = keyboard.text;
            }
        }
    }
}