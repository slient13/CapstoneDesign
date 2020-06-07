using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjBase : MonoBehaviour
{
    //회전
    public GameObject obj;
    public float degreeSize = 90f;
    public float rotateSpeed = 1f;

    private Vector3 rotation;
    private float degree = 0f;
    private bool isRotate = false;

    //움직임
    public float moveLength = 5f;
    public float moveSpeed = 1f;

    private Vector3 updatedPos;
    private bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        if (IsRightPos())
            updatedPos = obj.transform.localPosition;
        else
        {
            updatedPos = Vector3.zero;
            Debug.Log(obj.name + " 의 위치가 유효하지 않아 0으로 초기화함");
        }


        rotation = Vector3.zero;
        if(!Mathf.Approximately(obj.transform.localRotation.y % 90f,0f))
        {
            obj.transform.localRotation = Quaternion.identity;
            Debug.Log(obj.name + "오브젝트의 회전각이 맞지않아 0으로 초기화함");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //오브젝트 회전
        obj.transform.localRotation = Quaternion.Lerp(obj.transform.localRotation, Quaternion.Euler(rotation), rotateSpeed*Time.deltaTime);
        //obj.transform.localEulerAngles = Vector3.Lerp(obj.transform.localEulerAngles, rotation, rotatespeed*Time.deltaTime);

        //오브젝트 이동
        obj.transform.position = Vector3.Lerp(obj.transform.position, updatedPos, moveSpeed*Time.deltaTime);
    }

    //현재 위치가 moveLength와 비교하여 유효한지 검사하는 메소드
    /// <summary>
    /// 유효하다면 true, 유효하지 않다면 false 반환
    /// </summary>
    /// <returns></returns>
    bool IsRightPos()
    {
        if (Mathf.Approximately(obj.transform.localPosition.x % moveLength, 0f) && Mathf.Approximately(obj.transform.localPosition.y % moveLength, 0f))
            return true;
        else
            return false;
    }

    //돌아가는 중인지 확인
    /// <summary>
    /// 다 돌아갔으면 false, 돌아가는 중이면 true 출력
    /// </summary>
    void IsRotate()
    {
        if (Mathf.Approximately(Mathf.Round(obj.transform.localEulerAngles.y), degree) || Mathf.Approximately(Mathf.Round(obj.transform.localEulerAngles.y), 360f))
            isRotate = false;
        else
            isRotate = true;

        if (Mathf.Approximately(Mathf.Round(obj.transform.localEulerAngles.y), 360f) || Mathf.Approximately(Mathf.Round(obj.transform.localEulerAngles.y), 0f))
            obj.transform.localRotation = Quaternion.identity;
    }


    //오브젝트 이동중인지 확인
    /// <summary>
    /// 다 움직이면 false, 움직이는 중이면 true 출력
    /// </summary>
    void IsMoving()
    {
        if ( (obj.transform.position-updatedPos).sqrMagnitude >= 0.1f)
            isMoving = true;
        else
            isMoving = false;
    }

    //오브젝트 이동
    /// <summary>
    /// 오브젝트를 Vec3방향으로 *moveLength만큼 움직임
    /// </summary>
    /// <param name="tf"></param>
    void ObjMove(Vector3 posDirection)
    {
        IsRotate();
        IsMoving();

        if (!isRotate && !isMoving)
        {
            Debug.Log("posDir 입력받음");
            updatedPos = new Vector3(obj.transform.position.x + posDirection.x*moveLength, obj.transform.position.y + posDirection.y * moveLength, obj.transform.position.z + posDirection.z * moveLength);
        }
        else
            Debug.Log("오브젝트가 움직이는 중입니다!");
    }

    //오브젝트 회전
    /// <summary>
    /// 로컬Y좌표로 degree 만큼 회전
    /// </summary>
    /// <param name="degree"></param>
    void ObjRotation()
    {
        IsRotate();
        IsMoving();

        if (!isRotate && !isMoving)
        {
            degree += degreeSize;
            if (degree >= 360)
            {
                degree = 0;
                //obj.transform.localRotation = Quaternion.identity;
            }
            rotation = new Vector3(obj.transform.localEulerAngles.x, obj.transform.localEulerAngles.y + degreeSize, obj.transform.localEulerAngles.z);
        }
        else
            Debug.Log("물체가 다 돌아갈때까지 기다리세요!");
    }
}
