using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAirport : MonoBehaviour
{
    public float AirportSpeed=0.03f;
    public int AirportLoad;
    private int CurrentAirportLoad;
    private Vector3 CurrentAirportPosition;
    private bool isAttack = false;
    private float AttackPosition;
    private GameObject bomb;
    private Vector2 _targetPosition;
    private void Awake()
    {
        bomb = Resources.Load<GameObject>("Prefabs/BlackBomb");
    }

    
    void Start()
    {
        AttackPosition = 15.0f / AirportLoad;
        CurrentAirportLoad = AirportLoad;
        AudioManager.Instance.PlaySound("BombAirport",AudioManager.AudioKind.tip);
        _targetPosition = new Vector2(transform.position.x, -12.5f);
    }

    
    void Update()
    {
        AirportMove();
        BombLaunch();
    }

    private void AirportMove()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, AirportSpeed);
        if (transform.position.y <= -12.5f)
        {
            Destroy(gameObject);
        }
    }

    private void BombLaunch()
    {
        if (CurrentAirportLoad>0)
        {
            if (isAttack)
            {
                CurrentAirportPosition = transform.position;
                GameObject gameObject = Instantiate(bomb);
                gameObject.transform.position = CurrentAirportPosition;
                CurrentAirportLoad--;
                isAttack = false;
            }
            if (transform.position.y<3.5-AttackPosition*(AirportLoad-CurrentAirportLoad))
            {
                isAttack = true;
            }
        }
    }
}
