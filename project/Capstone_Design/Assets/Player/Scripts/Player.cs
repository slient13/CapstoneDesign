using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed; //인스펙터 창에서 설정 가능

    GameObject nearObject;

    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool iDown;

    bool isJump;
    bool isDodge;

    Vector3 moveVec;
    Vector3 dodgeVec;//회피 도중 방향전환이 되지 않도록 회피방향 Vector3 추가

    Rigidbody rigid;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>(); //Rigidbody 변수를 초기화 한다.
        anim = GetComponentInChildren<Animator>();
    }

    
    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
        Interation();
       
    }

    //함수를 기능에 따라 분리 => 이렇게 안하면 나중에 복잡할 수가 있음.
    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        //Axis 값을 정수 값으로 받는 함수 -> 키보드로 입력을 받으면 0,1 이런식으로 반환해준다
        wDown = Input.GetButton("Walk");
        //shift는 누를 때만 작동되도록 GetButton() 함수 사용
        jDown = Input.GetButton("Jump");
        //GetButton() 함수로 점프 입력 받기
        iDown = Input.GetButton("Interation");
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

    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }

    }

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

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void Interation()
    {
        if (iDown && nearObject != null && !isJump && !isDodge)
        {
            if (nearObject.tag == "Shop")
            {
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);
            }
        }
    }


    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Shop")
        {
            nearObject = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Shop")
        {
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            nearObject = null;
        }
    }
}
