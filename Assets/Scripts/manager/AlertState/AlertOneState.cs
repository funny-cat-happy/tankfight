using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertOneState : BaseAlertState
{
    private float nexttime=0;
    private float time = 0;
    private int _bombNumber;
    public AlertOneState()
    {
        AudioManager.Instance.PlayBackGroundSound(1);
        MapManager.Instance.CreateEnergyTower();
        MapManager.Instance.CreateAirdropAirport(3);
    }

    public override void RunState()
    {
        time = time + Time.deltaTime;
        if (time-nexttime>10)
        {
            _bombNumber = Random.Range(3, 6);
            MapManager.Instance.BombAirportAttack(_bombNumber);
            _bombNumber = Random.Range(3, 6);
            MapManager.Instance.BombAirportAttack(_bombNumber);
            nexttime = time;
        }
    }

    public override void ExitState()
    {
        
    }
    
}
