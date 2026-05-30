using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointNode : MonoBehaviour
{
    [Header("Speed set once we reach waypopoint")]
    public float maxSpeed = 0;

    [Header("Это точка пути, к которой мы идем, еще не достигнута.")]
    public float minDistanceToReachWaypoint = 5;

    public WaypointNode[] nextWaypointNode;
}
