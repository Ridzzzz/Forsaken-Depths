using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    // Setup SoundManager in Prefab
    // Use these commands from other classes to play music/sfx
    // AudioManager.Instance.ToggleMusic(true, AudioManager.MusicType.MENUBG);
    // AudioManager.Instance.playSFX(AudioManager.SFXType.eSFX1);

	public static AudioManager Instance;

    public bool playMusic;

    //audio source files, this script goes on a prefab, along with
    //audio sources, and these are the audio sources on said prefab
    public AudioSource ExampleSFX1;
    public AudioSource ExampleSFX2;
    public AudioSource PreGameBackground;
    public AudioSource InGameBackground;
    public AudioSource DeathScreenBackground;

    //indicate ALL SXF by enum, call by this
    public enum SFXType { eSFX1, eSFX2 };
    public enum MusicType { PreGameBG, InGameBG, DeathScreenBG };

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start() {
        // SET GAME MUSIC
        // To Loop Music Sound
        // Refer to audiosource.volume documentation for more
        PreGameBackground.loop = true;
        InGameBackground.loop = true;
        DeathScreenBackground.loop = true;
        ToggleMusic(playMusic = true, MusicType.PreGameBG);
    }

    //plays a sound effect.
    // IMPORTANT: If you want value adjusted so it will open play based on distance from the player, refer to video on 3D sound
    public void playSFX(SFXType sound) {
        switch (sound) {
            case SFXType.eSFX1:
                if (!ExampleSFX1.isPlaying) {
                    ExampleSFX1.volume = 0.25f;
                    ExampleSFX1.Play();
                }
                break;
            case SFXType.eSFX2:
                if (!ExampleSFX2.isPlaying) {
                    ExampleSFX2.volume = 0.25f;
                    ExampleSFX2.Play();
                }
                break;
        }
    }

    public void ToggleMusic(bool play, MusicType type) {
        if (PreGameBackground.isPlaying) {
            PreGameBackground.Pause();
        }
        if (InGameBackground.isPlaying) {
            InGameBackground.Pause();
        }
        if (DeathScreenBackground.isPlaying) {
            DeathScreenBackground.Pause();
        }
        if (play) {
            switch (type) {
                case MusicType.PreGameBG:
                    PreGameBackground.Play();
                    break;
                case MusicType.InGameBG:
                    InGameBackground.Play();
                    break;
                case MusicType.DeathScreenBG:
                    DeathScreenBackground.Play();
                    break;
                default:
                    break;
            }
        }
        playMusic = play;
    }
}
