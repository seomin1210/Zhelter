using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float hp = 100;
    public float maxHp = 100;
    public float regeneration = 2;

    public float moveSpeed = 10f;
    public float turnSpeed = 80f;

    public bool isDead = false;

    private Transform _playerTransform;
    private Animation _anim;

    void Start()
    {
        _playerTransform = GetComponent<Transform>();
        _anim = GetComponent<Animation>();
        _anim.Play("Idle");
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Mouse X");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        moveDir.Normalize();

        _playerTransform.Translate(moveDir * moveSpeed * Time.deltaTime);
        _playerTransform.Rotate(Vector3.up * r * turnSpeed * Time.deltaTime);

        PlayerAnim(h, v);
    }

    private void PlayerAnim(float h, float v)
    {
        if (h <= -0.1f)
            _anim.CrossFade("RunL", 0.25f);
        else if (h >= 0.1f)
            _anim.CrossFade("RunR", 0.25f);
        else if (v <= -0.1f)
            _anim.CrossFade("RunB", 0.25f);
        else if (v >= 0.1f)
            _anim.CrossFade("RunF", 0.25f);
        else
            _anim.CrossFade("Idle", 0.25f);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "Hand")
        {
            ZombieCtrl zombie = col.transform.GetComponentInParent<ZombieCtrl>();
            hp -= zombie.damage;
            UIManager.Instance.HpUIUpdate(hp / maxHp);
            if (hp <= 0)
            {
                isDead = true;
                GameManager.Instance.GameQuit();
            }
        }
    }
}
