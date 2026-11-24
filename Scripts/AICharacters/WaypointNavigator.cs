using UnityEngine;

public class WaypointNavigator : MonoBehaviour
{
    [Header("AI Character")]
    public CharacterNavigatorScript character;
    public WayPoint currentWaypoint;
    int direction;

    void Awake()
    {
        character = GetComponent<CharacterNavigatorScript>();
    }

    void Start()
    {
        direction = Random.Range(0, 2);
        MoveToNextWaypoint();
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