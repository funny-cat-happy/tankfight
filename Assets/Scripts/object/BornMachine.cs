using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BornMachine : MonoBehaviour
{
    private GameObject _normalTank;
    private int MaxEnemyTankNumber;
    private int currentEnemyTankNumber;
    private bool isCanRefresh = false;
    public float RefreshTime ;
    private void Awake()
    {
        _normalTank = Resources.Load<GameObject>(FilePath.TankPath+"NormalTank");
        MaxEnemyTankNumber = MapManager.Instance.MaxEnemyTankNumber;
    }

    private void Start()
    {
        Invoke(nameof(RefreshReset),2.0f);
    }

    private void Update()
    {
        currentEnemyTankNumber = MapManager.Instance.currentEnemyTankNumber;
        if (currentEnemyTankNumber<MaxEnemyTankNumber)
        {
            if (isCanRefresh)
            {
                CreateNormalTank();
                MapManager.Instance.currentEnemyTankNumber++;
                isCanRefresh = false;
                Invoke(nameof(RefreshReset),RefreshTime);
            }
        }
    }

    private void CreateNormalTank()  //创建普通敌人坦克
    {
        GameObject _gameObject = Instantiate(_normalTank);
        _gameObject.transform.position = transform.position;
    }
    private void RefreshReset()  //刷新重置
    {
        isCanRefresh = true;
    }
    
    
    private void PlaySound()
    {
        AudioManager.Instance.PlaySound("TransportReady",AudioManager.AudioKind.tip);
    }
}
