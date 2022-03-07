using System;
using UnityEngine;

public class CarriageAirport : MonoBehaviour
{
    private float _putTower=-1.0f;
    private GameObject _energyTower;
    public float speed=0.03f;
    private bool isCanPut = true;
    private Vector2 _targetPosition;

    private void Awake()
    {
        _energyTower = Resources.Load<GameObject>("Prefabs/EnergyTower");
    }

    private void Start()
    {
        AudioManager.Instance.PlaySound("helicopter",AudioManager.AudioKind.tip);
        _targetPosition=new Vector2(transform.position.x, 8.5f);
    }
    
    void Update()
    {
        AirportMove();
    }

    private void AirportMove()
    {
        if (transform.position.Equals(_targetPosition))
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _targetPosition, speed);
            if (transform.position.y>=_putTower)  //到达投放位置
            {
                if (isCanPut)
                {
                    Instantiate(_energyTower, transform.position, Quaternion.Euler(0, 0, 0));
                    isCanPut = false;
                }
            }
        }
    }
}
