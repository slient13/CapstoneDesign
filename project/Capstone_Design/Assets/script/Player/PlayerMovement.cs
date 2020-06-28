using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed; //움직이는 속도의 크기를 저장하는 변수
    Vector3 lookDirection; //Vector3의 lookDirection는 3차원 공간에서의 방향의 정보를 담고있는 변수

    private Rigidbody rigid;

    public int JumpPower;

    private bool IsJumping;
    private bool inAir = false;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        IsJumping = false;
    }

    // Update is called once per frame
    private void Update()
    {
        //if문은 방향키 입력을 받았을 때 실행하라는 의미
       if(Input.GetKey(KeyCode.UpArrow) ||
          Input.GetKey(KeyCode.DownArrow) ||
          Input.GetKey(KeyCode.LeftArrow) ||
          Input.GetKey(KeyCode.RightArrow) )
        {
            //GetAxisRaw("Vertical")은 z축에 대한 값(위: 1, 아래: -1, 이 외: 0)
            //GetAxisRaw("Horizontal")은 x축에 대한 값(오른쪽: 1, 왼쪽: -1, 이 외: 0)으로 방향키를 눌렸을때 값을 가져온다.
            float xx = Input.GetAxisRaw("Vertical");
            float zz = Input.GetAxisRaw("Horizontal");
            //lookDirection은 3차원 공간에서 방향을 나타낸다, 이 변수에 방향키에 대한 방향 정보를 담는다
            //예를 들어 방향키 위로를 누르면 lookDirection = 1 * (0,0,1) + 0 * (1.0.0) = (0,0,1)
            //즉, lookDirection은 +z축을 향하고 있다.
            lookDirection = xx * Vector3.forward + zz * Vector3.right;

            //lookDirection방향으로 물체를 회전시키는 역할
            //this는 자신을 나타낸다 -> 캡슐의 방향을 lookDirection으로 회전시키라는 의미
            this.transform.rotation = Quaternion.LookRotation(lookDirection);
            //회전을 시킨 방향으로 물체를 앞으로 이동시키는 것. 물체가 이미 움직일 방향으로 회전을 했으므로 앞으로 이동하면 된다.
            this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }


        InAir();
        Jump();



    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !inAir)
        {
            rigid.AddForce(Vector3.up * JumpPower, ForceMode.Impulse);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsJumping = false;
        }    
    }
    private void InAir()
    {
        if (this.transform.position.y > 1.5)
            inAir = true;
        else
            inAir = false;
    }

}
