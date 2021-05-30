using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControlable : Controlable
{
    public Transform CharBody;
    public Transform CameraArmSocket;
    public Transform CameraArm;

    Animator animator;
    Rigidbody rigidbody;

    public float Speed; //인스펙터 창에서 설정 가능
    public float JumpForce = 5f;

    public List<string> interactionTargetList;

    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool iDown;
    bool vDown;
    bool bDown;

    bool isVehicle;
    bool isJump;
    bool isDodge;

    Vector3 moveVec;
    Vector3 dodgeVec;//회피 도중 방향전환이 되지 않도록 회피방향 Vector3 추가

    //상속받은 Controlable 의 함수를 선언
    public override void Interact()
    {
        throw new System.NotImplementedException();
    }

     public override void Jump()
    {
       /*
       if (jDown && moveVec == Vector3.zero && !isJump && !isDodge)
        {
            rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            animator.SetBool("isJump", true);
            animator.SetTrigger("doJump");
            isJump = true;
        }
        */
        rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
    }

     public override void Move(Vector2 input)
    {
        /*
       moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        //normalized로 모든 방향으로의 벡터 값을 1로 해준다

        if (isDodge)
            moveVec = dodgeVec;
        //회피 중에는 움직임 벡터 => 회피방향 벡터로 바뀌도록 구현 

        transform.position += moveVec * Speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        //transform 이동은 꼭 Time.deltaTime까지 곱해야 한다.
        //wDown이 true면 0.3 false면 1

        animator.SetBool("isRun", moveVec != Vector3.zero);
        animator.SetBool("isWalk", wDown);
        */
        animator.SetFloat("MoveSpeed", input.magnitude);
        Vector3 LookForward = new Vector3(CameraArm.forward.x, 0f, CameraArm.forward.z).normalized;
        Vector3 LookRight = new Vector3(CameraArm.right.x, 0f, CameraArm.right.z).normalized;
        Vector3 MoveDir = LookForward * input.y + LookRight * input.x;

        if(input.magnitude != 0)
        {
            CharBody.forward = LookForward;
            transform.position += MoveDir * Time.deltaTime * 5f;
        }
    }

     public override void Rotate(Vector2 input)
    {
       //카메라를 회전시키는 기능 구현
       if(CameraArm != null)
        {
           Vector3 CamAngle = CameraArm.rotation.eulerAngles;
           float x = CamAngle.x - input.y;

           if(x < 180f)
           {
               x = Mathf.Clamp(x, -1f, 70f);
           }
           else
           {
               x = Mathf.Clamp(x, 335f, 361f);
           }

           //카메라 앞 회전 시키기
           CameraArm.rotation = Quaternion.Euler(x, CamAngle.y + input.x, CamAngle.z);
       }

    }

    // Start is called before the first frame update
    void Start()
    {
         //animator를 CharBody에서 가져와서 수정.
        animator = CharBody.GetComponent<Animator>();
        //rigidbody를 저장.
        rigidbody = GetComponent<Rigidbody>();
        //상호작용이 필요한 오브젝트의 태그를 적는다.
    }

    void Awake()
    {
        string[] targetList = {
            "Shop",
            "Vehicle"
        };
        interactionTargetList.AddRange(targetList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 
}
