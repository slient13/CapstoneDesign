using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour

{
    public GameObject target;
    public GameObject Car;

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentsteerAngle;
    private float currentbreakForce;
    private bool isBreaking;

    private bool state;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private void FixedUpdate()
    {
        //GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        Off();
    }

    void Start()
    {
        MappingInfo mapping = new MappingInfo("Car");
        mapping.AddMapping("Horizontal : 1", "_d"); //RightArrow
        mapping.AddMapping("Horizontal : -1", "_a"); //LeftArrow
        mapping.AddMapping("Horizontal : 0", "!d, !a");
        mapping.AddMapping("Vertical : 1", "_w"); //UpArrow
        mapping.AddMapping("Vertical : -1", "_s"); //DownArrow
        mapping.AddMapping("Vertical : 0", "!w, !s");
        mapping.AddMapping("Break : 1", "_n");
        mapping.AddMapping("Break : 0", "!n");
        mapping.Enroll("PlayerCar");
    }

    public void Horizontal(Message message)
    {
       horizontalInput = (int)message.args[0];
    }

    public void Vertical(Message message)
    {
        verticalInput = (int)message.args[0];
    }

    public void Break(Message message)
    {
        int isBreak = (int)message.args[0];
        if(isBreak == 1)
        {
            isBreaking = true;
        }
        else
        {
            isBreaking = false;
        }

    }

    private void GetInput()
    {
        
            horizontalInput = Input.GetAxis(HORIZONTAL);
            verticalInput = Input.GetAxis(VERTICAL);
            isBreaking = Input.GetKeyDown("n");
      
    }

    //차량 탑승
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown("b"))
            {
                //speed = 30;
                //isVehicle = true;

                //플레이어 오브젝트가 차량의 자식 오브젝트가 된다.
                target.transform.parent = Car.transform;

                motorForce = 1000;

                target.SetActive(false);
                print("사라짐");
                state = false;

                new Message($"ControlManager/LayerChanger : PlayerCar").FunctionCall();
            }
        }
    }

    //차량 하차 
    void Off()
    {

        if (Input.GetKeyDown("v") && state == false)
        {
            target.SetActive(true);
            print("생김");
            state = true;
            //플레이어 오브젝트가 다시 독립적으로 나온다.
            target.transform.parent = null;
            motorForce = 0;

            new Message("ControlManager/LayerChanger : general").FunctionCall();

        }
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
        
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentsteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentsteerAngle;
        frontRightWheelCollider.steerAngle = currentsteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }


}
