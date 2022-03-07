using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirdropAirport : MonoBehaviour
{
    public Vector2 DropPosition;
    private Vector2 _targetPosition;
    private float speed = 0.03f;
    private GameObject _normalTank;
    private bool isCanPut=true;

    private void Awake()
    {
        _normalTank = Resources.Load<GameObject>(FilePath.TankPath+"NormalTank");
    }

    void Start()
    {
        AudioManager.Instance.PlaySound("helicopter",AudioManager.AudioKind.tip);
        _targetPosition=new Vector2(transform.position.x,-12.5f);
    }
    
    void Update()
    {
        AirportMove();
    }

    private void AirportMove()
    {
        if (transform.position.y <= _targetPosition.y)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, speed);
            if (transform.position.y<=DropPosition.y)  //到达投放位置
            {
                if (isCanPut)
                {
                    Instantiate(_normalTank, transform.position, Quaternion.Euler(0, 0, 0));
                    AudioManager.Instance.PlaySound("totheground",AudioManager.AudioKind.tip);
                    isCanPut = false;
                }
            }
        }
    }
}
