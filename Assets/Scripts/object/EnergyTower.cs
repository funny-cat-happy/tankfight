using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyTower : MonoBehaviour
{
    private Vector2 _targetPosition;
    public float health;
    public float speed;
    private DamageAble _damageAble;
    private bool isMoveFinish=false;
    private Animator _animator;
    private Transform[] _playerTank;
    private CauseDamage _causeDamage;
    public float _damagenumber;

    private void Awake()
    {
        _damageAble = transform.GetComponent<DamageAble>();
        _animator = transform.GetComponent<Animator>();
        _causeDamage = transform.GetComponent<CauseDamage>();
        _playerTank = GameObject.Find("Player1").transform.GetChild(0).GetComponentsInChildren<Transform>();
    }

    void Start()
    {
        _damageAble.health = health;
        _damageAble.OnDead += Dead;
        _damageAble.isProtected = true;
        _causeDamage.damage = _damagenumber;
        _targetPosition = new Vector2(transform.position.x, -9.5f);
        _damageAble.isProtected = false;
    }
    
    void Update()
    {
        // TowerFall();
    }

    private void TowerFall()
    {
        if (isMoveFinish) return;
        if (transform.position.Equals(_targetPosition))
        {
            AudioManager.Instance.PlaySound("totheground",AudioManager.AudioKind.tip);
            isMoveFinish = true;
            _damageAble.isProtected = false;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, speed);
        }
    }
    
    private void Dead()
    {
        _animator.SetTrigger("dead");
        AudioManager.Instance.PlaySound("spark",AudioManager.AudioKind.tip);
        foreach (var eve in _playerTank)
        {
            _causeDamage.OnDamage(eve.gameObject);
        }
        Destroy(gameObject,0.6f);
    }
}
