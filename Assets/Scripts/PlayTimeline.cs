using UnityEngine;
using UnityEngine.Playables;

public class PlayTimeline : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

    private void Awake()
    {
        if (director == null)
            director = GetComponent<PlayableDirector>();

        if (director != null)
        {
            director.playOnAwake = false;
            director.extrapolationMode = DirectorWrapMode.Hold;
        }
    }

    public void PlayFromStart()
    {
        if (director == null)
            return;

        director.time = 0;
        director.Evaluate();
        director.Play();
    }
}