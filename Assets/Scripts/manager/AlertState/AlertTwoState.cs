using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertTwoState : BaseAlertState
{

    private float nexttime=0;
    private float time = 0;
    public AlertTwoState()
    {
        BuffManager.Instance.EnemyMoveStatus(NormalTank.MoveStatus.hunt);
        AudioManager.Instance.PlayBackGroundSound(6);
    }

    public override void RunState()
    {
        time = time + Time.deltaTime;
        if (time-nexttime>10)
        {
            MapManager.Instance.MissileAttack(10);
            MapManager.Instance.MissileAttack(10);
            nexttime = time;
        }
    }
}
