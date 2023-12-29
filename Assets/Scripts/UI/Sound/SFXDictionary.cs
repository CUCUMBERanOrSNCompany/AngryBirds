using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Managing the SFX Dictionary
/// </summary>
public class SFXDictionary : MonoBehaviour
{
    /// <summary>
    /// List of keys for the SFX menu
    /// </summary>
    [SerializeField] private List<SoundsEnum> _sfxKeys = new List<SoundsEnum>();

    /// <summary>
    /// List of values for the SFX menu
    /// </summary>
    [SerializeField] private List<AudioClip> _sfxValues = new List<AudioClip>();

    /// <summary>
    /// The dictionary itself
    /// </summary>
    private Dictionary<SoundsEnum, AudioClip> _sfxMenu = null;

    /// <summary>
    /// Singleton
    /// </summary>
    public static SFXDictionary Instance { get; private set; }

    /// <summary>
    /// Checks that the keys list and the values list is of the same length
    /// And calling the initialization method.
    /// </summary>
    void Start()
    {
        if(_sfxKeys.Count != _sfxValues.Count)
        {
            throw new Exception($"Mismatch between number of SFX names " +
                $"({_sfxKeys.Count}) and SFX clips ({_sfxValues.Count})! " +
                $"Please make sure you provide an audio clip for " +
                $"each SFX name in SFX Dictionary!");
        }

        Instance = this;

        SFXMenuBuilder();
    }

    /// <summary>
    /// Building the SFX menu
    /// </summary>
    private void SFXMenuBuilder()
    {
        _sfxMenu = _sfxKeys.Zip(_sfxValues, (key, value)
            => new { Key = key, Value = value }).ToDictionary(pair
            => pair.Key, pair => pair.Value);
    }

    /// <summary>
    /// Returning a value from the SFX menu
    /// </summary>
    /// <param name="key">Key from the menu</param>
    /// <returns>Audio clip that corresponds to the provided key</returns>
    public AudioClip GetSFX(SoundsEnum key)
    {
        if(_sfxMenu.ContainsKey(key)) return _sfxMenu[key];

        throw new Exception($"You did NOT provided a valid key-value pair for " +
            $"{key} in SFX Dictionary!");
    }
}
