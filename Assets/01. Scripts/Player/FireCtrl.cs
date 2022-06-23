using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public Transform firePos;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void Fire()
    {
        BulletCtrl bulletPrefab = PoolManager.Instance.Pop("Bullet") as BulletCtrl;
        bulletPrefab.SetPositionAndRotation(firePos.position, firePos.rotation);
    }
}
