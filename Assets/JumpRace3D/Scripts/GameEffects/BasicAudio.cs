using UnityEngine;

[System.Serializable]
public class BasicAudio
{
    [Header("Basic Audio Properties")]
    [SerializeField]
    private AudioClip _clip;

    /// <summary>
    /// The audio clip of the BasicAudio, of type
    /// AudioClip
    /// </summary>
    public AudioClip Clip { get { return _clip; } }

    [SerializeField]
    [Range(0, 1)]
    private float _volume;

    /// <summary>
    /// The volume of the audio clip, of type float
    /// </summary>
    public float Volume { get { return _volume; } }
}
