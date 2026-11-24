using System.Collections;
using UnityEngine;

public class PoliceOfficer2Spawner : MonoBehaviour
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
            Debug.LogError("PoliceOfficer2Spawner: Missing AI prefabs or spawn points.", this);
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
                Debug.LogError("PoliceOfficer2Spawner: Selected AI prefab is missing.", this);
                yield break;
            }
            PoliceOfficer2WaypointNavigator navigator = prefab.GetComponent<PoliceOfficer2WaypointNavigator>();
            if (navigator == null)
            {
                Debug.LogError("PoliceOfficer2Spawner: AI prefab is missing PoliceOfficer2WaypointNavigator.", prefab);
                yield break;
            }
            int spawnIndex = Random.Range(0, transform.childCount);
            Transform child = transform.GetChild(spawnIndex);
            WayPoint waypoint = child.GetComponent<WayPoint>();
            if (waypoint == null)
            {
                Debug.LogError("PoliceOfficer2Spawner: Spawn point is missing WayPoint.", child);
                yield break;
            }
            GameObject obj = Instantiate(prefab);
            obj.transform.position = child.position;
            PoliceOfficer2WaypointNavigator spawnedNavigator = obj.GetComponent<PoliceOfficer2WaypointNavigator>();
            spawnedNavigator.currentWaypoint = waypoint;
            count++;
            if (count < aiToSpawn)
                yield return new WaitForSeconds(timer);
        }
    }
}