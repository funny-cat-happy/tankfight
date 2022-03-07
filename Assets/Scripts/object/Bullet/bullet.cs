using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    protected Rigidbody2D _rigidbody2D;
    public float speed;
    protected Animator _animator;
    protected CauseDamage _causedamage;
    protected virtual void Awake()
    {
        _rigidbody2D = transform.GetComponent<Rigidbody2D>();
        _animator = transform.GetComponent<Animator>();
        _causedamage = transform.GetComponent<CauseDamage>();
    }
    
    void Update()
    {
        BulletFly();
    }

    private void BulletFly()
    {
        switch (transform.rotation.eulerAngles.z)
        {
            case 0:
                _rigidbody2D.velocity = new Vector2(0, speed);
                break;
            case 180:
                _rigidbody2D.velocity = new Vector2(0, -speed);
                break;
            case 270:
                _rigidbody2D.velocity = new Vector2(speed,0);
                break;
            case 90:
                _rigidbody2D.velocity = new Vector2( -speed,0);
                break;
        }
    }

    
    
}
