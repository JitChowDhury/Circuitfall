using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    public Transform[] GetWayPoints() => waypoints;
}
