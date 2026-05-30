using System.IO;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Input")]
    private float moveInput;
    private float steerInput;

    [Header("References")]
    [SerializeField] private Transform[] rayPoints;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask drivable;
    [SerializeField] private Transform accelerationPoint;
    [SerializeField] private GameObject[] tires = new GameObject[4];  //private GameObject[] tires = new GameObject[4];
    [SerializeField] private GameObject[] frontTires = new GameObject[2]; //передние   
    [SerializeField] private TrailRenderer[] skidMarks = new TrailRenderer[2];
    [SerializeField] private ParticleSystem[] skidSmokes = new ParticleSystem[2];
    [SerializeField] private AudioSource engineSound;
    [SerializeField] private AudioSource skidSound;

    [Header("Suspension Settings")]
    [SerializeField] private float restLength;
    [SerializeField] private float springTravel;
    [SerializeField] private float springStiffness;
    [SerializeField] private float damperStiffness;
    [SerializeField] private float wheelRadius;

    [Header("Car Settings")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float steerStrength = 15f;
    [SerializeField] private AnimationCurve turningCurve;
    [SerializeField] private float dragCoefficient = 1f;
    [SerializeField] private float brakingDeceleration = 100f;
    [SerializeField] private float brakingDragCoefficient = 0.5f;

    private Vector3 currentCarLocalVelocity;
    private float carVelocityRatio;

    private int[] wheelIsGrounded = new int[4];
    private bool isGrounded = false;

    [Header("Visuals")]
    [SerializeField] private float tyreRotSpeed = 3000f;
    [SerializeField] private float maxSteeringAngle = 30f;
    [SerializeField] private float minSideSkidVelocity = 10f;

    [Header("Audio")]
    [SerializeField][Range(0, 1)] private float minPitch = 1f;
    [SerializeField][Range(1, 5)] private float maxPitch = 5f;


    #region Unity Functions


    private void Update()
    {
        //GetPlayerInput();
    }

    private void FixedUpdate()
    {
        CalculateCarVelocity();
        Suspension();
        GroundCheck();
        Movement();

        // visuals
        Visuals();

        // SFX
        EngineSound();
    }

    // private void OnDestroy()
    // {
    //     if (fileWriter != null)
    //     {
    //         fileWriter.Close();
    //     }
    // }

    #endregion

    #region Input Management

    // private void GetPlayerInput()
    // {
    //     moveInput = inputVector.z;      //moveInput = Input.GetAxis("Vertical");
    //     steerInput = inputVector.x;   //steerInput = Input.GetAxis("Horizontal");
    // }

    public void SetInputVector(Vector3 inputVector) {
        moveInput = inputVector.z;
        steerInput = inputVector.x;
    }

    #endregion

    #region Movement

    private void Movement()
    {
        if (isGrounded)
        {
            Accelerate();
            Decelerate();
            Turn();
            SidewaysDrag();
        }
    }

    private void Accelerate()
    {
        if (currentCarLocalVelocity.z < maxSpeed)
        {
            rb.AddForceAtPosition(acceleration * moveInput * rb.transform.forward, accelerationPoint.position, ForceMode.Acceleration);
        }
        //Debug.Log(currentCarLocalVelocity.z);
    }
    // private void Accelerate() //DEEPSEEK
    // {
    //     if (moveInput == 0) return;
    
    //     float targetSpeed = maxSpeed * Mathf.Abs(moveInput);
    //     if (Mathf.Abs(currentCarLocalVelocity.z) < targetSpeed)
    //     {
    //         float accelerationForce = acceleration * moveInput;
    //         rb.AddForceAtPosition(accelerationForce * rb.transform.forward, 
    //                               accelerationPoint.position, 
    //                               ForceMode.Acceleration);
    //     }
    // }   

    private void Decelerate()
    {
        rb.AddForce((Input.GetKey(KeyCode.KeypadPlus) ? brakingDeceleration : deceleration) * carVelocityRatio * -rb.transform.forward, ForceMode.Acceleration);
    }
    // private void Decelerate() //DEEPSEEK
    // {
    //     bool isBraking = Input.GetKey(KeyCode.KeypadPlus);
    //     float decelerationForce = (isBraking ? brakingDeceleration : deceleration) 
    //                               * carVelocityRatio;
    
    //     rb.AddForce(-decelerationForce * rb.transform.forward, ForceMode.Acceleration);
    // }

    private void Turn()
    {
        rb.AddRelativeTorque(steerStrength * steerInput * turningCurve.Evaluate(Mathf.Abs(carVelocityRatio)) * Mathf.Sign(carVelocityRatio) * rb.transform.up, ForceMode.Acceleration);
    }
    // private void Turn() //DEEPSEEk
    // {
    //     if (Mathf.Abs(carVelocityRatio) < 0.1f) return; // не поворачивать на месте
        
    //     float turnTorque = steerStrength * steerInput 
    //                        * turningCurve.Evaluate(Mathf.Abs(carVelocityRatio)) 
    //                        * Mathf.Sign(carVelocityRatio);
        
    //     rb.AddRelativeTorque(turnTorque * rb.transform.up, ForceMode.Acceleration);
    // }

    private void SidewaysDrag()
    {
        float currentSidewaysSpeed = currentCarLocalVelocity.x;
        float dragForceMagnitude = -currentSidewaysSpeed * (Input.GetKey(KeyCode.KeypadPlus) ? brakingDragCoefficient : dragCoefficient);

        Vector3 dragForce = rb.transform.right * dragForceMagnitude;

        rb.AddForceAtPosition(dragForce, rb.worldCenterOfMass, ForceMode.Acceleration);
    }

    #endregion

    #region Car Status Check

    private void CalculateCarVelocity()
    {
        currentCarLocalVelocity = rb.transform.InverseTransformDirection(rb.velocity);
        carVelocityRatio = currentCarLocalVelocity.z / maxSpeed;
    }

    // some modifications done in ground check function
    // private void GroundCheck()
    // {
    //     int tempGroundedWheels = 0;

    //     for (int i = 0; i < wheelIsGrounded.Length; i++)
    //     {
    //         tempGroundedWheels += wheelIsGrounded[i];
    //     }

    //     if (tempGroundedWheels > 0)
    //     {
    //         isGrounded = true;
    //     }
    //     else
    //     {
    //         isGrounded = false;
    //     }
    // }
    // Текущий код неэффективен. Замените на:
    private void GroundCheck()
    {
        isGrounded = false;
        for (int i = 0; i < wheelIsGrounded.Length; i++)
        {
            if (wheelIsGrounded[i] == 1)
            {
                isGrounded = true;
                return; // выход при первом же касании
            }
        }
    }

    #endregion

    #region Visuals

    private void Visuals()
    {
        TireVisuals();
        VFX();
    }

    private void TireVisuals()
    {
        float steeringAngle = steerInput * maxSteeringAngle;


        for (int i = 0; i < tires.Length; i++)
        {
            if (i < 2)
            {
                tires[i].transform.Rotate(Vector3.right, tyreRotSpeed * carVelocityRatio * Time.deltaTime, Space.Self);

                frontTires[i].transform.localEulerAngles = new Vector3(frontTires[i].transform.localEulerAngles.x, steeringAngle, frontTires[i].transform.localEulerAngles.z);
            }
            else
            {
                tires[i].transform.Rotate(Vector3.right, tyreRotSpeed * moveInput * Time.deltaTime, Space.Self);
            }
        }
    }
    // private void TireVisuals()  //DEEPSEEK
    // {
    //     float steeringAngle = steerInput * maxSteeringAngle;
    //     float rotationSpeed = tyreRotSpeed * Time.deltaTime;
    //     float frontRotation = rotationSpeed * carVelocityRatio;
    //     float rearRotation = rotationSpeed * moveInput;
    
    //     for (int i = 0; i < tires.Length; i++)
    //     {
    //         if (tires[i] == null) continue;
        
    //         if (i < 2) // передние
    //         {
    //             tires[i].transform.Rotate(Vector3.right, frontRotation, Space.Self);
    //             if (frontTires[i] != null)
    //             {
    //                 Vector3 angles = frontTires[i].transform.localEulerAngles;
    //                 frontTires[i].transform.localEulerAngles = new Vector3(angles.x, steeringAngle, angles.z);
    //             }
    //         }
    //         else // задние
    //         {
    //             tires[i].transform.Rotate(Vector3.right, rearRotation, Space.Self);
    //         }
    //     }
    // }

    private void SetTirePosition(GameObject tire, Vector3 targetPosition)
    {
        tire.transform.position = targetPosition;
    }

    private void VFX()
    {
        if (isGrounded && Mathf.Abs(currentCarLocalVelocity.x) > minSideSkidVelocity)
        {
            ToggleSkidMarks(true);
            ToggleSkidSmoke(true);
            ToggleSkidSound(true);
        }
        else
        {
            ToggleSkidMarks(false);
            ToggleSkidSmoke(false);
            ToggleSkidSound(false);
        }
    }

    private void ToggleSkidMarks(bool enable)
    {
        foreach (TrailRenderer skidMark in skidMarks)
        {
            skidMark.emitting = enable;
        }
    }

    private void ToggleSkidSmoke(bool enable)
    {
        foreach (var skidSmoke in skidSmokes)
        {
            if (enable)
            {
                skidSmoke.Play();
            }
            else
            {
                skidSmoke.Stop();
            }
        }
    }

    #endregion

    #region Audio

    private void EngineSound()
    {
        engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.Abs(carVelocityRatio));
    }

    private void ToggleSkidSound(bool enable)
    {
        skidSound.mute = !enable;
    }

    #endregion

    #region Suspension

    public void Suspension()
    {
        for (int i = 0; i < rayPoints.Length; i++)
        {
            RaycastHit hit;
            float maxDistance = restLength;

            if (Physics.Raycast(rayPoints[i].position, -rayPoints[i].up, out hit, maxDistance + wheelRadius, drivable))
            {
                wheelIsGrounded[i] = 1;

                float currentSpringLength = hit.distance - wheelRadius;
                float springCompression = (restLength - currentSpringLength) / springTravel;

                // Calculate damper force (proportional to the velocity of suspension compression)
                float springVelocity = Vector3.Dot(rb.GetPointVelocity(hit.point), rayPoints[i].up);
                float damperForce = damperStiffness * springVelocity;

                float springForce = springCompression * springStiffness;

                float netForce = springForce - damperForce;

                rb.AddForceAtPosition(netForce * rayPoints[i].up, rayPoints[i].position);

                SetTirePosition(tires[i], hit.point + rayPoints[i].up * wheelRadius);

                Debug.DrawLine(rayPoints[i].position, hit.point, Color.red);
            }
            else
            {
                wheelIsGrounded[i] = 0;

                // visuals
                SetTirePosition(tires[i], rayPoints[i].position - rayPoints[i].up * maxDistance);

                Debug.DrawLine(rayPoints[i].position, rayPoints[i].position + (maxDistance + wheelRadius) * -rayPoints[i].up, Color.green);
            }
        }
    }
    // [SerializeField] private float suspensionForceMultiplier = 1f; // добавить DEEPSEEK

    // public void Suspension()
    // {
    //     Vector3 pointVelocity = Vector3.zero;
    
    //     for (int i = 0; i < rayPoints.Length; i++)
    //     {
    //         RaycastHit hit;
    //         float maxDistance = restLength;
    //         Transform rayPoint = rayPoints[i];
        
    //         if (Physics.Raycast(rayPoint.position, -rayPoint.up, out hit, 
    //                             maxDistance + wheelRadius, drivable))
    //         {
    //             wheelIsGrounded[i] = 1;
            
    //             float currentSpringLength = hit.distance - wheelRadius;
    //             float springCompression = Mathf.Clamp01((restLength - currentSpringLength) 
    //                                                  / springTravel);
            
    //             // Кэшируем скорость точки
    //             pointVelocity = rb.GetPointVelocity(hit.point);
    //             float springVelocity = Vector3.Dot(pointVelocity, rayPoint.up);
            
    //             float springForce = springCompression * springStiffness;
    //             float damperForce = damperStiffness * springVelocity;
    //             float netForce = (springForce - damperForce) * suspensionForceMultiplier;
            
    //             rb.AddForceAtPosition(netForce * rayPoint.up, rayPoint.position);
    //             SetTirePosition(tires[i], hit.point + rayPoint.up * wheelRadius);
    //         }
    //         else
    //         {
    //             wheelIsGrounded[i] = 0;
    //             SetTirePosition(tires[i], rayPoint.position - rayPoint.up * maxDistance);
    //         }
    //     }
    // }

    #endregion
}
