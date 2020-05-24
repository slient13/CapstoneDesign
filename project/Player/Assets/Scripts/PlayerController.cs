using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    //스피드 조정 변수
    //[SerializeField]를 입력해주면 기존의 private의 보안도 지키면서 인스펙터창에서 수정 가능.
    [SerializeField] private float walkSpeed;

    [SerializeField]
    private float runSpeed;
    [SerializeField]
    //앉기 스피드
    private float crouchSpeed;

    private float applySpeed;

    [SerializeField]
    private float jumpForce;

    //상태 변수
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = false;

    //앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    //얼마나 앉을지 결정하는 변수
    private float crouchPosY;
    //원래 상태를 나타내는 변수
    private float originPosY;
    //위의 두가지를 한번에 나타내기 위해 사용하는 변수
    private float applyCrouchPosY;

    //땅 착지 여부
    private CapsuleCollider capsuleCollider;

    //카메라의 민감도를 의미
    [SerializeField] private float lookSensitivity;

    //카메라의 한계
    [SerializeField] 
    //캐릭터가 고개를 들때 그 정도치를 정해주기 위함.
    private float cameraRotationLimit;
    //카메라가 정면을 바라보게끔 설정/0이라고 선언을 안해도 기본값이 0
    private float currentCameraRotationX = 0;

    //필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;

    //플레이어의 실질적인 몸을 의미
    private Rigidbody myRigid;


    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        //Rigidbody 컴포넌트를 myRigid라는 변수에 가져온다.
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        //실제적인 카메라 기준이 아닌 상대적인 카메라 기준을 사용하기 위해 local을 사용
        //초기화
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update() 
    {
        IsGround();
        Tryjump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();

    }
    //앉기 시도
    private void TryCrouch()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    //앉기 동작
    private void Crouch()
    {
        isCrouch = !isCrouch;
        /* 
        위의 문장과 같은 의미
        if (isCrouch)
            isCrouch = false;
        else
            isCrouch = true;
        */
        if(isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
    }
    //부드러운 앉기 동작(카메라 이동)을 위한 함수
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while(_posY != applyCrouchPosY)
        {
            //보관 이용 -> 처음에는 빠르게 증가하다가 목적에 다다를수록 천천히 증가(더 자연스러움)
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            //보관의 단점은 무한 반복하면서 정확한 값을 못나타내고 근사값을 나타낸다는건데 count라는 변수를 줘서 횟수의 제한을 주면 정확한 값을 도출 가능하다.
            if (count > 15)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0f);
    }
    //지면 체크
    private void IsGround()
    {    
        //벡터 대신에 transform.up을 쓸수도 있지만 캡슐기준으로 하면 레이저가 땅을 쏘는게 아니라 위를 쏘는 경우가 생길수 있으므로 항상 땅을 향해서 레어저가 나가도록 벡터 사용
        //캡슐 기준으로 y의 2분의 1만큼 레이저를 쏘면 땅에 닫는다 하지만 계단이나 대각선같은 경사의 지형에서는 오차가 발생할 수 있으므로 0.1f정도의 오차를 준다.
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }
    //점프 시도
    private void Tryjump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }
    //점프
    private void Jump()
    {
        //앉은 상태에서 점프했을 때 다시 서있게 만드는 조건문
        if (isCrouch)
            Crouch();
        myRigid.velocity = transform.up * jumpForce;
    }
    //달리기 시도
    private void TryRun()
    {
       if(Input.GetKey(KeyCode.LeftShift))
    {
        Running();
    }
       if(Input.GetKeyUp(KeyCode.LeftShift))
    {
        RunningCancel();
    }
     }
    //달리기 실행
    private void Running()
{
        //앉은 상태에서 달리기 했을 때 다시 서있게 만드는 조건문
        if (isCrouch)
            Crouch();

        isRun = true;
    applySpeed = runSpeed;
}

    //달리기 취소
private void RunningCancel()
{
    isRun = false;
    applySpeed = walkSpeed;
}
    //움직임 실행
    private void Move()
    {
        //키보드 좌,우 or A,D을 누르면 1,-1이 리턴되면서 _moveDirX에 입력된다.
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        //키보드 상,하 or W,S을 누르면 1,-1이 리턴되면서 _moveDirZ에 입력된다.
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        //좌,우 구분(_moveDirX를 곱해줌으로써)
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        //상,하 구분(_moveDirZ를 곱해줌으로써)
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        //벡터의 연산후에 합이 1이 나오도록 정규화 해주면 유니티에서도 권장하는 방법이고 계산하기도 편해진다.
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        //1초도안 _Velocity 만큼 움직이게 하도록 Tkme.deltaTime을 곱해준다.
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    //좌우 캐릭터 회전
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    //상하 카메라 회전
    private void CameraRotation()
    {
        //마우스는 2차원 이므로 X로 받는다.
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        //마우스를 살짝올렸는데 한번에 바로 치대치만큼 올리지 않고 자연스럽게 올리기 위함.
        float _cameraRotatonX = _xRotation * lookSensitivity;
        //현재의 수치에 더해준다.
        currentCameraRotationX -= _cameraRotatonX;
        //currentCameraRotationX의 값을 -cameraRotationLimit와 cameraRotationLimit사이에 있도록 가둔다.
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);


        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
