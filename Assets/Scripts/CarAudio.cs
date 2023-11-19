using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudio : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    private float currentSpeed;

    private Rigidbody carRb;
    private AudioSource engineAudio;
    private AudioSource tireAudio;
    [SerializeField] private GameObject tireScreechengineAudio;
    private CarController carController;

    public float minPitch;
    public float maxPitch;
    private float pitchFromCar;

    void Start()
    {
        engineAudio = GetComponent<AudioSource>();
        carRb = GetComponent<Rigidbody>();
        carController = GetComponent<CarController>();
        tireAudio = tireScreechengineAudio.GetComponent<AudioSource>();
    }

    void Update()
    {
        EngineSound();
        TireScreech();
    }

    void EngineSound()
    {
        currentSpeed = carRb.velocity.magnitude;
        pitchFromCar = carRb.velocity.magnitude * 0.06f;

        if (currentSpeed < minSpeed)
        {
            engineAudio.pitch = minPitch;
        }

        if (currentSpeed > minSpeed && currentSpeed < maxSpeed)
        {
            engineAudio.pitch = minPitch + pitchFromCar;
        }

        if (currentSpeed > maxSpeed)
        {
            engineAudio.pitch = maxPitch;
        }
    }

    void TireScreech()
    {
        if (carController.isBreaking && carRb.velocity.magnitude > 10f || carController.isDrifting && carController.horizontalInput != 0 && carRb.velocity.magnitude > 10f)
        {
            if (!tireAudio.isPlaying) 
            {
               tireAudio.Play();    
            }
        }
        else
        {
            if (tireAudio.isPlaying) { tireAudio.Stop(); }
        }
    }
}
