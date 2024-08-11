using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarAiSpawner : MonoBehaviour
{

    public GameObject[] aiPrefabs;
    public int aiToSpawn;
    public int timer;

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int count = 0;
        while(count < aiToSpawn)
        {
            int randomIndex = Random.Range(0, aiPrefabs.Length);

            GameObject obj = Instantiate(aiPrefabs[randomIndex]);

            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<CarWaypointNavigator>().currentWaypoint = child.GetComponent<WayPoint>();

            obj.transform.position = child.position;

            yield return new WaitForSeconds(timer);

            count ++;
        }
    }
}
