using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SpawnZombieCoroutine());
    }
    public void SpawnZombie(Vector3 posToSpawn)
    {
        ZombieCtrl zombie = PoolManager.Instance.Pop("Zombie1") as ZombieCtrl;
        zombie.transform.position = posToSpawn;
    }

    IEnumerator SpawnZombieCoroutine()
    {
        float wait = 5f;
        while (true)
        {
            yield return new WaitForSeconds(wait);
            Vector3 posToSpawn = transform.position;
            SpawnZombie(posToSpawn);
            wait = Random.Range(4f, 8f);
        }
    }
}
