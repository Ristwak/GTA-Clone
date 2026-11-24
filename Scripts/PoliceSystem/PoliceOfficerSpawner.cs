using System.Collections;
using UnityEngine;

public class PoliceOfficerSpawner : MonoBehaviour
{
    public GameObject[] aiPrefabs;
    public int aiToSpawn;
    public int timer;

    void Start()
    {
        if (aiToSpawn <= 0)
            return;
        if (aiPrefabs == null || aiPrefabs.Length == 0 || transform.childCount == 0)
        {
            Debug.LogError("PoliceOfficerSpawner: Missing AI prefabs or spawn points.", this);
            return;
        }
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int count = 0;
        while (count < aiToSpawn)
        {
            int randomIndex = Random.Range(0, aiPrefabs.Length);
            GameObject prefab = aiPrefabs[randomIndex];
            if (prefab == null)
            {
                Debug.LogError("PoliceOfficerSpawner: Selected AI prefab is missing.", this);
                yield break;
            }
            PoliceWaypointNavigator navigator = prefab.GetComponent<PoliceWaypointNavigator>();
            if (navigator == null)
            {
                Debug.LogError("PoliceOfficerSpawner: AI prefab is missing PoliceWaypointNavigator.", prefab);
                yield break;
            }
            int spawnIndex = Random.Range(0, transform.childCount);
            Transform child = transform.GetChild(spawnIndex);
            WayPoint waypoint = child.GetComponent<WayPoint>();
            if (waypoint == null)
            {
                Debug.LogError("PoliceOfficerSpawner: Spawn point is missing WayPoint.", child);
                yield break;
            }
            GameObject obj = Instantiate(prefab);
            obj.transform.position = child.position;
            PoliceWaypointNavigator spawnedNavigator = obj.GetComponent<PoliceWaypointNavigator>();
            spawnedNavigator.currentWaypoint = waypoint;
            count++;
            if (count < aiToSpawn)
                yield return new WaitForSeconds(timer);
        }
    }
}