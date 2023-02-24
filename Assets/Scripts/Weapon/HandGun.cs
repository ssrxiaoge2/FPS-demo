using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts.Weapon
{
    public class HandGun : Firearms
    {
        private IEnumerator reloadAmmoCheckerCoroutine;


        public GameObject BulletImpactPrefab;
        public MouseLook ML;

        protected override void start()
        {
            base.start();
            reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
        }
        protected override void Reload()
        {

            GunAnimtor.SetLayerWeight(2, 1);
            GunAnimtor.SetTrigger(CurrentAmmo > 0 ? "ReloadLeft" : "ReloadOutOf");
            FirearmsReloadAudioSource.clip =
                CurrentAmmo > 0 ? FireamsAudioData.ReloadLeft : FireamsAudioData.ReloadOutOf;
            FirearmsReloadAudioSource.Play();
            if (reloadAmmoCheckerCoroutine == null)
            {
                reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(reloadAmmoCheckerCoroutine);
            }
            else
            {
                StopCoroutine(reloadAmmoCheckerCoroutine);
                reloadAmmoCheckerCoroutine = null;
                reloadAmmoCheckerCoroutine = CheckReloadAmmoAnimationEnd();
                StartCoroutine(reloadAmmoCheckerCoroutine);
            }

        }

        protected override void Shooting()
        {
            if (FP.MovementSpeed == FP.runSpeed) return;
            if (CurrentAmmo <= 0) return;
            if (!IsAllowShooting()) return;
            muzzleParticle.Play();
            CurrentAmmo -= 1;
            GunAnimtor.Play("fire", isAiming ? 1 : 0, 0);//播放哪个图层的fire动画
            FirearmsShootingAudioSource.clip = FireamsAudioData.ShootingAudio;
            FirearmsShootingAudioSource.volume = 0.5f;
            FirearmsShootingAudioSource.Play();
            CreateBullet();
            Instantiate(casingParticle, CasingPoint.transform.position, CasingPoint.transform.rotation);
            ML.FringForTest();
            lastFireTime = Time.time;
        }
        protected void CreateBullet()
        {
            GameObject tmp_Bullet = Instantiate(BulletPrefab, MuzzlePoint.position, MuzzlePoint.rotation);
            tmp_Bullet.transform.eulerAngles += CalculateSpreadOffset();//子弹散射
            var tmp_BulletScript = tmp_Bullet.AddComponent<Bullet>();
            tmp_BulletScript.ImpactPrefab = BulletImpactPrefab;
            tmp_BulletScript.ImpactAudioData = ImpactAudioData;
            tmp_BulletScript.BulletSpeed = 100f;

        }

    }
}
