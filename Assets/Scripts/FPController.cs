using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{
    public float Gravity = 9.8f;//����
    public float JumpHeight = 2f;//��Ծ�߶�

    #region =========== ʵ�������� ===========
    private Animator characterAnimator;
    private CharacterController characterController;
    public Transform characterTransform;//��ȡ��ǰλ��
    #endregion

    private float velocity;

    #region =========== �ƶ��ٶ���� ===========
    private Vector3 MovementDirection;//�ƶ�����
    public float MovementSpeed=6f;//�ƶ��ٶ�
    public float runSpeed = 9f;
    public float walkSpeed = 6f;
    public float CrouchedSpeed = 3f;//�����ƶ��ٶ�
    #endregion
    
    #region =========== ��ɫ������� ===========
    private bool isCrouched;//�ж��Ƿ����
    public float CrouchHeight = 1f;//�¶׸߶�
    private float originHeight;//��ʼ�߶�
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        //characterAnimator = GetComponentInChildren<Animator>();
        characterTransform = transform;
        originHeight = characterController.height;//��ȡ��ʼ�߶�
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    /// <summary>
    /// �ƶ�
    /// </summary>
    public void Move()
    {
        if (characterController.isGrounded)//�ж��Ƿ��ڵ���
        {

            float tmp_Horizontal = Input.GetAxis("Horizontal");
            float tmp_Vertical = Input.GetAxis("Vertical");
            MovementDirection =
            characterTransform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical)).normalized;
            //��Ծ�߶�
            MovementDirection.y = Input.GetButtonDown("Jump") ? JumpHeight : MovementDirection.y;
            if(Input.GetKeyDown(KeyCode.C))
            {
                float tmp_CurrentHeight = isCrouched ? originHeight : CrouchHeight;
                StartCoroutine(DoCrouch(tmp_CurrentHeight));
                isCrouched = !isCrouched;
                
            }
            //�ı��ƶ��ٶ�
            if(isCrouched)
            {
                MovementSpeed = CrouchedSpeed;
            }
            else
            {
                MovementSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
            }
            Vector3 tmp_Velocity = characterController.velocity;
            tmp_Velocity.y = 0;
            velocity = tmp_Velocity.magnitude;//��ȡ����
            //if(characterAnimator !=null)
                characterAnimator.SetFloat("Velocity", velocity, 0.1f, Time.deltaTime);
        }
        MovementDirection.y -= Gravity * Time.deltaTime;//y�����������µ���
        characterController.Move(MovementSpeed * Time.deltaTime * MovementDirection);
    }
    /// <summary>
    /// Э�̶���
    /// </summary>
    /// <param name="_target"></param>
    /// <returns></returns>
    private IEnumerator DoCrouch(float _target)
    {
        float tmp_CurrentHeight = 0;
        while(Mathf.Abs(characterController.height- _target) >0.1f)
        {
            yield return null;
            characterController.height =
                Mathf.SmoothDamp(characterController.height, _target, 
                    ref tmp_CurrentHeight,Time.deltaTime * 5); 
        }
    }
    internal void SetUpAnimator(Animator _animator)
    {
        characterAnimator = _animator;
    }
}
