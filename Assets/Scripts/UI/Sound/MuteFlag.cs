using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// Responsible for Constantly indicate the global sound volume in the game.
/// </summary>
public class MuteFlag : MonoBehaviour
{
    /// <summary>
    /// Singleton
    /// </summary>
    public static MuteFlag Instance { get; private set; } = null;

    /// <summary>
    /// Mute State
    /// </summary>
    public ReactiveProperty<bool> IsMuted { get; set; }
        = new ReactiveProperty<bool>(false);

    private void Start()
    {
        Instance = this;
    }

}
