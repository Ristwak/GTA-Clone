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
        direction = Random.Range(0, 2);
        if (car != null && currentWaypoint != null)
            car.LocateDestination(currentWaypoint.GetPosition());
    }

    void Update()
    {
        if (car == null || currentWaypoint == null || !car.destinationReached)
            return;
        MoveToNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        WayPoint nextWaypoint = direction == 0 ? currentWaypoint.nextWaypoint : currentWaypoint.previousWaypoint;
        if (nextWaypoint == null)
        {
            direction = 1 - direction;
            nextWaypoint = direction == 0 ? currentWaypoint.nextWaypoint : currentWaypoint.previousWaypoint;
        }
        if (nextWaypoint == null)
            return;
        currentWaypoint = nextWaypoint;
        car.LocateDestination(currentWaypoint.GetPosition());
    }
}