using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackToGame : MonoBehaviour,IPointerDownHandler
{
    private GameObject _canvas;
    private Transform _GameButton;
    private Transform _EquipmentPanel;
    private void Awake()
    {
        _canvas=GameObject.Find("Canvas");
        _GameButton = _canvas.transform.Find("GameButton");
        _EquipmentPanel = transform.parent;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _GameButton.gameObject.SetActive(true);
        _EquipmentPanel.gameObject.SetActive(false);
    }
}
