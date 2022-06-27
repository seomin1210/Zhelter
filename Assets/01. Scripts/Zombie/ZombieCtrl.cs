using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieCtrl : PoolableMono
{
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.TRACE;
    public int hp = 10;
    public int damage = 15;
    public float traceDist = 1000f;
    public float attackDist = 2f;
    public bool isDead = false;

    private Transform _zombieTransform;
    private Transform _targetTransform;
    public NavMeshAgent _agent;
    private Animator _anim;

    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashDie1 = Animator.StringToHash("Die1");
    private readonly int hashDie2 = Animator.StringToHash("Die2");

    private void Awake()
    {
        _zombieTransform = GetComponent<Transform>();
        _targetTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _anim = GetComponent<Animator>();

    }
    private IEnumerator CheckZombieState()
    {
        while (isDead == false)
        {
            yield return new WaitForSeconds(0.3f);

            if (isDead == true)
                yield break;
            if (state == State.DIE)
                yield break;

            float distance = Vector3.Distance(_zombieTransform.position, _targetTransform.position);

            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }

    private void Update()
    {
        if(_agent.remainingDistance >= 2f)
        {
            Vector3 direction = _agent.desiredVelocity;

            Quaternion rotation = Quaternion.LookRotation(direction);

            _zombieTransform.rotation = Quaternion.Slerp(_zombieTransform.rotation, rotation, Time.deltaTime * 10f);
        }
    }

    private IEnumerator ZombieAction()
    {
        SphereCollider[] spheres = GetComponentsInChildren<SphereCollider>();
        while (isDead == false)
        {
            switch (state)
            {
                case State.IDLE:
                    _agent.isStopped = true;
                    _anim.SetBool(hashTrace, false);
                    break;
                case State.TRACE:
                    _agent.SetDestination(_targetTransform.position);
                    _agent.isStopped = false;
                    _anim.SetBool(hashTrace, true);
                    _anim.SetBool(hashAttack, false);
                    foreach (SphereCollider sphere in spheres)
                    {
                        if (sphere.tag == "Hand")
                            sphere.enabled = false;
                    }
                    break;
                case State.ATTACK:
                    _anim.SetBool(hashAttack, true);
                    foreach (SphereCollider sphere in spheres)
                    {
                        if (sphere.tag == "Hand")
                            sphere.enabled = true;
                    }
                    break;
                case State.DIE:
                    isDead = true;
                    _agent.isStopped = true;
                    int random = Random.Range(0, 2);
                    if (random == 0) _anim.SetTrigger(hashDie1);
                    else _anim.SetTrigger(hashDie2);
                    GetComponent<CapsuleCollider>().enabled = false;
                    foreach (SphereCollider sphere in spheres)
                    {
                        sphere.enabled = false;
                    }
                    GameManager.Instance.AddMoney(10);
                    yield return new WaitForSeconds(3f);
                    StopAllCoroutines();
                    this.gameObject.SetActive(false);
                    PoolManager.Instance.Push(this);
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
    public void Hit(int damage)
    {
        _anim.SetTrigger(hashHit);
        hp -= damage;
        if(hp <= 0)
        {
            state = State.DIE;
        }
    }

    public override void Reset()
    {
        state = State.TRACE;
        isDead = false;
        GetComponent<CapsuleCollider>().enabled = true;
        SphereCollider[] spheres = GetComponentsInChildren<SphereCollider>();
        foreach (SphereCollider sphere in spheres)
        {
            if (sphere.tag == "Hand")
                sphere.enabled = false;
            else if (sphere.tag == "Head")
                sphere.enabled = true;
        }
        StartCoroutine(CheckZombieState());

        StartCoroutine(ZombieAction());
        _agent.enabled = false;
    }

    public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }
}
