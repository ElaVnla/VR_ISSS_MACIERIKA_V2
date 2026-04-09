using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class ProximityTwo : MonoBehaviour
{
    public Transform targetObject;
    public Transform targetObject2;
    public HapticImpulsePlayer RHand;
    public HapticImpulsePlayer LHand;

    public float range = 1f;

    [Range(0f, 1f)] public float minIntensity = 0.1f;
    [Range(0f, 1f)] public float maxIntensity = 1f;

    [Range(0.01f, 0.2f)] public float duration = 0.05f;

    public float minPulseInterval = 0.08f;
    public float maxPulseInterval = 0.9f;

    private float timer = 0f;

    void Awake()
    {
        if (RHand == null)
            RHand = GetComponent<HapticImpulsePlayer>();
        if (LHand == null)
            LHand = GetComponent<HapticImpulsePlayer>();
    }

    void Update()
    {
        if (targetObject == null || targetObject2 == null || RHand == null || LHand == null)
            return;

        float distance = Vector3.Distance(targetObject.position, targetObject2.position);
        if (distance < 1f)
        {
            timer = 0f;
            return;
        }
        if (distance < range)
        {
            float t = 1f - Mathf.InverseLerp(0f, range, distance);

            float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
            float pulseInterval = Mathf.Lerp(maxPulseInterval, minPulseInterval, t);

            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                RHand.SendHapticImpulse(intensity, duration);
                LHand.SendHapticImpulse(intensity, duration);
                timer = pulseInterval;
            }
        }
        else
        {
            timer = 0f;
        }



    }
}