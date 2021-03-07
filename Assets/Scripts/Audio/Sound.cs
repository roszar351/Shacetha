using UnityEngine.Audio;
using UnityEngine;

// Class that describes a sound
[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0, 1f)]
    public float volume;
    [Range(-3f, 3f)]
    public float pitch = 1f;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
