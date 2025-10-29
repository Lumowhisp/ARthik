using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class TouchForwarderNew : MonoBehaviour
{
    private void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = touchPosition
            };

            Debug.Log("Touch detected at: " + touchPosition);
        }
    }
    
}