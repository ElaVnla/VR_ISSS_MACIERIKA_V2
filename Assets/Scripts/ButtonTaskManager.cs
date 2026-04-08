using UnityEngine;

public class ButtonTaskManager : MonoBehaviour
{
    public bool button1Pressed = false;
    public bool button2Pressed = false;

    public bool AreBothButtonsPressed()
    {
        return button1Pressed && button2Pressed;
    }

    public void PressButton1()
    {
        button1Pressed = true;
        Debug.Log("Button 1 pressed");
    }

    public void PressButton2()
    {
        button2Pressed = true;
        Debug.Log("Button 2 pressed");
    }
}