using UnityEngine;

public class TempButtonPress : MonoBehaviour
{
    public ButtonTaskManager taskManager;
    public int buttonID;
    private bool alreadyPressed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyPressed) return;

        if (other.CompareTag("Hand") || other.CompareTag("Player"))
        {
            alreadyPressed = true;

            if (buttonID == 1)
                taskManager.PressButton1();
            else if (buttonID == 2)
                taskManager.PressButton2();

            Debug.Log(gameObject.name + " was pressed");
        }
    }
}