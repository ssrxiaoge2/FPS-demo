using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Weapon
{
    public class Bullet:MonoBehaviour
    {
        public float BulletSpeed=100f;
        public GameObject ImpactPrefab;
        public ImpactAudioData ImpactAudioData;


        public Transform bulletTransform;
        private Vector3 prevPosition;

        private void Start()
        {
            
            bulletTransform = transform;
            prevPosition = bulletTransform.position;

        }
        private void Update()
        {
            prevPosition = bulletTransform.position;
            bulletTransform.Translate(0, 0, BulletSpeed*Time.deltaTime);
            if (!Physics.Raycast(prevPosition, (bulletTransform.position - prevPosition).normalized,
                out RaycastHit tmp_Hit, (bulletTransform.position - prevPosition).magnitude)) return;
            var tmp_BulletEffect = Instantiate(ImpactPrefab, 
                tmp_Hit.point, Quaternion.LookRotation(tmp_Hit.normal, Vector3.up));
            Destroy(tmp_BulletEffect, 8);
           
            //播放音频
            var tmp_TagsWithAudio=
                ImpactAudioData.ImpactTagsWithAudios.Find((_audioData)=>_audioData.Tag.Equals(tmp_Hit.collider.tag));
            if (tmp_TagsWithAudio == null) return;
            int tmp_Length = tmp_TagsWithAudio.ImpactAudioClips.Count;
            AudioClip tmp_AudioClip = tmp_TagsWithAudio.ImpactAudioClips[Random.Range(0, tmp_Length)];
            AudioSource.PlayClipAtPoint(tmp_AudioClip, tmp_Hit.point);
            
            
            
            
        }
    }
}
