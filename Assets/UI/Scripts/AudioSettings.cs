using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
  [Header("Sliders de Volumen")]
  [SerializeField] private Slider masterSlider;
  [SerializeField] private Slider musicSlider;
  [SerializeField] private Slider sfxSlider;
  [SerializeField] private Slider uiSlider;

  [Header("Claves de Guardado (PlayerPrefs)")]
  private const string MASTER_KEY = "MasterVolumePref";
  private const string MUSIC_KEY = "MusicVolumePref";
  private const string SFX_KEY = "SFXVolumePref";
  private const string UI_KEY = "UIVolumePref";

  private void Start()
  {
    InitializeSliders();
  }

  private void InitializeSliders()
  {
    /* Cargar valores guardados o defecto (1.0) */
    float masterVol = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
    float musicVol = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
    float sfxVol = PlayerPrefs.GetFloat(SFX_KEY, 1f);
    float uiVol = PlayerPrefs.GetFloat(UI_KEY, 1f);

    /* Vincular sliders y setear volúmenes */
    masterSlider.value = masterVol;
    masterSlider.onValueChanged.AddListener(SetMasterVolume);

    musicSlider.value = musicVol;
    musicSlider.onValueChanged.AddListener(SetMusicVolume);

    sfxSlider.value = sfxVol;
    sfxSlider.onValueChanged.AddListener(SetSFXVolume);

    uiSlider.value = uiVol;
    uiSlider.onValueChanged.AddListener(SetUIVolume);

    SetMasterVolume(masterVol);
    SetMusicVolume(musicVol);
    SetSFXVolume(sfxVol);
    SetUIVolume(uiVol);
  }

  public void SetMasterVolume(float value)
  {
    AudioManager.Instance.SetChannelVolume(AudioChannel.Master, value);
    PlayerPrefs.SetFloat(MASTER_KEY, value);
  }

  public void SetMusicVolume(float value)
  {
    AudioManager.Instance.SetChannelVolume(AudioChannel.Music, value);
    PlayerPrefs.SetFloat(MUSIC_KEY, value);
  }

  public void SetSFXVolume(float value)
  {
    AudioManager.Instance.SetChannelVolume(AudioChannel.SFX, value);
    PlayerPrefs.SetFloat(SFX_KEY, value);
  }

  public void SetUIVolume(float value)
  {
    AudioManager.Instance.SetChannelVolume(AudioChannel.UI, value);
    PlayerPrefs.SetFloat(UI_KEY, value);
  }
}
