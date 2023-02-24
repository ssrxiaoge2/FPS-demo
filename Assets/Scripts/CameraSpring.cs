using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpring : MonoBehaviour
{
    public float Frequence = 25f;
    public float Damp = 15f;

    public Vector2 MinRecoilRange = new Vector2(-2, -2);
    public Vector2 MaxRecoilRange = new Vector2(2, 2);


    private CameraSpringUtility cameraSpringUtility;
    private Transform cameraSpringTransform;

    private void Start()
    {
        cameraSpringUtility = new CameraSpringUtility(Frequence,Damp);
        cameraSpringTransform = transform;
    }

    private void Update()
    {
        cameraSpringUtility.UpdateSpring(Time.deltaTime, Vector3.zero);
        cameraSpringTransform.localRotation = Quaternion.Slerp(cameraSpringTransform.localRotation,
            Quaternion.Euler(cameraSpringUtility.Values),Time.deltaTime*10);
    }

    public void StartCameraSpring()
    {
        cameraSpringUtility.Values = new Vector3(0,
            UnityEngine.Random.Range(MinRecoilRange.x,MaxRecoilRange.x),
            UnityEngine.Random.Range(MinRecoilRange.y, MaxRecoilRange.y));
    }
}
