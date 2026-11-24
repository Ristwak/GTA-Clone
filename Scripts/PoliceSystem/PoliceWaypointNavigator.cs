using UnityEngine;

public class PoliceWaypointNavigator : MonoBehaviour
{
    [Header("AI Character")]
    public PoliceOfficer character;
    public WayPoint currentWaypoint;
    int direction;

    void Awake()
    {
        character = GetComponent<PoliceOfficer>();
    }

    void Start()
    {
        direction = Random.Range(0, 2);
        if (character != null && currentWaypoint != null)
            character.LocateDestination(currentWaypoint.GetPosition());
    }

    void Update()
    {
        if (character == null || currentWaypoint == null || !character.destinationReached)
            return;
        MoveToNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        if (currentWaypoint == null || character == null)
            return;
        WayPoint nextWaypoint = direction == 0 ? currentWaypoint.nextWaypoint : currentWaypoint.previousWaypoint;
        if (nextWaypoint == null)
        {
            direction = 1 - direction;
            nextWaypoint = direction == 0 ? currentWaypoint.nextWaypoint : currentWaypoint.previousWaypoint;
        }
        if (nextWaypoint == null)
            return;
        currentWaypoint = nextWaypoint;
        character.LocateDestination(currentWaypoint.GetPosition());
    }
}