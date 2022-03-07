using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine.EventSystems;
using UnityEngine;

public class SkillButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public static SkillButton Instance;
    public bool isSkillButtonDown;
    private void Awake()
    {
        Instance = this;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isSkillButtonDown = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isSkillButtonDown = false;
    }
}