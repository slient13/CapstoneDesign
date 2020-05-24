using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{

    public Animator animator;
    public Rigidbody rigidbody;

    private float h;
    private float v;

    //X축으로 이동할 거리
    private float moveX;
    //Y축으로 이동할 거리
    private float moveZ;
    //실직적으로 좌,우로 이동하는 스피드
    private float speedH = 80f;
    //실직적으로 앞,뒤로 이동하는 스피드
    private float speedZ = 100f;

    //어떠한 오브젝트를 들고있는지 아닌지를 판단
    private bool got = false;

    private float speed = 10.0f;
    private float V;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        V = Input.GetAxis("Vertical");
        // 스페이스를 꾹누르면 점프모션이 계속 생긴다 따라서 GetKeyDown을 사용.
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //-1은 기본적인 레이어 베이스, 0은 시간차를 없애기 위함.
            animator.Play("JUMP00", -1, 0);
            SoundManager.instance.PlaySE("Jump");
        }
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        //사용자가 직접 수치를 입력해서 이동 시키는 함수
        animator.SetFloat("h", h);
        animator.SetFloat("v", v);

        moveX = h * speedH * Time.deltaTime;
        moveZ = v * speedZ * Time.deltaTime;

        //뒤로가는 키를 누르면 옆으로 이동 못하게 하는 식
        if(moveZ <=0 )
        {
            moveX = 0;
        }
        rigidbody.velocity = new Vector3(moveX, 0, moveZ);

        //아이템 줍기 
        if(Input.GetKey(KeyCode.Z))
        {
            GameObject chan = GameObject.Find("Cube") as GameObject;
            if(got == false)
            {
                chan.transform.parent = this.transform;
                got = true;
            }else
            {
                chan.transform.parent = null;
                got = false;
            }
            SoundManager.instance.PlaySE("Item_Get");
        }

        /*
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        */
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (v >= 0)
            {
                this.transform.Rotate(0, -speed * 10 * Time.deltaTime, 0);
            }else
            {
                this.transform.Rotate(0, speed * 10 * Time.deltaTime, 0);
            }

        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (v >= 0)
            {
                this.transform.Rotate(0, speed *10 * Time.deltaTime, 0);
            }
            else
            {
                this.transform.Rotate(0, -speed * 10 * Time.deltaTime, 0);
            }
        }
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
            SoundManager.instance.PlaySE("Walk");
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            SoundManager.instance.PlaySE("Walk");
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            SoundManager.instance.PlaySE("Walk");
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            SoundManager.instance.PlaySE("Walk");
        }
    }
//충돌이 발생하는 시점에서 발생
private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Cube")
        {
            Debug.Log("충돌 감지");
            /*
            animator.Play("DAMAGED01");
            this.transform.Translate(Vector3.back * speedZ * Time.deltaTime);
             */
         }

    }
    //충돌이 발생하면서 계속해서 발생
    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.tag == "Cube")
        {
            Debug.Log("충돌 유지");
        }
    }
    //충돌이 끝나면서 발생
    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Cube")
        {
            Debug.Log("충돌 종료");
        }
    }
}
