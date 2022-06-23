using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : PoolableMono
{
    public float force = 1500.0f;
    public int damage = 1;
    private Rigidbody bulletRigidbody = null;
    private Transform bulletTransform = null;
    private ZombieCtrl zombie;


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Body")
        {
            zombie = other.gameObject.GetComponent<ZombieCtrl>();
            zombie.Hit(damage);
            gameObject.SetActive(false);
            PoolManager.Instance.Push(this);
        }
        else if(other.tag == "Head")
        {
            zombie = other.gameObject.GetComponentInParent<ZombieCtrl>();
            zombie.Hit(damage * 2);
            gameObject.SetActive(false);
            PoolManager.Instance.Push(this);
        }
        else
        {
            gameObject.SetActive(false);
            PoolManager.Instance.Push(this);
        }
    }

    public override void Reset()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletTransform = GetComponent<Transform>();
        bulletRigidbody.velocity = Vector3.zero;
    }

    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
        bulletRigidbody.AddForce(bulletTransform.forward * force);
    }
}
