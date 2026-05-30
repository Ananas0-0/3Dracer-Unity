using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AICarHandler : MonoBehaviour
{
    public enum AIMode { followPlayer, followWayPoints };

    [Header("AI Settings")]
    [SerializeField] private AIMode aiMode;
    [SerializeField] private bool isAvoidingCars = true;
    [SerializeField] private float maxSpeed = 16;
    [SerializeField] private Rigidbody rb;

    //Local variables
    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;

    //WayPoints
    WaypointNode currentWaypoint = null;
    WaypointNode[] allWayPoints;

    //Colliders
    BoxCollider boxCollider;

    //Components
    CarController carController;


    private void Awake()
    {
        carController = GetComponent<CarController>();
        allWayPoints = FindObjectsOfType<WaypointNode>();

        boxCollider = GetComponent<BoxCollider>();
    }

    private void FixedUpdate()
    {
        Debug.Log(rb.velocity.magnitude);
        Vector3 inputVector = Vector3.zero;

        switch (aiMode)
        {
            case AIMode.followPlayer:
                FollowPlayer();
                break;

            case AIMode.followWayPoints:
                FollowWaypoints();
                break;
        }

        inputVector.x = TurnTowardTarget();
        inputVector.z = ApplyThorttleOrBrake(inputVector.x);

        carController.SetInputVector(inputVector);
    }

    private void FollowPlayer() {
        if (targetTransform == null)
        {
            targetTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
        }
    }

    // private void FollowWaypoints() {
    //     if (currentWaypoint == null)
    //         currentWaypoint = FindClosestWayPoint();

    //     if (currentWaypoint != null) {
    //         targetPosition = currentWaypoint.transform.position;

    //         float distanceToWayPoint = (targetPosition - transform.position).magnitude;

    //         if (distanceToWayPoint <= currentWaypoint.minDistanceToReachWaypoint) {
    //             currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0, currentWaypoint.nextWaypointNode.Length)];
    //         }
    //     }
    // }
    private void FollowWaypoints() {
        if (currentWaypoint == null)
            currentWaypoint = FindClosestWayPoint();

        if (currentWaypoint != null) {
            targetPosition = currentWaypoint.transform.position;

            float distanceToWayPoint = (targetPosition - transform.position).magnitude;

            if (distanceToWayPoint <= currentWaypoint.minDistanceToReachWaypoint) {
                if (currentWaypoint.maxSpeed > 0) { maxSpeed = currentWaypoint.maxSpeed; }
                else { maxSpeed = 100; }

                currentWaypoint = currentWaypoint.nextWaypointNode[Random.Range(0, currentWaypoint.nextWaypointNode.Length)];
            }
        }
    }

    // WaypointNode FindClosestWayPoint() {
    //     return allWayPoints
    //         .OrderBy(t => Vector3.Distance(transform.position, t.transform.position))
    //         .FirstOrDefault();
    // }
    WaypointNode FindClosestWayPoint() {
        return allWayPoints
            .OrderBy(t => Vector3.Distance(transform.position, t.transform.position))
            .FirstOrDefault();
    }

    private float TurnTowardTarget()
    {
        Vector3 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();

        if (isAvoidingCars) {
            AvoidCars(vectorToTarget, out vectorToTarget);
        }

        float angleToTarget = Vector3.SignedAngle(transform.forward, vectorToTarget, Vector3.up);
        float steerAmount = angleToTarget / 45.0f;

        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);

        return steerAmount;
    }

    private float ApplyThorttleOrBrake(float inputX) {
        if (rb.velocity.magnitude > maxSpeed) {
            return 0;
        }

        return 1.05f - Mathf.Abs(inputX) / 1.0f;
    }

    // private bool IsCarInFrontOfAICar(out Vector3 position, out Vector3 otherCarRightVector) {
    //     boxCollider.enabled = false;

    //     RaycastHit2D raycastHit2d = Physics2D.CircleCast(transform.position + transform.up * 0.5f, 1.2f, transform.up, 12, 1 << LayerMask.NameToLayer("Car"));

    //     boxCollider.enabled = true;

    //     if (raycastHit2d.collider != null) {
    //         Debug.DrawRay(transform.position, transform.up * 12, Color.red);

    //         position = raycastHit2d.collider.transform.position;
    //         otherCarRightVector = raycastHit2d.collider.transform.right;

    //         return true;
    //     }
    //     else { Debug.DrawRay(transform.position, transform.up * 12, Color.green); }

    //     position = Vector3.zero;
    //     otherCarRightVector = Vector3.zero;

    //     return false;
    // }

    // private void AvoidCars() {
    //     IsCarInFrontOfAICar(out Vector3 otherCarRightVector, out Vector3 otherCarRightVector);
    // }

    private bool IsCarInFrontOfAICar(out Vector3 position, out Vector3 otherCarRightVector) {
        position = Vector3.zero;
        otherCarRightVector = Vector3.zero;

        boxCollider.enabled = false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.up * 0.5f, transform.forward, out hit, 12, 1 << LayerMask.NameToLayer("Car"))) {
            Debug.DrawRay(transform.position, transform.forward * 12, Color.red);

            position = hit.collider.transform.position;
            otherCarRightVector = hit.collider.transform.right;

            boxCollider.enabled = true;
            return true;
        }
        else {
            Debug.DrawRay(transform.position, transform.forward * 12, Color.green);
        }

        boxCollider.enabled = true;
        return false;
    }


    // private void AvoidCars(Vector2 vectorToTarget, out Vectror2 newVectorToTarget) {
    //     if (IsCarInFrontOfAICar(out Vector3 position, out Vector3 otherCarRightVector)) {
    //         Vector2 avoidanceVector = Vector2.zero;

    //         avoidanceVector = Vector2.Reflect((otherCarPosition - transform.position).normalized, otherCarRightVector);

    //         float distanceToTarget = (targetPosition - transform.position).magnitude;

    //         float driveToTargetInfluence = 6.0f / distanceToTarget;

    //         driveToTargetInfluence = Mathf.Clamp(driveToTargetInfluence, 0.30f, 1.0f);

    //         float avoidanceInfluence = 1.0f - driveToTargetInfluence;

    //         newVectorToTarget = vectorToTarget * driveToTargetInfluence + avoidanceVector;
    //         newVectorToTarget.Normalize();

    //         Debug.DrawRay(transform.position, avoidanceVector * 10, Color.blue);
    //         Debug.DrawRay(transform.position, newVectorToTarget * 10, Color.yellow);
    //         return;
    //     }

    //     newVectorToTarget = vectorToTarget;
    // }

    private void AvoidCars(Vector3 vectorToTarget, out Vector3 newVectorToTarget) {
        if (IsCarInFrontOfAICar(out Vector3 position, out Vector3 otherCarRightVector)) {
            Vector3 avoidanceVector = Vector3.zero;

            Vector3 otherCarPosition = position; // Предполагаем, что это ваша переменная для позиции другой машины

            avoidanceVector = Vector3.Reflect((otherCarPosition - transform.position).normalized, otherCarRightVector);

            float distanceToTarget = (targetPosition - transform.position).magnitude;

            float driveToTargetInfluence = 6.0f / distanceToTarget;

            driveToTargetInfluence = Mathf.Clamp(driveToTargetInfluence, 0.30f, 1.0f);

            float avoidanceInfluence = 1.0f - driveToTargetInfluence;

            newVectorToTarget = vectorToTarget * driveToTargetInfluence + avoidanceVector * avoidanceInfluence;
            newVectorToTarget.Normalize();

            Debug.DrawRay(transform.position, avoidanceVector * 10, Color.blue);
            Debug.DrawRay(transform.position, newVectorToTarget * 10, Color.yellow);
            return;
        }

        newVectorToTarget = vectorToTarget;
    }



}