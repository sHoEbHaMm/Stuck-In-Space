using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rbd;
    AudioSource audioSource;
    [SerializeField] private ParticleSystem Booster;
    [SerializeField] private ParticleSystem LeftThruster;
    [SerializeField] private ParticleSystem RightThruster;
    [SerializeField] private AudioClip ThrustSound;
    [SerializeField] private float ThrustForce = 100f;
    [SerializeField] private float RotationSpeed = 100f;
    
    private void Awake()
    {
        rbd = GetComponent<Rigidbody>(); 
        audioSource = GetComponent<AudioSource>();       
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        Booster.Stop();
    }

    private void StartThrusting()
    {
        rbd.AddRelativeForce(Vector3.up * Time.deltaTime * ThrustForce, ForceMode.Impulse);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(ThrustSound);
        }
        if (!Booster.isPlaying)
        {
            Booster.Play();
        }
    }

    void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if(Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    private void StopRotating()
    {
        RightThruster.Stop();
        LeftThruster.Stop();
    }

    private void RotateRight()
    {
        if (!LeftThruster.isPlaying)
        {
            LeftThruster.Play();
        }
        ApplyRotation(-RotationSpeed);
    }

    private void RotateLeft()
    {
        if (!RightThruster.isPlaying)
        {
            RightThruster.Play();
        }
        ApplyRotation(RotationSpeed);
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rbd.freezeRotation = true;
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);
        rbd.freezeRotation = false;
    }
}
