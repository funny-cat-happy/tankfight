using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AlertManager : MonoBehaviour
{
    public static AlertManager Instance;
    //UI
    private GameObject _alertPanel;
    private Sprite[] _NumbersImage;
    private Transform _NumObject;
    private Transform _CirCleObject;
    private float currentTime=0;
    private Image CircleImage;
    private Image NumberImage;
    private int PerStageTime=10;
    private int CurrentStage=0;
    private BaseAlertState _currentAlertState;
    private void Awake()
    {
        Instance = this;
        _alertPanel=GameObject.Find("AlertPanel");
        _NumbersImage = Resources.LoadAll<Sprite>("UI/number");
        _NumObject = _alertPanel.transform.GetChild(1).GetChild(1);
        _CirCleObject = _alertPanel.transform.GetChild(1).GetChild(0);
        CircleImage = _CirCleObject.GetComponent<Image>();
        NumberImage = _NumObject.GetComponent<Image>();
    }

    private void Start()
    {
        _currentAlertState = new AlertZeroState();
    }
    
    private void Update()
    {
        AlertRatePush();
        _currentAlertState.RunState();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void AlertRatePush()
    {
        if (CurrentStage>7)
        {
            return;
        }
        if (currentTime<PerStageTime)
        {
            currentTime += Time.deltaTime;
            CircleImage.fillAmount = currentTime/PerStageTime;
        }
        else
        {
            _currentAlertState.ExitState();
            currentTime = 0;
            AudioManager.Instance.PlaySound("warning",AudioManager.AudioKind.tip);
            NumberImage.sprite = _NumbersImage[++CurrentStage];
            StateChange(CurrentStage);
        }
    }

    private void StateChange(int stage)
    {
        switch (stage)
        {
            case 1:
                _currentAlertState = new AlertOneState();
                break;
            case 2:
                _currentAlertState = new AlertTwoState();
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
        }
    }
}
