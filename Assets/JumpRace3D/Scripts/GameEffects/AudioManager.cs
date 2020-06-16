using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Manager Properties")]
    [SerializeField]
    private AudioSource _musicSource;

    [SerializeField]
    private AudioSource _soundfxSource;

    [SerializeField]
    private BasicAudio _music;

    [SerializeField]
    private BasicAudio[] _hurt;

    [SerializeField]
    private BasicAudio _stageBounce;

    [SerializeField]
    private BasicAudio _waterSplash;

    [SerializeField]
    private BasicAudio _confetti;

    [SerializeField]
    private BasicAudio _boosterPickup;

    [SerializeField]
    private BasicAudio _winnerMusic;

    [SerializeField]
    private BasicAudio _winnerClap;

    [SerializeField]
    private BasicAudio _loserMusic;


    public static AudioManager Instance;


    private void Awake()
    {
        if (Instance == null) // If instance null then assigning it
        {
            Instance = this; // Assigning instance
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Setting and playing the music
        _musicSource.clip = _music.Clip;
        _musicSource.volume = _music.Volume;
        _musicSource.Play(); // Playing the music
    }

    /// <summary>
    /// This method plays the soundfx.
    /// </summary>
    /// <param name="soundfx">The soundfx to play, of type
    ///                       BasicAudio</param>
    private void PlaySoundFx(BasicAudio soundfx)
        => _soundfxSource.PlayOneShot(soundfx.Clip, soundfx.Volume);

    /// <summary>
    /// This method plays the stage bounce sfx.
    /// </summary>
    public void PlayStageBounce() => PlaySoundFx(_stageBounce);

    /// <summary>
    /// This method plays a random hurt sfx.
    /// </summary>
    public void PlayHurt() => PlaySoundFx(_hurt[Random.Range(0, _hurt.Length)]);

    /// <summary>
    /// This method plays the water splash sfx.
    /// </summary>
    public void PlayerWaterSplash() => PlaySoundFx(_waterSplash);

    /// <summary>
    /// This method plays the confetti sfx.
    /// </summary>
    public void PlayConfetti() => PlaySoundFx(_confetti);

    /// <summary>
    /// This method plays the booster pickup sfx.
    /// </summary>
    public void PlayBoosterPickup() => PlaySoundFx(_boosterPickup);

    /// <summary>
    /// This method plays the winner sfx.
    /// </summary>
    public void PlayWinner()
    {
        PlaySoundFx(_winnerMusic);
        PlaySoundFx(_winnerClap);
    }

    /// <summary>
    /// This method plays the looser sfx.
    /// </summary>
    public void PlayLoser() => PlaySoundFx(_loserMusic);
}
