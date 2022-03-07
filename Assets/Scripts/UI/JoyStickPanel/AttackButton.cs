using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine.EventSystems;
using UnityEngine;

public class AttackButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public static AttackButton Instance;
    public bool isAttackButtonDown;
    private void Awake()
    {
        Instance = this;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        isAttackButtonDown = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isAttackButtonDown = false;
    }
}
