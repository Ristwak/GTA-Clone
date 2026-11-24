using System.Collections;
using UnityEngine;

public class AISpawner : MonoBehaviour
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
            Debug.LogError("AISpawner: Missing AI prefabs or spawn points.", this);
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
                Debug.LogError("AISpawner: Selected AI prefab is missing.", this);
                yield break;
            }
            WaypointNavigator navigator = prefab.GetComponent<WaypointNavigator>();
            if (navigator == null)
            {
                Debug.LogError("AISpawner: AI prefab is missing WaypointNavigator.", prefab);
                yield break;
            }
            int spawnIndex = Random.Range(0, transform.childCount);
            Transform child = transform.GetChild(spawnIndex);
            WayPoint waypoint = child.GetComponent<WayPoint>();
            if (waypoint == null)
            {
                Debug.LogError("AISpawner: Spawn point is missing WayPoint.", child);
                yield break;
            }
            GameObject obj = Instantiate(prefab);
            obj.transform.position = child.position;
            WaypointNavigator spawnedNavigator = obj.GetComponent<WaypointNavigator>();
            spawnedNavigator.currentWaypoint = waypoint;
            count++;
            if (count < aiToSpawn)
                yield return new WaitForSeconds(timer);
        }
    }
}