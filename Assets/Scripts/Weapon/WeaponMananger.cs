using Assets.Scripts.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMananger : MonoBehaviour
{
    public Firearms MainWeapon;
    public Firearms SecondaryWeapon;

    private Firearms carriedWeapon;
    private FPController fPController;
    private IEnumerator waitingForHolsterEndCoroutine;
    private void Start()
    {
        carriedWeapon = MainWeapon;
        fPController = FindObjectOfType<FPController>();
        fPController.SetUpAnimator(carriedWeapon.GunAnimtor);
    }

    private void Update()
    {
        if (!carriedWeapon) return;
        SwapWeapon();
        if (Input.GetMouseButton(0))
        {
            carriedWeapon.HoldTrigger();
        }
        if (Input.GetMouseButtonUp(0))
        {
            carriedWeapon.ReleaseTrigger();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            carriedWeapon.ReloadAmmo();
        }

        if (Input.GetMouseButtonDown(1))
        {
            carriedWeapon.Aiming(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            carriedWeapon.Aiming(false);
        }
    }

    private void SwapWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            //更换为主武器
            if (carriedWeapon == MainWeapon) return;
            if(carriedWeapon.gameObject.activeInHierarchy)
            {
                StartWaitingForHolsterEndCoroutine();
                carriedWeapon.GunAnimtor.SetTrigger("holster");
            }
            else
            {
                SetupCarriedWeapon(MainWeapon);
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //更换为副武器
            if (carriedWeapon == SecondaryWeapon) return;
            if (carriedWeapon.gameObject.activeInHierarchy)
            {
                StartWaitingForHolsterEndCoroutine();
                carriedWeapon.GunAnimtor.SetTrigger("holster");
            }
            else
            {
                SetupCarriedWeapon(SecondaryWeapon);
            } 
        }
        
    }

    private void StartWaitingForHolsterEndCoroutine()
    {
        if (waitingForHolsterEndCoroutine == null)
            waitingForHolsterEndCoroutine = WaitingForHolsterEnd();
        StartCoroutine(waitingForHolsterEndCoroutine);
    }
    private IEnumerator WaitingForHolsterEnd()
    {
        while(true)
        {
            AnimatorStateInfo tmp_AnimatorStateInfo = carriedWeapon.GunAnimtor.GetCurrentAnimatorStateInfo(0);
            if(tmp_AnimatorStateInfo.IsTag("holster"))
            {
                if(tmp_AnimatorStateInfo.normalizedTime>=0.9f)
                {
                    var tmp_TargetWeapon = carriedWeapon == MainWeapon ? SecondaryWeapon : MainWeapon;
                    SetupCarriedWeapon(tmp_TargetWeapon);
                    waitingForHolsterEndCoroutine = null;
                    yield break;

                }
            }
            yield return null;
        }
    }
    private void SetupCarriedWeapon(Firearms _targetWeapon)
    {
        carriedWeapon.gameObject.SetActive(false);
        carriedWeapon = _targetWeapon;
        carriedWeapon.gameObject.SetActive(true);
    }
}
