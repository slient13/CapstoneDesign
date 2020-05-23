using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjBase : MonoBehaviour
{
    //디버그용
    public float ydegree;

    //오브젝트 설정
    public GameObject obj;
    public float degreeSize = 90f;
    public float rotatespeed = 1f;

    private Vector3 rotation;
    private float degree = 0f;
    private bool isRotate = false;

    // Start is called before the first frame update
    void Start()
    {
        if(obj == null)
        {
            Debug.Log("오브젝트가 설정되지 않았습니다!");
        }
        else
        {
            rotation = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ydegree = obj.transform.localEulerAngles.y;     //디버그용

        /*
        //임시 인풋
        if(Input.GetKeyDown(KeyCode.Space))
            ObjRotation();
        */

        obj.transform.localRotation = Quaternion.Lerp(obj.transform.localRotation, Quaternion.Euler(rotation), rotatespeed*Time.deltaTime);
        //obj.transform.localEulerAngles = Vector3.Lerp(obj.transform.localEulerAngles, rotation, rotatespeed*Time.deltaTime);



    }

    //오브젝트 회전
    /// <summary>
    /// 로컬Y좌표로 degree 만큼 회전
    /// </summary>
    /// <param name="degree"></param>
    void ObjRotation()
    {
        IsRotate();

        if (!isRotate)
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

    //돌아가는 중인지 확인
    /// <summary>
    /// 다 돌아갔으면 false, 돌아가는 중이면 true 출력
    /// </summary>
    void IsRotate()
    {
        if (Mathf.Approximately(Mathf.Round(obj.transform.localEulerAngles.y), degree)|| Mathf.Approximately(Mathf.Round(obj.transform.localEulerAngles.y), 360f))
            isRotate = false;
        else  
            isRotate = true;

        if (Mathf.Approximately(Mathf.Round(obj.transform.localEulerAngles.y), 360f) || Mathf.Approximately(Mathf.Round(obj.transform.localEulerAngles.y), 0f))
            obj.transform.localRotation = Quaternion.identity;
    }
}
