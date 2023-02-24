using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    [CreateAssetMenu(menuName ="FPS/Impact Audio Data")]
    public class ImpactAudioData:ScriptableObject
    {
        public List<ImpactTagsWithAudio> ImpactTagsWithAudios;


    }
    [System.Serializable]
    public class ImpactTagsWithAudio
    {
        public string Tag;
        public List<AudioClip> ImpactAudioClips;
    }
}
