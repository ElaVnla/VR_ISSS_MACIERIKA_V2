using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DockingUI : MonoBehaviour
{
    [System.Serializable]
    public class DockingStep
    {
        public TMP_Text statusText;
        public Image progressFill;
        public Button startButton;
    }

    public DockingStep[] steps;

    public float duration = 3f;

    private int currentStep = 0;

    void Start()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            steps[i].progressFill.fillAmount = 0f;

            if (i == 0)
            {
                steps[i].startButton.interactable = true;
                steps[i].statusText.text = "Status: Ready";
            }
            else
            {
                steps[i].startButton.interactable = false;
                steps[i].statusText.text = "Status: Locked";
            }
        }
    }

    public void StartStep(int index)
    {
        if (index != currentStep) return;

        steps[index].startButton.interactable = false;
        steps[index].statusText.text = "Status: Processing...";

        StartCoroutine(ProgressBar(index));
    }

    IEnumerator ProgressBar(int index)
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            steps[index].progressFill.fillAmount = time / duration;
            yield return null;
        }

        steps[index].progressFill.fillAmount = 1f;
        steps[index].progressFill.color = Color.green;
        steps[index].statusText.text = "Status: Complete";

        // unlock next
        currentStep++;

        if (currentStep < steps.Length)
        {
            steps[currentStep].startButton.interactable = true;
            steps[currentStep].statusText.text = "Status: Ready";
        }
    }
}