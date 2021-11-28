using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed; //인스펙터 창에서 설정 가능
    public float jumpHeight; //점프높이 설정


    public List<string> interactionTargetList;

    GameObject nearObject;

    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool iDown;
    bool vDown;
    bool bDown;

    bool isJump;
    bool isDodge;

    string act_command = "None : ";

    Vector3 moveVec;
    Vector3 dodgeVec;//회피 도중 방향전환이 되지 않도록 회피방향 Vector3 추가

    Rigidbody rigid;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //Rigidbody 변수를 초기화 한다.
        anim = GetComponentInChildren<Animator>();

        // 여기에 상호작용이 필요한 오브젝트의 태그를 적으면 된다.
        string[] targetList = {
            "Shop",
            "Vehicle"
        };
        interactionTargetList.AddRange(targetList);

        
    }

    void Start()
    {
        MappingInfo playerControl = new MappingInfo("Player");
        playerControl.AddMapping("Move : ", "!esc");
        playerControl.AddMapping("Jump : ", "space");
        playerControl.AddMapping("Action : ", "mouseL");
        playerControl.AddMapping("Interaction : ", "e");
        playerControl.Enroll();
    }

    void Update()
    {
        GetInput();
        Turn();
        // Dodge(); // 자꾸 벽을 뚫고 들어가는 버그가 있어 비활성화.
    }



    //함수를 기능에 따라 분리 => 이렇게 안하면 나중에 복잡할 수가 있음.
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        //Axis 값을 정수 값으로 받는 함수 -> 키보드로 입력을 받으면 0,1 이런식으로 반환해준다
        wDown = Input.GetButton("Walk");
        //shift는 누를 때만 작동되도록 GetButton() 함수 사용
        vDown = Input.GetButton("OnCollisionEnter");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        //normalized로 모든 방향으로의 벡터 값을 1로 해준다

        if (isDodge)
            moveVec = dodgeVec;
        //회피 중에는 움직임 벡터 => 회피방향 벡터로 바뀌도록 구현 

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        //transform 이동은 꼭 Time.deltaTime까지 곱해야 한다.
        //wDown이 true면 0.3 false면 1

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);//현재 위치에서 나아갈 방향 벡터를 더한 벡터로 회전시켜준다.
        //LookAt()함수는 지정된 벡터를 향해서 회전시켜주는 함수
    }

    //점프
    void Jump()
    {
        // Debug.Log("Player.Jump.debug : is called");
        if (!isJump && !isDodge)
        {
            rigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }

    }

    //회피. 
    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.4f); //Invoke 함수로 시간차를 둔다
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    // 점프 중 오브젝트랑 충돌 시 애니메이션 초기화.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "NotFloor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }

        if (collision.gameObject.name == "Vehicle")
        {
            speed = 30;
  
        }

         //플레이어가 포탈 오브젝트와 충돌시
        if(collision.gameObject.name == "RacingPotal")
        {
            SceneManager.LoadScene("Racing");
        }
    }

    void Action()
    {
        new Message(act_command).FunctionCall();
    }

    void SetAction(Message message)
    {
        this.act_command = (string) message.args[0];
    }

    void Interaction()
    {
        if (nearObject != null && !isJump && !isDodge)
            nearObject.SendMessage("Interaction", this);
    }

    // 인접 오브젝트 확인.
    // 복수의 오브젝트가 충돌 시, 제일 나중에 입력된 태그를 가진 오브젝트가 우선권을 가짐.
    void OnTriggerEnter(Collider other)
    {
        // 태그 검사하고 대상 변경.                
        foreach (string targetTag in interactionTargetList)
            if (other.tag == targetTag)
                nearObject = other.gameObject;
    }

    // 오브젝트와의 충돌이 끝나는 경우 그것이 'nearObject'였다면 초기화.
    void OnTriggerExit(Collider other)
    {
        if (nearObject != null && other.tag == nearObject.tag)
            nearObject = null;
    }
    
    
}
