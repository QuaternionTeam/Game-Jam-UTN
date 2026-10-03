using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

internal class AudioManager : MonoBehaviour
{
  internal static AudioManager Instance { get; private set; }

  [Header("Audio Mixer Setup")]
  [SerializeField] private AudioMixer audioMixer;
  [SerializeField] private AudioMixerGroup masterGroup;
  [SerializeField] private AudioMixerGroup musicGroup;
  [SerializeField] private AudioMixerGroup sfxGroup;
  [SerializeField] private AudioMixerGroup uiGroup;
  [SerializeField] private AudioMixerGroup voiceGroup;

  [Header("Audio Pool Settings")]
  [Tooltip("Cantidad de AudioSources en el pool para reproducir efectos simultáneos.")]
  [SerializeField] private int sfxPoolSize = 16;

  [Header("Sound Library")]
  [SerializeField] private Sound[] sounds;

  private Dictionary<string, Sound> _soundDictionary;
  private List<AudioSource> _sfxPool;

  private void Awake()
  {
    if (Instance != null)
    {
      Destroy(gameObject);
      return;

    }

    Instance = this;
    transform.parent = null;
    DontDestroyOnLoad(gameObject);

    InitializeAudioPool();
    InitializeAudioSources();
  }

  private void OnEnable()
  {
    GameEvents.OnPlaySound += Play;
    GameEvents.OnStopSound += Stop;
  }

  private void OnDisable()
  {
    GameEvents.OnPlaySound -= Play;
    GameEvents.OnStopSound -= Stop;
  }

  private void InitializeAudioPool()
  {
    _sfxPool = new List<AudioSource>();

    GameObject poolContainer = new("SFX_Pool");
    poolContainer.transform.SetParent(transform);

    for (int i = 0; i < sfxPoolSize; i++)
    {
      AudioSource source = poolContainer.AddComponent<AudioSource>();
      source.playOnAwake = false;
      _sfxPool.Add(source);
    }
  }

  private void InitializeAudioSources()
  {
    _soundDictionary = new Dictionary<string, Sound>();

    foreach (Sound sound in sounds)
    {
      if (string.IsNullOrEmpty(sound.Name) || sound.Clip == null) continue;

      // Los sonidos con LOOP (como la Música de Fondo) necesitan su propio AudioSource dedicado
      if (sound.Loop)
      {
        sound.Source = gameObject.AddComponent<AudioSource>();
        sound.Source.clip = sound.Clip;
        sound.Source.volume = sound.Volume;
        sound.Source.pitch = sound.Pitch;
        sound.Source.loop = true;
        sound.Source.outputAudioMixerGroup = GetMixerGroup(sound.channel);
      }

      if (!_soundDictionary.ContainsKey(sound.Name))
        _soundDictionary.Add(sound.Name, sound);
      else
        Debug.LogWarning($"[AudioManager] Sonido duplicado omitido: {sound.Name}");
    }
  }

  private AudioMixerGroup GetMixerGroup(AudioChannel channel)
  {
    return channel switch
    {
      AudioChannel.Master => masterGroup,
      AudioChannel.Music  => musicGroup,
      AudioChannel.SFX    => sfxGroup,
      AudioChannel.UI     => uiGroup,
      AudioChannel.Voice  => voiceGroup,
      _                   => masterGroup
    };
  }

  private void Play(string soundName)
  {
    if (!_soundDictionary.TryGetValue(soundName, out Sound sound))
    {
      Debug.LogWarning($"[AudioManager] No se encontró el sonido: {soundName}");
      return;
    }

    // Caso 1: Sonidos en Loop (Música/Ambiente) usan su AudioSource dedicado
    if (sound.Loop)
    {
      if (sound.Source != null && !sound.Source.isPlaying)
        sound.Source.Play();
      return;
    }

    // Caso 2: SFX / UI / Voces de un solo disparo usan el Pool
    AudioSource freeSource = GetFreeAudioSource();
    if (freeSource == null)
    {
      Debug.LogWarning("[AudioManager] Pool de AudioSource agotado. Aumentar 'sfxPoolSize'.");
      return;
    }

    // Configuración del source antes de reproducir
    freeSource.clip = sound.Clip;
    freeSource.volume = sound.Volume;
    freeSource.outputAudioMixerGroup = GetMixerGroup(sound.channel);

    // Variación aleatoria de Pitch si está activada
    float pitch = sound.Pitch;
    if (sound.randomizePitch)
      pitch += Random.Range(-sound.pitchVariance, sound.pitchVariance);
    freeSource.pitch = pitch;

    freeSource.PlayOneShot(sound.Clip, sound.Volume);
  }

  private void Stop(string soundName)
  {
    if (_soundDictionary.TryGetValue(soundName, out Sound sound))
      if (sound.Loop && sound.Source != null)
        sound.Source.Stop();
  }

  private AudioSource GetFreeAudioSource()
  {
    for (int i = 0; i < _sfxPool.Count; i++)
      if (!_sfxPool[i].isPlaying)
        return _sfxPool[i];
    
    return null;
  }

  public void SetChannelVolume(AudioChannel channel, float linearVolume)
  {
    if (audioMixer == null)
      return;

    float clampedVolume = Mathf.Clamp(linearVolume, 0.0001f, 1f);
    float dB = Mathf.Log10(clampedVolume) * 20f;

    string parameterName = channel switch
    {
      AudioChannel.Master => "MasterVolume",
      AudioChannel.Music  => "MusicVolume",
      AudioChannel.SFX    => "SFXVolume",
      AudioChannel.UI     => "UIVolume",
      AudioChannel.Voice  => "VoiceVolume",
      _                   => "MasterVolume"
    };

    audioMixer.SetFloat(parameterName, dB);
  }
}
