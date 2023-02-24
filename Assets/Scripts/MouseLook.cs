using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform PlayerBody;//玩家当前位置
    public Transform FPcon;//fpcontroller当前位置
    private Vector3 cameraRotation;
    public float mouseSen = 150f;

    public AnimationCurve RecoilCurve;
    public Vector2 RecoilRange;

    public float RecoilFadeOutTime=0.3f;
    private float currentRecoilTime;
    private Vector2 currentRecoil;

    public CameraSpring cameraSpring;

    // Start is called before the first frame update
    void Start()
    {
        //隐藏光标
        Cursor.lockState = CursorLockMode.Locked;
        cameraTransform = transform;
        currentRecoil = RecoilRange;
        cameraSpring = GetComponentInChildren<CameraSpring>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSen * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSen * Time.deltaTime;
        cameraRotation.x -= mouseY;
        cameraRotation.y += mouseX;
        CalculateRecoilOffset();
        cameraRotation.x -= currentRecoil.x;
        cameraRotation.y += currentRecoil.y;
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, -80f, 80f);
        cameraTransform.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0f);
        //Debug.Log(cameraRotation.y);
        PlayerBody.rotation = Quaternion.Euler(0f, cameraRotation.y, 0f);
        FPcon.rotation = Quaternion.Euler(0f, cameraRotation.y, 0f);

    }

    private void CalculateRecoilOffset()
    {
        currentRecoilTime += Time.deltaTime;
        float tmp_RecoilFraction = currentRecoilTime/RecoilFadeOutTime;
        float tmp_Recoilvalue = RecoilCurve.Evaluate(tmp_RecoilFraction);

        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_Recoilvalue);
        
    }
    public void FringForTest()
    {
        currentRecoil += RecoilRange;
        cameraSpring.StartCameraSpring();
        currentRecoilTime = 0;
    }
}
