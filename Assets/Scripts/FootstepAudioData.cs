using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="FPS/Footstep Audio Data")]
public class FootstepAudioData: ScriptableObject
{
    public List<FootstepAudio> FootstepAudios = new List<FootstepAudio>();
}
[System.Serializable]
public class FootstepAudio
{
    public string tag;
    public List<AudioClip> AudioClips = new List<AudioClip>();
    public float Delay;
    public float RunDlay;
    public float CrouchDelay;
}