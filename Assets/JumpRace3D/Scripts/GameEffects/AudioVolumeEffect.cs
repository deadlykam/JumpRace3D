using UnityEngine;

[System.Serializable]
public class AudioVolumeEffect : BasicAudio
{
    [Header("Audio Volume Effect")]
    [Tooltip("The minimum volume in percentage")]
    [Range(0f, 1f)]
    [SerializeField]
    private float _minVolumePercentage; // The minimum volume in percentage

    /// <summary>
    /// The minimum volume, of type float
    /// </summary>
    private float _minVolume
    { get { return Volume * _minVolumePercentage; } }

    [SerializeField]
    private float _speed; // The speed to change the volume effect
    
    private float _currentVolume; // The current volume level of the
                                  // audio clip

    /// <summary>
    /// This returns the current volume of the clip, of type float
    /// </summary>
    public float CurrentVolume
    { get { return _currentVolume < _minVolume ? 
                    _minVolume : 
                    _currentVolume; } }

    private float _step = 0; // Needed to change the lerp

    private float _fps; // Storing the fps value for accurate calculation

    /// <summary>
    /// This method updates the AudioVolemEffect.
    /// </summary>
    public void UpdateAudioVolumeEffect()
    {
        _fps = _speed * Time.deltaTime; // Storing the fps value

        // Calculating the step for lerping
        _step = _step + _fps >= Volume ? Volume : _step + _fps;

        // Changing the strength of the volume
        _currentVolume = Mathf.Lerp(_minVolume, Volume, _step);
    }

    /// <summary>
    /// This method resets the current volume strength to the minimum
    /// volume.
    /// </summary>
    public void ResetVolume() => _step = 0;
}
