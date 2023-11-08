using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private bool isDrifting;

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    [SerializeField] private GameObject frontLeftWheelEffectObj, frontRightWheelEffectObj, rearLeftWheelEffectObj, rearRightWheelEffectObj;

    public Vector3 com;
    public Rigidbody rb;
    public float antiRoll;

    public static int currentSpeed;
    public static int maxSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com;
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        ApplyAntiRoll();
        ApplyTireFriction();
        UpdateWheels();

    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
        
        // Drifiting Input
        isDrifting = Input.GetMouseButton(0); // 0 corresponds to the left mouse button
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
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void ApplyAntiRoll() 
    {
        WheelHit hit;
        float travelL = 1.0f;
        float travelR = 1.0f;

        bool groundedL = frontLeftWheelCollider.GetGroundHit(out hit);
        if (groundedL)
            travelL = (-frontLeftWheelCollider.transform.InverseTransformPoint(hit.point).y - frontLeftWheelCollider.radius) / frontLeftWheelCollider.suspensionDistance;

        bool groundedR = frontRightWheelCollider.GetGroundHit(out hit);
        if (groundedR)
            travelR = (-frontRightWheelCollider.transform.InverseTransformPoint(hit.point).y - frontRightWheelCollider.radius) / frontRightWheelCollider.suspensionDistance;

       float antiRollForce = (travelL - travelR) * antiRoll;

        if (groundedL)
            rb.AddForceAtPosition(frontLeftWheelCollider.transform.up * -antiRollForce,
                  frontLeftWheelCollider.transform.position);
        if (groundedR)
            rb.AddForceAtPosition(frontRightWheelCollider.transform.up * antiRollForce,
                   frontRightWheelCollider.transform.position);
    }

    private void ApplyTireFriction() 
    {
        float frictionStiffness = 1.0f;

       // Debug.Log(isDrifting);

        if (isDrifting)
        {
            frictionStiffness = 0.3f;
        }

        WheelFrictionCurve frictionFL = frontLeftWheelCollider.sidewaysFriction;
        WheelFrictionCurve frictionFR = frontRightWheelCollider.sidewaysFriction;
        WheelFrictionCurve frictionRL = rearLeftWheelCollider.sidewaysFriction;
        WheelFrictionCurve frictionRR = frontRightWheelCollider.sidewaysFriction;

        frictionFL.stiffness = frictionStiffness;
        frictionFR.stiffness = frictionStiffness;
        frictionRL.stiffness = frictionStiffness;
        frictionRR.stiffness = frictionStiffness;

        frontLeftWheelCollider.sidewaysFriction = frictionFL;
        frontRightWheelCollider.sidewaysFriction = frictionFR;
        rearLeftWheelCollider.sidewaysFriction = frictionRL;
        rearRightWheelCollider.sidewaysFriction = frictionRR;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);

        ApplyWheelEffects();
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void ApplyWheelEffects()
    {
        if (isBreaking || isDrifting && horizontalInput != 0)
        {
            rearLeftWheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
            rearRightWheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
        }
        else 
        {
            rearLeftWheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
            rearRightWheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
        }
    }


}