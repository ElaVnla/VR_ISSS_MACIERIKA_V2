using UnityEngine;
using UnityEngine.Playables;

public class LeverDebug : MonoBehaviour
{
    [SerializeField] private HingeJoint hinge;
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private float triggerAngle = 115f;

    private bool hasTriggered;

    private void Awake()
    {
        if (hinge == null)
            hinge = GetComponent<HingeJoint>();

        if (timeline == null)
            timeline = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (!hasTriggered && hinge.angle >= triggerAngle)
        {
            hasTriggered = true;
            Debug.Log("Triggered at angle: " + hinge.angle);
            timeline.Play();

        }

        if (hasTriggered && hinge.angle < triggerAngle)
        {
            hasTriggered = false;
        }
    }
}