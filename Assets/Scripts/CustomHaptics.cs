using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class Cube1Haptics : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public HapticImpulsePlayer RHand;
    public HapticImpulsePlayer LHand;



    [Range(0f, 1f)]
    public float intensity = 0.5f;
    public float duration = 0.2f;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(PlayHaptic);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(PlayHaptic);
    }

    void PlayHaptic(SelectEnterEventArgs args)
    {
        var interactorTransform = args.interactorObject.transform;

        if (interactorTransform == null)
            return;

        string interactorName = interactorTransform.name.ToLower();

        if (interactorName.Contains("left"))
        {
            LHand?.SendHapticImpulse(intensity, duration);
        }
        else if (interactorName.Contains("right"))
        {
            RHand?.SendHapticImpulse(intensity, duration);
        }
    }
}