
using UnityEngine;

public class AlertZeroState : BaseAlertState
{
    public AlertZeroState()
    {
        AudioManager.Instance.PlayBackGroundSound(0);
        MapManager.Instance.CreatePlayer();
        MapManager.Instance.BornMachineInit();
    }
    
    
}
