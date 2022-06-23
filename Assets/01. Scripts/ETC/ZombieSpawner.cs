using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(SpawnZombieCoroutine());
    }
    public void SpawnZombie()
    {
        ZombieCtrl zombie = PoolManager.Instance.Pop("Zombie1") as ZombieCtrl;
        zombie.transform.position = transform.position;
    }

    IEnumerator SpawnZombieCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            SpawnZombie();
        }
    }
}
