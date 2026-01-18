using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CartMovement : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float speed;

    [Header("Waypoints")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();
    [SerializeField] private int waypointIndex;

    private void Start()
    {
        waypointIndex = 0;
    }

    private void Update()
    {
        ChangePosToWaypoint(waypointIndex);
    }

    public void MoveToWaypoint(int Pos)
    {
        waypointIndex += Pos;
        waypointIndex = Mathf.Clamp(waypointIndex, 0, waypoints.Count - 1);

        Debug.Log(waypointIndex);
    }

    private void ChangePosToWaypoint(int Index)
    {
        Vector3 dir = waypoints[waypointIndex].position - transform.position;

        float angleZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler(0f, 0f, angleZ);

        transform.position = Vector3.MoveTowards(transform.position, waypoints[Index].position, speed * Time.deltaTime);
    }
}
