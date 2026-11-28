using System.Collections;
using UnityEngine;

public class CarAiSpawner : MonoBehaviour
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
            Debug.LogError("CarAiSpawner: Missing AI prefabs or spawn points.", this);
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
                Debug.LogError("CarAiSpawner: Selected AI prefab is missing.", this);
                yield break;
            }
            CarWaypointNavigator navigator = prefab.GetComponent<CarWaypointNavigator>();
            if (navigator == null)
            {
                Debug.LogError("CarAiSpawner: AI prefab is missing CarWaypointNavigator.", prefab);
                yield break;
            }
            int spawnIndex = Random.Range(0, transform.childCount);
            Transform child = transform.GetChild(spawnIndex);
            WayPoint waypoint = child.GetComponent<WayPoint>();
            if (waypoint == null)
            {
                Debug.LogError("CarAiSpawner: Spawn point is missing WayPoint.", child);
                yield break;
            }
            GameObject obj = Instantiate(prefab);
            obj.transform.position = child.position;
            CarWaypointNavigator spawnedNavigator = obj.GetComponent<CarWaypointNavigator>();
            spawnedNavigator.currentWaypoint = waypoint;
            count++;
            if (count < aiToSpawn)
                yield return new WaitForSeconds(timer);
        }
    }
}