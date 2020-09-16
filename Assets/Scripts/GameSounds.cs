using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSounds {

    public enum Sound {
        PlayerMovement,
        PlayerBlock,
        PlayerChange,
        LvLWin,
        LvLFail
    }

    private static GameObject soundGameObject;
    private static AudioSource audioSource;
    
    public static void PlayerSound(Sound sound) {
        if(soundGameObject == null) {
            soundGameObject = new GameObject("Sound");
            audioSource = soundGameObject.AddComponent<AudioSource>();
        }
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    public static void SetVolume(float volume) {
        audioSource.volume = volume;
    }
    
    private static AudioClip GetAudioClip(Sound sound) {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.instance.soundsArray) {
            if (soundAudioClip.sound == sound) {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }
}
