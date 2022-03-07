using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance;
    public Action<float> EnemyDamageChange;
    public Action<NormalTank.MoveStatus> EnemyMoveStatus;
    private void Awake()
    {
        Instance = this;
    }


    public void EnemyDamageChangeExecute(float damage)
    {
        DataManager.Instance.currentNormalTankData.damage = damage;
        if (EnemyDamageChange !=null)
        {
            EnemyDamageChange(damage);
        }
    }

    public void EnemyMoveStatusChangeExecute(NormalTank.MoveStatus moveStatus)
    {
        if (EnemyMoveStatus !=null)
        {
            EnemyMoveStatus(moveStatus);
        }
    }
}
