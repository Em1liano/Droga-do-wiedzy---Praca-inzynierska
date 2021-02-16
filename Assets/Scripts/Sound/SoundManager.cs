using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        PlayerMove,
        PlayerAttack,
        Dash,


        ArcherBow,
        ArcherMelee,
        ArcherDead,

        BeetleHit,
        BeetleDead,


        ZombieGetHit,
        ZombieExpload,

        MenuClick,
        MenuChange,

        DoorOpen,

        Teleport,

        PlayerHit,
        PlayerDead,

        CrabStartup,
        CrabAttack1,
        CrabAttack2,
        CrabGetHit,
        CrabDead
    }

    public enum SoundTrack
    {
        Menu,
        Level1,
        Level2,
        Level3,
        Boss,
        Level4,
        EndScreen
    }
    private static Dictionary<Sound, float> soundTimerDictionary;

    public static void Initialize()
    {
        soundTimerDictionary = new Dictionary<Sound, float>();
        soundTimerDictionary[Sound.PlayerMove] = 0f;
    }

    public static void PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            soundGameObject.tag = "SFX";
            soundGameObject.transform.position = position;
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.volume = .1f;
            audioSource.clip = GetAudioClip(sound);
            audioSource.Play();
        }
    }
    public static void PlaySoundTrack(SoundTrack soundTrack)
    {
        GameObject soundTrackGameObject = new GameObject("SoundTrack");
        AudioSource audioSource = soundTrackGameObject.AddComponent<AudioSource>();
        audioSource.volume = PlayerPrefsController.GetMasterVolume();
        audioSource.loop = true;
        audioSource.clip = GetAudioClipSoundTrack(soundTrack);

        audioSource.Play();
    }
    public static void SetVolume(float volume)
    {
        GameObject.Find("SoundTrack").GetComponent<AudioSource>().volume = volume;
    }
    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            GameObject soundGameObject = new GameObject("Sound");
            AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;
            case Sound.PlayerMove:
                if (soundTimerDictionary.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDictionary[sound];
                    float playerMoveTimerMax = .5f;
                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        soundTimerDictionary[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }   
                //break;
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameController.SoundAudioClip soundAudioClip in GameController.instance.SoundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + "Not found!");
        return null;
    }
    private static AudioClip GetAudioClipSoundTrack(SoundTrack soundTrack)
    {
        foreach (GameController.SoundtrackAudioClip soundAudioClipSoundTrack in GameController.instance.SoundTrackAudioClipArray)
        {
            if (soundAudioClipSoundTrack.soundTrack == soundTrack)
            {
                return soundAudioClipSoundTrack.audioClip;
            }
        }
        Debug.LogError("Sound " + soundTrack + "Not found!");
        return null;
    }
}
