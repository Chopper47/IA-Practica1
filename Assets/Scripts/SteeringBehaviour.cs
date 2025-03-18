using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class SteeringBehaviour : MonoBehaviour
{
    [SerializeField] protected float maxSpeed = 1;
    [SerializeField] protected float linearSpeed = 0.5f, angularSpeed = 25f;
    [SerializeField] protected Vector3 velocity, acceleration, position;
    public bool wallInWay;
    public bool rightFeelerDetection;
    public bool leftFeelerDetection;

    [SerializeField] protected bool dodging;
    protected Rigidbody rb;

    public Transform target;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    protected void Start()
    {
        velocity = new Vector3(0f, transform.position.y, 0f);
        acceleration = Vector3.zero;
        position = transform.position;
    }

    /// <summary>
    /// Gets the desired position to steer towards
    /// </summary>
    /// <param name="targetPosition"></param>
    protected virtual void GetSteering(Vector3 targetPosition)
    {
        Vector3 velocityToApply = targetPosition - position;
        velocityToApply.Normalize();
        velocityToApply *= linearSpeed;
        acceleration += velocityToApply;

    }

    /// <summary>
    /// Applies the steering
    /// </summary>
    protected virtual void Steer()
    {
        
        velocity = Vector3.ClampMagnitude(velocity + acceleration, maxSpeed);
        position += velocity * Time.deltaTime;
        acceleration = Vector3.zero;
        transform.position = position;
    }

    /// <summary>
    /// While the raycast detects a wall, rotates little by little. Changes the target of the steering to straight forward while it rotates and steers towards it while doing so
    /// </summary>
    protected void DodgeObstacle()
    {
        dodging = true;
        Quaternion targetRotation = Quaternion.identity;
        Vector3 newTarget = Vector3.zero;
        if (rightFeelerDetection && !leftFeelerDetection)
        {
            targetRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 1f, transform.rotation.z);
            

        }
        else if (leftFeelerDetection && !rightFeelerDetection)
        {
            targetRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y - 1f, transform.rotation.z);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        newTarget = transform.position + transform.forward * Time.deltaTime * linearSpeed;

        GetSteering(newTarget);
        Steer();
        dodging = false;
        
    }

    /// <summary>
    /// Rotates towards the specified target using the angular speed
    /// </summary>
    /// <param name="targetPosition"></param>
    protected void RotateTowardsTarget(Vector3 targetPosition)
    {
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetPosition - transform.position, 50f * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    /// <summary>
    /// Rotates towards the specified target using the angular speed
    /// </summary>
    /// <param name="targetPosition"></param>
    protected void RotateAwayFromTarget(Vector3 targetPosition)
    {
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, transform.position - targetPosition, 50f * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
