using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class Proximity : MonoBehaviour
{
    [Header("Targets")]
    public List<Transform> targetObjects = new List<Transform>();

    [Header("Left Hand")]
    public Transform leftControllerTransform;
    public HapticImpulsePlayer leftHaptics;

    [Header("Right Hand")]
    public Transform rightControllerTransform;
    public HapticImpulsePlayer rightHaptics;

    [Header("Settings")]
    public float range = 0.01f;

    [Range(0f, 1f)] public float minIntensity = 0.1f;
    [Range(0f, 1f)] public float maxIntensity = 0.7f;

    public float pulseDuration = 0.08f;

    private float leftTimer = 0f;
    private float rightTimer = 0f;

    void Update()
    {
        HandleHand(leftControllerTransform, leftHaptics, ref leftTimer);
        HandleHand(rightControllerTransform, rightHaptics, ref rightTimer);
    }

    void HandleHand(Transform controllerTransform, HapticImpulsePlayer hapticPlayer, ref float timer)
    {
        if (controllerTransform == null || hapticPlayer == null || targetObjects.Count == 0)
            return;

        Transform nearestTarget = GetNearestTarget(controllerTransform.position);

        if (nearestTarget == null)
            return;

        float distance = Vector3.Distance(controllerTransform.position, nearestTarget.position);

        if (distance < range)
        {
            float t = 1f - Mathf.InverseLerp(0f, range, distance);
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
            float pulseInterval = Mathf.Lerp(0.9f, 0.08f, t);

            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                hapticPlayer.SendHapticImpulse(intensity, pulseDuration);
                timer = pulseInterval;
            }
        }
        else
        {
            timer = 0f;
        }
    }

    Transform GetNearestTarget(Vector3 handPosition)
    {
        Transform nearest = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Transform target in targetObjects)
        {
            if (target == null) continue;

            float distance = Vector3.Distance(handPosition, target.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = target;
            }
        }

        return nearest;
    }
}