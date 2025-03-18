using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    public SteeringBehaviour steeringAgent;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float sightDistance;
    private void Awake()
    {
        steeringAgent = GetComponent<SteeringBehaviour>();
    }

    //Two rays that launch diagonally from the character towards its forward direction
    void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward + Vector3.right) * sightDistance, Color.blue);
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward + Vector3.left) * sightDistance, Color.red);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + Vector3.right), out RaycastHit hitInfo, sightDistance, layer))
        {
            steeringAgent.wallInWay = true;
            steeringAgent.rightFeelerDetection = true;
            steeringAgent.leftFeelerDetection = false;

        }
    
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward + Vector3.left), out RaycastHit hitInfo2, sightDistance, layer))
        {
            steeringAgent.wallInWay = true;
            steeringAgent.leftFeelerDetection = true;
            steeringAgent.rightFeelerDetection = false;

        }
        else
        {
            steeringAgent.wallInWay = false;
            steeringAgent.rightFeelerDetection = false;
            steeringAgent.leftFeelerDetection = false;
        }
    }
}
