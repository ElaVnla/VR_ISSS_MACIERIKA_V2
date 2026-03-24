using UnityEngine;

public class DebrisCollider : MonoBehaviour
{
    public GameObject loseUI;

    private bool hasTriggered = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasTriggered) return;

        if (collision.gameObject.CompareTag("GrabWalls"))
        {
            hasTriggered = true;

            Debug.Log("CUBE HIT WALL!");

            TriggerLose();
        }
    }

    private void TriggerLose()
    {
        Debug.Log("TriggerLose called");

        if (loseUI != null)
        {
            loseUI.SetActive(true);
            Debug.Log("Lose UI activated: " + loseUI.name);
            Time.timeScale = 0f;
        }
        else
        {
            Debug.Log("loseUI is NULL");
        }
    }
}
