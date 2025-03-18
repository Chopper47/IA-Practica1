using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Seek : SteeringBehaviour
{    
    
    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (wallInWay) DodgeObstacle();
            else
            {
                GetSteering(target.position);
                RotateTowardsTarget(target.position);
                Steer();

            } 

        }
    }
}
