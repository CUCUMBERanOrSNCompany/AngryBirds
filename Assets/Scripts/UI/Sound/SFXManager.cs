using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// Playing provided SFX.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the AudioSource component
    /// </summary>
    private AudioSource _source = null;

    /// <summary>
    /// Let us to dispose unused listeners.
    /// </summary>
    private CompositeDisposable _disposables = new CompositeDisposable();

    /// <summary>
    /// Getting reference for SFX audio source.
    /// </summary>
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.mute = MuteFlag.Instance.IsMuted.Value;
        ListenerSetter();
    }

    /// <summary>
    /// Setting a listener to the global mute state
    /// </summary>
    private void ListenerSetter()
    {
        MuteFlag.Instance.IsMuted
            .Subscribe(isMuted =>
            {
                if(_source != null) _source.mute = isMuted;
            }).AddTo(_disposables);
    }

    /// <summary>
    /// Playing the SFX only if nothing is playing right now.
    /// </summary>
    /// <param name="key"></param>
    public void PlaySFXNoOverride(SoundsEnum key)
    {
        if(_source.isPlaying)
        {
            return;
        }

        _source.clip = SFXDictionary.Instance.GetSFX(key);
        _source.Play();
    }

    /// <summary>
    /// Playing the SFX and overriding any SFX playing right now.
    /// </summary>
    /// <param name="key"></param>
    public void PlaySFXOverride(SoundsEnum key)
    {
        _source.Stop();
        _source.clip = SFXDictionary.Instance.GetSFX(key);
        _source.Play();
    }




}
