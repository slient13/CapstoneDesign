using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //플레이어 움직임 속도
    public float moveSpeed = 5f;
    //플레이어 방향회전속도
    public float rotationSpeed = 360f;

    CharacterController characterController;
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        //캐릭터 컨트롤러 컴포넌트를 사용
        characterController = GetComponent<CharacterController>();
        //캐릭터 애니메이터 컴포넌트를 사용
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //상하좌우로 움직임
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //부드러운 움직임
        if(direction.sqrMagnitude > 0.01f)
        {
            Vector3 forward = Vector3.Slerp(
                transform.forward,
                direction,
                rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction));

            transform.LookAt(transform.position + forward);
        }

        //점프 구현
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //-1은 기본적인 레이어 베이스, 0은 시간차를 없애기 위함.
            animator.Play("JUMP00", -1, 0);
        }

        //충돌 처리
        characterController.Move(direction * moveSpeed * Time.deltaTime);
        //달리는 애니메이션 추가
        animator.SetFloat("Speed", characterController.velocity.magnitude);
    }
}
