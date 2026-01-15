using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] public List<PlayableDirector> _timelineScenes;

    public void PlayCutscene(int targetNumber)
    {
        _timelineScenes[targetNumber].Play();
    }
}
