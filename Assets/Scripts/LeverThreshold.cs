using UnityEngine;

public class LeverDebug : MonoBehaviour
{
    [SerializeField] private HingeJoint hinge;
    [SerializeField] private float triggerAngle = 115f;
    private bool hasTriggered;

    private void Awake()
    {
        if (hinge == null)
            hinge = GetComponent<HingeJoint>();
    }

    private void Update()
    {
        

        if (!hasTriggered && hinge.angle >= triggerAngle)
        {
            hasTriggered = true;
            Debug.Log("Triggered at angle: " + hinge.angle);
        }

        if (hasTriggered && hinge.angle < triggerAngle)
        {
            hasTriggered = false;
        }
    }
}