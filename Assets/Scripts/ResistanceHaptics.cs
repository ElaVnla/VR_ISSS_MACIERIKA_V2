using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class StateHaptics : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;

    public HapticImpulsePlayer RHand;
    public HapticImpulsePlayer LHand;

    [Header("Haptics")]
    [Range(0f, 1f)] public float intensity = 1f;
    [Range(0.01f, 0.2f)] public float duration = 0.05f;
    [Range(0f, 1f)] public float frequency = 0f;
    public float interval = 0.05f;

    private Coroutine currentHapticsRoutine;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        if (grabInteractable == null)
            return;

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnDisable()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }

        StopCurrentHaptics();
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        if (args.interactorObject == null)
            return;

        string interactorName = args.interactorObject.transform.name.ToLower();

        StopCurrentHaptics();

        if (interactorName.Contains("left") && LHand != null)
        {
            currentHapticsRoutine = StartCoroutine(HapticsLoop(LHand));
        }
        else if (interactorName.Contains("right") && RHand != null)
        {
            currentHapticsRoutine = StartCoroutine(HapticsLoop(RHand));
        }
    }

    void OnReleased(SelectExitEventArgs args)
    {
        StopCurrentHaptics();
    }

    IEnumerator HapticsLoop(HapticImpulsePlayer hand)
    {
        while (true)
        {
            hand.SendHapticImpulse(intensity, duration, frequency);
            yield return new WaitForSeconds(interval);
        }
    }

    void StopCurrentHaptics()
    {
        if (currentHapticsRoutine != null)
        {
            StopCoroutine(currentHapticsRoutine);
            currentHapticsRoutine = null;
        }
    }
}