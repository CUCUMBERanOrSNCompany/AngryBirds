using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for muting/unmuting sound
/// </summary>
public class MuteOrUnMute : MonoBehaviour
{
    /// <summary>
    /// List of audio sources. E.g. Background, SFX
    /// </summary>
    [SerializeField] private List<AudioSource> _audioSources = null;

    /// <summary>
    /// Switching the isMuted State and applying it to the audio sources.
    /// </summary>
    public void ChangeMuteStateOfAllSources()
    {

        MuteFlag.Instance.IsMuted.Value = !MuteFlag.Instance.IsMuted.Value;

        foreach(var source in _audioSources)
        {
            source.mute = MuteFlag.Instance.IsMuted.Value;
        }
    }
}
