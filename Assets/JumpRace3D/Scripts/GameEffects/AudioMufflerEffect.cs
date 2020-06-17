using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioMufflerEffect
{
    [Header("Audio Muffler Effect Properties")]
    [SerializeField]
    private AudioLowPassFilter _lowPassFilter; // The lowpass filter

    [SerializeField]
    private float _maxFreq; // The maximum cutoff frequency

    [SerializeField]
    private float _minFreq; // The minimum cutoff frequency
    
    /// <summary>
    /// Flag to check if lowpass effect is happening, of type bool
    /// </summary>
    private bool _isEffect
    { get { return GameData.Instance.SimulationSpeed != 1; } }


    /// <summary>
    /// This method updates the AudioMufflerEffect.
    /// </summary>
    public void UpdateAudioMufflerEffect()
    {
        // Condition for applying the lowpass filter effect
        if (_isEffect)
            _lowPassFilter.cutoffFrequency = Mathf.Lerp(
                                                _minFreq,
                                                _maxFreq,
                                                GameData.Instance.
                                                SimulationSpeedNormalized);
        else // Condition for not applying the lowpass filter effect
        {
            // Condition to fix the lowpass filter frequency value
            if (_lowPassFilter.cutoffFrequency != _maxFreq)
                _lowPassFilter.cutoffFrequency = _maxFreq;
        }
    }
}
