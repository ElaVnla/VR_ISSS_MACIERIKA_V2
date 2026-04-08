using UnityEngine;
using UnityEngine.Playables;

public class PlayTimelineOnStart : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;

    private void Start()
    {
        if (director == null)
            director = GetComponent<PlayableDirector>();

        if (director == null)
            return;

        director.time = 0;
        director.Evaluate();
        director.Play();
    }
}