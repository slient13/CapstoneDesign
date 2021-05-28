using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleControlable : Controlable
{
    public Transform CameraArmSocket;
    public Transform CharacterSeat;
    public Transform RidePosition; //탑승 위치
    public Transform CameraArm;

    [SerializeField]
    private WheelCollider[] wheelColliders;
    [SerializeField]
    private GameObject[] wheelMeshes;

    [SerializeField]
    private Animator doorAnimator;

    public CharacterControlable driveCharacter;

    //상속받은 Controlable 의 함수를 선언
     public override void Interact()
    {
        throw new System.NotImplementedException();
    }

     public override void Jump()
    {
        //throw new System.NotImplementedException();
        //핸드 브레이크 기능 
    }

    //입력에 따라서 wheelCollider를 통해 움직이도록 구현.
     public override void Move(Vector2 input)
    {
        for(int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 position;
            wheelColliders[i].GetWorldPose(out position, out quat);
            wheelMeshes[i].transform.SetPositionAndRotation(position, quat);
        }

        wheelColliders[0].steerAngle = wheelColliders[1].steerAngle = input.x * 20f;

        for(int i =0; i < 4; i++)
        {
            wheelColliders[i].motorTorque = input.y * (2000f/4);
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
