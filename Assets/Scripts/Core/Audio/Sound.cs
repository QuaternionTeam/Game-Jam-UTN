using System;
using UnityEngine;

public enum AudioChannel
{
  Master,
  Music,
  SFX,
  UI,
  Voice
}

[Serializable]
internal class Sound 
{
  [SerializeField] internal string Name;
  [SerializeField] internal AudioClip Clip;
  [SerializeField] internal AudioChannel channel = AudioChannel.SFX;

  [Range(0f, 1f)]   [SerializeField] internal float Volume = 0.5f;
  [Range(0.1f, 3f)] [SerializeField] internal float Pitch = 1f;
  [SerializeField] internal bool Loop = false;

  [Header("SFX Only")]
  [SerializeField] internal bool randomizePitch = false;
  [Range(0f, 0.5f)] [SerializeField] internal float pitchVariance = 0.1f;

  [HideInInspector] internal AudioSource Source;
}
