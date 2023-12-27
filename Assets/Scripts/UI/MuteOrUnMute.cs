using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for muting/unmuting sound
/// </summary>
public class MuteOrUnMute : MonoBehaviour
{
    [SerializeField] private List<AudioSource> _audioSources = null;

    private bool _isMuted = false;

    public void ChangeMuteStateOfAllSources()
    {
        _isMuted = !_isMuted;

        foreach(var source in _audioSources)
        {
            source.mute = _isMuted;
        }
    }
}
