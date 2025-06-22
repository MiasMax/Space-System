using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public Vector3 initialVelocity;

    private Vector3 velocity;
    private Rigidbody rb;

    // Liste statique partagée de tous les corps avec gravité
    public static List<GravityBody> allBodies = new List<GravityBody>();

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        velocity = initialVelocity;

        allBodies.Add(this);
    }

    void OnDestroy()
    {
        allBodies.Remove(this);
    }

    void FixedUpdate()
    {
        Vector3 acceleration = Vector3.zero;

        foreach (GravityBody other in allBodies)
        {
            if (other != this)
                acceleration += ComputeAcceleration(other);
        }

        velocity += acceleration * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    Vector3 ComputeAcceleration(GravityBody other)
    {
        Vector3 direction = other.rb.position - rb.position;
        float distanceSqr = direction.sqrMagnitude;

        if (distanceSqr == 0f)
            return Vector3.zero;

        Vector3 forceDir = direction.normalized;
        float forceMagnitude = PhysicsConstants.G * other.GetComponent<Rigidbody>().mass / distanceSqr;

        return forceDir * forceMagnitude;
    }
}
