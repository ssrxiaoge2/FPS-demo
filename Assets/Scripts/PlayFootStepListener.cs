using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayFootStepListener : MonoBehaviour
{
    public FootstepAudioData FootstepAudioData;
    public AudioSource FootstepAudioSource;

    private CharacterController characterController;
    private Transform footstepTransform;

    private float nextPlayTime= 0.2f;
    public enum State
    { 
        Walk,
        Run,
        Crouching
    }
    public State characterState;
    //Q.��ɫ���������ıر�����
    //A.��ɫ�ƶ����߷����ϴ���ȶ���ʱ��������

    //Q.��μ���ɫ�Ƿ����ƶ�
    //A.��physic API���

    //Q.���ʵ�ֽ�ɫ��̤λ�õĶ�Ӧ���ʵ�����
    //A.��physic API���

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        footstepTransform = transform;
    }

    private void FixedUpdate()
    {
       if(characterController.isGrounded)
        {
            if(characterController.velocity.normalized.magnitude>=0.1f)
            {
                nextPlayTime += Time.deltaTime;
                if(characterController.velocity.magnitude>=9)
                {
                    characterState = State.Run;
                }
                else if(characterController.velocity.magnitude >= 6)
                {
                    characterState = State.Walk;
                }
                else
                {
                    characterState = State.Crouching;
                }
                //Debug.Log(characterController.velocity.magnitude);
                bool ishit= Physics.Linecast(footstepTransform.position,
                    Vector3.down* (characterController.height/2+characterController.skinWidth-characterController.center.y),
                    out RaycastHit tmp_Hitinfo);
                if(ishit)
                {
                    
                    //�������
                    foreach(var tmp_AudioElement in FootstepAudioData.FootstepAudios)
                    {
                        if (tmp_Hitinfo.collider.CompareTag(tmp_AudioElement.tag))
                        {
                            float tmp_Delay = 0f;
                            switch(characterState)
                            {
                                case State.Run:
                                    tmp_Delay = tmp_AudioElement.RunDlay;
                                    break;
                                case State.Walk:
                                    tmp_Delay = tmp_AudioElement.Delay;
                                    break;
                                case State.Crouching:
                                    tmp_Delay = tmp_AudioElement.CrouchDelay;
                                    break;
                            }
                            if (nextPlayTime >= tmp_Delay)
                            {
                                //�����ƶ�����
                                int tmp_AudioCount = tmp_AudioElement.AudioClips.Count;
                                int tmp_AudioIndex = UnityEngine.Random.Range(0, tmp_AudioCount);
                                AudioClip tmp_FootstepAudioClip = tmp_AudioElement.AudioClips[tmp_AudioIndex];
                                FootstepAudioSource.clip = tmp_FootstepAudioClip;
                                FootstepAudioSource.Play();
                                nextPlayTime = 0;
                                break; 
                            }
                           
                        }

                    }
                    
                }
            }
        }
    }
}
