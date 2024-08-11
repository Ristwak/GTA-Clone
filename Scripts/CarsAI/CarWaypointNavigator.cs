using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarWaypointNavigator : MonoBehaviour
{
    [Header("AI car")]
    public CarNavigatorScript car;
    public WayPoint currentWaypoint;
    int direction;

    void Awake()
    {
        car = GetComponent<CarNavigatorScript>();
    }

    void Start()
    {
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));
        car.LocateDestination(currentWaypoint.GetPosition());
    }

    void Update()
    {
        if (car.destinationReached)
        {
            if (currentWaypoint.nextWaypoint != null)
            {
                currentWaypoint = currentWaypoint.previousWaypoint;
            }

            car.LocateDestination(currentWaypoint.GetPosition());
        }
    }
}
