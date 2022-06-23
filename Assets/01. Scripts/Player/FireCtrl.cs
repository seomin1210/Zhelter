using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCtrl : MonoBehaviour
{
    public Transform firePos;

    public int maxAmmo = 510;
    public int hasAmmo = 510;
    public int magazine = 30;
    public int ammo = 30;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    private void Fire()
    {
        if(ammo <= 0)
        {
            ammo = 0;
            return;
        }
        ammo--;
        BulletCtrl bulletPrefab = PoolManager.Instance.Pop("Bullet") as BulletCtrl;
        bulletPrefab.SetPositionAndRotation(firePos.position, firePos.rotation);
    }

    private void Reload()
    {
        if(hasAmmo <= 0)
        {
            return;
        }
        int reload = magazine - ammo;
        if((hasAmmo - reload) <= 0)
        {
            ammo += hasAmmo;
            hasAmmo = 0;
        }
        else
        {
            ammo = magazine;
            hasAmmo -= reload;
        }
    }
    public void RefillAmmo()
    {
        hasAmmo = maxAmmo;
    }
}
