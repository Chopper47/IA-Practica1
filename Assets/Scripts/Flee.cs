using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : SteeringBehaviour
{
    
    void Update()
    {
        if(target != null)
        {
            if (wallInWay) 
            {
                DodgeObstacle();
            } 
            else
            {
                GetSteering(target.position);
                RotateAwayFromTarget(target.position);
                Steer();

            }

        }
    }

    //Moves away from the target. If it's dodging, it goes back to normal steering.
    protected override void GetSteering(Vector3 targetPosition)
    {
        Vector3 velocityToApply = Vector3.zero;
        if (!dodging)
        {
            velocityToApply = targetPosition + position;

        }
        else
        {
            velocityToApply = targetPosition - position;
        }
        velocityToApply.Normalize();
        velocityToApply *= linearSpeed;
        acceleration += velocityToApply;

    }
}
