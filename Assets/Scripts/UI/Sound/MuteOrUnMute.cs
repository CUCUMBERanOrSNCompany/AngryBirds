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
    /// Getting the Player preferences for mute, and applying it.
    /// </summary>
    private void Start()
    {
        if(PlayerPrefs.HasKey("Mute"))
        {
            if (PlayerPrefs.GetInt("Mute") == 1) ChangeMuteStateOfAllSources();
        }
        else
        {
            PlayerPrefs.SetInt("Mute", 0);
        }
    }

    /// <summary>
    /// Switching the isMuted State and applying it to the audio sources.
    /// </summary>
    public void ChangeMuteStateOfAllSources()
    {
        // Flipping the current mute state.
        bool isMuted = !MuteFlag.Instance.IsMuted.Value;

        // Applying the new state to the global dirty flag.
        MuteFlag.Instance.IsMuted.Value = isMuted;

        // Saving the new state.
        PlayerPrefs.SetInt("Mute", isMuted ? 1 : 0);

        foreach(var source in _audioSources)
        {
            source.mute = MuteFlag.Instance.IsMuted.Value;
        }
    }
}
