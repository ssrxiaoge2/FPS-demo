using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPController : MonoBehaviour
{
    public float Gravity = 9.8f;//重力
    public float JumpHeight = 2f;//跳跃高度

    #region =========== 实例化对象 ===========
    private Animator characterAnimator;
    private CharacterController characterController;
    public Transform characterTransform;//获取当前位置
    #endregion

    private float velocity;

    #region =========== 移动速度相关 ===========
    private Vector3 MovementDirection;//移动方向
    public float MovementSpeed=6f;//移动速度
    public float runSpeed = 9f;
    public float walkSpeed = 6f;
    public float CrouchedSpeed = 3f;//蹲下移动速度
    #endregion
    
    #region =========== 角色蹲下相关 ===========
    private bool isCrouched;//判断是否蹲下
    public float CrouchHeight = 1f;//下蹲高度
    private float originHeight;//初始高度
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        //characterAnimator = GetComponentInChildren<Animator>();
        characterTransform = transform;
        originHeight = characterController.height;//获取初始高度
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    /// <summary>
    /// 移动
    /// </summary>
    public void Move()
    {
        if (characterController.isGrounded)//判断是否在地面
        {

            float tmp_Horizontal = Input.GetAxis("Horizontal");
            float tmp_Vertical = Input.GetAxis("Vertical");
            MovementDirection =
            characterTransform.TransformDirection(new Vector3(tmp_Horizontal, 0, tmp_Vertical)).normalized;
            //跳跃高度
            MovementDirection.y = Input.GetButtonDown("Jump") ? JumpHeight : MovementDirection.y;
            if(Input.GetKeyDown(KeyCode.C))
            {
                float tmp_CurrentHeight = isCrouched ? originHeight : CrouchHeight;
                StartCoroutine(DoCrouch(tmp_CurrentHeight));
                isCrouched = !isCrouched;
                
            }
            //改变移动速度
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
            velocity = tmp_Velocity.magnitude;//获取长度
            //if(characterAnimator !=null)
                characterAnimator.SetFloat("Velocity", velocity, 0.1f, Time.deltaTime);
        }
        MovementDirection.y -= Gravity * Time.deltaTime;//y轴上增加向下的力
        characterController.Move(MovementSpeed * Time.deltaTime * MovementDirection);
    }
    /// <summary>
    /// 协程蹲下
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
