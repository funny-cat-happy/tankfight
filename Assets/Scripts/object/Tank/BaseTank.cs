using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BaseTank : MonoBehaviour
{
    public float speed;
    public float heath;
    protected Rigidbody2D _rigidbody2D;
    protected SpriteRenderer _spriteRenderer;
    private Transform bullet_position;
    public float AttackGap;
    private bool AttackStatus=true;
    protected GameObject TankBullet;
    private Animator _animator;
    protected DamageAble _damageAble;
    private BoxCollider2D _boxCollider2D;
    protected enum TankStatus
    {
        born,
        norml,
        dead
    }
    protected float BulletDamage;
    protected TankStatus CurrentTankStatus;
    protected virtual void Awake()
    {
        //组件初始化
        _rigidbody2D = transform.GetComponent<Rigidbody2D>();
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
        bullet_position = transform.Find("bullet_position");
        _animator = transform.GetComponent<Animator>();
        _damageAble = transform.GetComponent<DamageAble>();
        _boxCollider2D = transform.GetComponent<BoxCollider2D>();
        CurrentTankStatus = TankStatus.born;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _damageAble.OnDead += TankDead;
        _damageAble.health = heath;
        _damageAble.isProtected = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (CurrentTankStatus==TankStatus.born)
        {
            return;
        }
        if (CurrentTankStatus==TankStatus.norml)
        {
            TankMove();
            TankAttack();
        }
        
    }

    protected virtual void TankMove()
    {
        if (playerinput.Instance.up_position.Down)
        {
            _rigidbody2D.velocity = new Vector2(0, speed);
            transform.rotation=Quaternion.Euler(0,0,0);
        }
        if (playerinput.Instance.down_position.Down)
        {
            _rigidbody2D.velocity = new Vector2(0, -speed);
            transform.rotation=Quaternion.Euler(0,0,180);
        }
        
        if (playerinput.Instance.left_position.Down)
        {
            _rigidbody2D.velocity = new Vector2(-speed, 0);
            transform.rotation=Quaternion.Euler(0,0,90);
        }
        
        if (playerinput.Instance.right_position.Down)
        {
            _rigidbody2D.velocity = new Vector2(speed,0);
            transform.rotation=Quaternion.Euler(0,0,-90);
        }
        
        if (!playerinput.Instance.up_position.Hold&&!playerinput.Instance.down_position.Hold&&!playerinput.Instance.left_position.Hold&&!playerinput.Instance.right_position.Hold)
        {
            _rigidbody2D.velocity = new Vector2(0, 0);
        }
        //移动端移动
        // if (JoyStick.Instance.JoyStickTouchPosition.x>-0.7f&&JoyStick.Instance.JoyStickTouchPosition.x<0.7&&JoyStick.Instance.JoyStickTouchPosition.y>0f&&JoyStick.Instance.JoyStickTouchPosition.y<1)
        // {
        //     _rigidbody2D.velocity = new Vector2(0, speed);
        //     transform.rotation=Quaternion.Euler(0,0,0);
        // }
        // if (JoyStick.Instance.JoyStickTouchPosition.x>-0.7f&&JoyStick.Instance.JoyStickTouchPosition.x<0.7f&&JoyStick.Instance.JoyStickTouchPosition.y>-1&&JoyStick.Instance.JoyStickTouchPosition.y<0)
        // {
        //     _rigidbody2D.velocity = new Vector2(0, -speed);
        //     transform.rotation=Quaternion.Euler(0,0,180);
        // }
        //
        // if (JoyStick.Instance.JoyStickTouchPosition.x>0&&JoyStick.Instance.JoyStickTouchPosition.x<1&&JoyStick.Instance.JoyStickTouchPosition.y>-0.7f&&JoyStick.Instance.JoyStickTouchPosition.y<0.7f)
        // {
        //     _rigidbody2D.velocity = new Vector2(speed, 0);
        //     transform.rotation=Quaternion.Euler(0,0,-90);
        // }
        //
        // if (JoyStick.Instance.JoyStickTouchPosition.x>-1&&JoyStick.Instance.JoyStickTouchPosition.x<0&&JoyStick.Instance.JoyStickTouchPosition.y>-0.7f&&JoyStick.Instance.JoyStickTouchPosition.y<0.7f)
        // {
        //     _rigidbody2D.velocity = new Vector2(-speed,0);
        //     transform.rotation=Quaternion.Euler(0,0,90);
        // }
        //
        // if (JoyStick.Instance.JoyStickTouchPosition==Vector2.zero)
        // {
        //     _rigidbody2D.velocity = new Vector2(0, 0);
        // }
    }

    private void TankAttack()
    {
        if (AttackStatus==false)
        {
            return;
        }

        if (playerinput.Instance.fire.Down)
        {
            GameObject gameObject = GameObject.Instantiate(TankBullet);
            gameObject.GetComponent<WindTankBullet>().damage = BulletDamage;
            gameObject.transform.position = bullet_position.position;
            gameObject.transform.rotation=Quaternion.Euler(0,0,transform.rotation.eulerAngles.z);
            AttackStatus = false;
            AudioManager.Instance.PlaySound("GunFire");
            Invoke("ResetAttackStatus",AttackGap);
        }
        // if (AttackButton.Instance.isAttackButtonDown)
        // {
        //     GameObject gameObject = GameObject.Instantiate(TankBullet);
        //     gameObject.GetComponent<WindTankBullet>().damage = BulletDamage;
        //     gameObject.transform.position = bullet_position.position;
        //     gameObject.transform.rotation=Quaternion.Euler(0,0,transform.rotation.eulerAngles.z);
        //     AttackStatus = false;
        //     AudioManager.Instance.PlaySound("GunFire");
        //     Invoke("ResetAttackStatus",AttackGap);
        // }
    }

    private void ResetAttackStatus()
    {
        AttackStatus = true;
    }

    protected void TankDead()
    {
        CurrentTankStatus = TankStatus.dead;
        _boxCollider2D.enabled = false;
        _rigidbody2D.velocity = new Vector2(0, 0);
        _animator.SetTrigger("dead");
        AudioManager.Instance.PlaySound("TankExplosion");
        transform.position = new Vector3(-5.5f, -9.5f);
    }

    private void TankBornFinish()
    {
        AudioManager.Instance.PlaySound("transport");
        CurrentTankStatus = TankStatus.norml;
        _damageAble.isProtected = false;
    }
    
    
}
