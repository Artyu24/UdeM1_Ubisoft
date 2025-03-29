using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    [SerializeField] private string name;
    [SerializeField] private AudioClip clip;
    [SerializeField, Range(0f, 1f)] private float volume = 0.9f;
    [SerializeField, Range(.1f, 3f)] private float pitch = 1f;
    [SerializeField] private bool loop;
    [SerializeField] private bool atStart;

    [SerializeField] private AudioMixerGroup audioMixer;

    private AudioSource source;
    [SerializeField] private SoundState actualSound;

    #region PROPERTY
    public string Name => name;
    public AudioClip Clip => clip;
    public float Volume => volume;
    public float Pitch => pitch;
    public bool Loop => loop;
    public bool AtStart => atStart;
    public AudioMixerGroup AudioMixer => audioMixer;

    public AudioSource Source { get => source; set => source = value; }
    public SoundState ActualSound => actualSound;

    #endregion
}
