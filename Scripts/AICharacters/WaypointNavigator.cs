using System.Collections;
using System.Collections.Generic;
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
        direction = Mathf.RoundToInt(Random.Range(0f, 1f));
        character.LocateDestination(currentWaypoint.GetPosition());
    }

    void Update()
    {
        if(character.destinationReached)
        {
            if(direction == 0)
            {
                if(currentWaypoint.nextWaypoint != null)
                {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                }
                else
                {
                    currentWaypoint = currentWaypoint.previousWaypoint;
                    direction = 1;
                }
            }
            else if(direction == 1)
            {
                if(currentWaypoint.nextWaypoint != null)
                {
                    currentWaypoint = currentWaypoint.previousWaypoint;
                }
                else
                {
                    currentWaypoint = currentWaypoint.nextWaypoint;
                    direction = 0;
                }
            }

            character.LocateDestination(currentWaypoint.GetPosition());
        }
    }
}
