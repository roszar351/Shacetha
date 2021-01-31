using UnityEngine.Audio;
using System;
using UnityEngine;

// Contains methods to play music and sounds, change volume levels
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public AudioMixer mixer;
    public AudioMixerGroup soundMixer;
    public AudioMixerGroup musicMixer;

    public Sound[] sounds;
    public Sound[] music;

    // Singleton, need to initialize all the sound clips and make sure the audio manager doesnt get destroyed between scenes to keep music playing
    // using singleton as game should only ever have one audio manager playing sounds i.e. prevents duplication of sounds.
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.LogError("Tried to create another instance of " + this.name);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = soundMixer;
        }

        foreach (Sound s in music)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.outputAudioMixerGroup = musicMixer;
        }
    }

    private void Start()
    {
        PlayMusic("Theme");
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(music, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }

    public void PauseSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Pause();
    }

    public void PauseMusic(string name)
    {
        Sound s = Array.Find(music, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Pause();
    }

    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Pause();
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(music, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Pause();
    }

    public void PlayOneShotSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.PlayOneShot(s.source.clip);
    }

    public void PlayOneShotMusic(string name)
    {
        Sound s = Array.Find(music, sound => sound.name == name);
        if (s == null)
            return;
        s.source.PlayOneShot(s.source.clip);
    }

    public void PlayMovementSound()
    {
        Sound s = Array.Find(sounds, sound => sound.name == "Movement");
        if (s == null)
            return;

        if (!s.source.isPlaying)
            s.source.Play();
    }

    public void UpdateMasterVolume(float masterVolume)
    {
        mixer.SetFloat("masterVolume", masterVolume);
    }

    public void UpdateMusicVolume(float musicVolume)
    {
        mixer.SetFloat("musicVolume", musicVolume);
    }

    public void UpdateEffectsVolume(float effectsVolume)
    {
        mixer.SetFloat("effectsVolume", effectsVolume);
    }
}
