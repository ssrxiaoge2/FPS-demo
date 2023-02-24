using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Weapon
{
    public abstract class Firearms :MonoBehaviour,IWeapon
    {
        public Transform MuzzlePoint;//枪口
        public Transform CasingPoint;//抛壳点

        public Camera EyeCamera; 

        public ParticleSystem muzzleParticle;
        public GameObject casingParticle;

        public AudioSource FirearmsShootingAudioSource;
        public AudioSource FirearmsReloadAudioSource;
        public FireamsAudioData FireamsAudioData;
        public ImpactAudioData ImpactAudioData;

        public float FireRate=11.7f;

        public int AmmoMsg = 30;
        public int MaxAmmoCarried=120;
        public GameObject BulletPrefab;

        protected int CurrentAmmo = 30;
        protected int CurrentMaxAmmoCarried = 120;
        protected float lastFireTime=1f;

        public Animator GunAnimtor;
        protected AnimatorStateInfo GunStateInfo;
        protected float OriginFOV=70f;
        protected bool isAiming;
        protected bool IsHoldingTrigger;

        public FPController FP;

        public float SpreadAngle;
        private IEnumerator doAimCoroutinue;
        protected virtual void start()
        {
            CurrentAmmo = AmmoMsg;
            CurrentMaxAmmoCarried = MaxAmmoCarried;
            GunAnimtor = GetComponent<Animator>();
            OriginFOV = EyeCamera.fieldOfView;
            doAimCoroutinue = DoAim();
        }
        public void DoAttack()
        {
            Shooting();
        }

        protected abstract void Shooting();
        protected abstract void Reload();
        //protected abstract void Aim();

        
        protected bool IsAllowShooting()
        {
            return Time.time - lastFireTime > 1 / FireRate;
             
        }
        //子弹散射
        protected Vector3 CalculateSpreadOffset()
        {
            float tmp_SpreadPercent= SpreadAngle / EyeCamera.fieldOfView;

            return tmp_SpreadPercent * UnityEngine.Random.insideUnitCircle;
        }

        protected IEnumerator CheckReloadAmmoAnimationEnd()
        {
            while (true)
            {
                yield return null;
                GunStateInfo = GunAnimtor.GetCurrentAnimatorStateInfo(2);
                if (GunStateInfo.IsTag("ReloadAmmo"))
                {
                    if (GunStateInfo.normalizedTime >= 0.9f)
                    {
                        int tmp_NeedAmmoCount = AmmoMsg - CurrentAmmo;
                        int tmp_RemainAmmo = CurrentMaxAmmoCarried - tmp_NeedAmmoCount;
                        if (tmp_RemainAmmo <= 0)
                        {
                            CurrentAmmo += CurrentMaxAmmoCarried;
                        }
                        else
                        {
                            CurrentAmmo = AmmoMsg;
                        }
                        CurrentMaxAmmoCarried = tmp_RemainAmmo <= 0 ? 0 : tmp_RemainAmmo;
                        GunAnimtor.SetLayerWeight(2, 0);
                        yield break;
                    }
                }
            }
        }

        protected IEnumerator DoAim()
        {
            while (true)
            {
                yield return null;

                float tmp_CurrentFOV = 0f;
                EyeCamera.fieldOfView = Mathf.SmoothDamp(EyeCamera.fieldOfView,
                    isAiming ? 26 : OriginFOV,
                    ref tmp_CurrentFOV, Time.deltaTime * 2);
            }
        }
        internal void Aiming(bool _IsAiming)
        {
            isAiming = _IsAiming;
            GunAnimtor.SetLayerWeight(1, 1);
            GunAnimtor.SetBool("aim", isAiming);
            if (doAimCoroutinue == null)
            {
                doAimCoroutinue = DoAim();
                StartCoroutine(doAimCoroutinue);
            }
            else
            {
                StopCoroutine(doAimCoroutinue);
                doAimCoroutinue = null;
                doAimCoroutinue = DoAim();
                StartCoroutine(doAimCoroutinue);
            }
        }

        internal void HoldTrigger()
        {
            DoAttack();
            IsHoldingTrigger = true;
        }
        internal void ReleaseTrigger()
        {
            IsHoldingTrigger = false;
        }
        internal void ReloadAmmo()
        {
            Reload();
        }
    }
}
