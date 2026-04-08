using UnityEngine;
using UnityEngine.Playables;

public class TimelineButtonHelper : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

    public void PlayFromStart()
    {
        director.time = 0;
        director.Evaluate();   // force frame 0
        director.Play();
    }
}