using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : SteeringBehaviour
{
    [SerializeField]
    DetectObjects radar;

    public Vector3 FindTheNearestThreat(ICollection<Rigidbody> list)
    {
        Vector3 threatPos = Vector3.zero;

        float shortestTimeToCollide = float.PositiveInfinity;

        foreach(Rigidbody r in list)
        {
            Vector3 relativePos = rb.position - r.position;
            Vector3 relativeVel = rb.velocity - r.velocity;
            float relativeSpeed = relativeVel.magnitude;

            if(relativeSpeed == 0)
            {
                continue;
            }

            float timeToCollide = -1.0f * Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);
            Vector3 separation = relativePos + relativeVel * timeToCollide;

            if(separation.magnitude > r.GetComponent<SphereCollider>().radius + GetComponent<SphereCollider>().radius)
            {
                continue;
            }

            if(timeToCollide > 0 && timeToCollide < shortestTimeToCollide)
            {
                shortestTimeToCollide = timeToCollide;
                threatPos = r.position;
            }
        }

        return threatPos;
    }

    public override Vector3 CalculateSteeringForce()
    {
        Vector3 steer = Vector3.zero;
        Vector3 threatPos = FindTheNearestThreat(radar.Obstacles);

        if(threatPos != Vector3.zero)
        {
            Vector3 dir = rb.position - threatPos;
            Vector3 desiredVelo = dir.normalized * maxSpeed;

            steer = Vector3.ClampMagnitude(desiredVelo - rb.velocity, maxForce);
            steer /= mass;
        }

        return steer;
    }
}
