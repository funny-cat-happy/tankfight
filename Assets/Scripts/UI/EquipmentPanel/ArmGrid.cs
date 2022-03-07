using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class ArmGrid : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{
    private GameObject _MainPanel;
    private Transform _TipPanel;
    public int id;
    public string ArmName;
    public bool isHeavyArm;
    public string ArmIntroduction;
    private Image ArmImage;
    private GameObject _ArmIcon;
    private Transform _EquipmentPanel;
    private GameObject currentDragObject;
    private void Awake()
    {
        ArmImage = transform.GetChild(0).GetComponent<Image>();
        _EquipmentPanel = GameObject.Find("EquipmentPanel").transform;
        _ArmIcon = Resources.Load<GameObject>("Prefabs/UI/ArmIcon");
        _MainPanel = GameObject.Find("EquipmentPanel");
        _TipPanel = _MainPanel.transform.Find("TipPanel");
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        _TipPanel.gameObject.SetActive(false);
        currentDragObject = GameObject.Instantiate(_ArmIcon);
        currentDragObject.transform.SetParent(_EquipmentPanel);
        Image _image=currentDragObject.GetComponent<Image>();
        _image.overrideSprite = ArmImage.overrideSprite;
        _image.color = new Color(255, 255, 255);
        currentDragObject.transform.position = eventData.position;
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        currentDragObject.transform.position = eventData.position;
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        Image _targetImage = eventData.pointerCurrentRaycast.gameObject.GetComponent<Image>();
        if (eventData.pointerCurrentRaycast.gameObject.name=="SecondaryArm1Img"||eventData.pointerCurrentRaycast.gameObject.name=="SecondaryArm2Img")
        {
            if (_targetImage.overrideSprite==null&&BagManager.Instance.isHeavyArm==false)
            {
                if (!isJudgeGridNull()&&isHeavyArm)
                {
                    Destroy(currentDragObject);
                    return;
                }
                if (eventData.pointerCurrentRaycast.gameObject.name=="SecondaryArm1Img")
                {
                    BagManager.Instance.ArmId[0] = id;
                }
                else
                {
                    BagManager.Instance.ArmId[1] = id;
                }

                if (isHeavyArm)
                {
                    BagManager.Instance.isHeavyArm = isHeavyArm;
                }
                _targetImage.overrideSprite = ArmImage.overrideSprite;
                _targetImage.color = new Color(255, 255, 255);
                gameObject.SetActive(false);
            }
        }
        Destroy(currentDragObject);
    }
    private bool isJudgeGridNull()
    {
        for (int i = 0; i < 2; i++)
        {
            if (BagManager.Instance.ArmId[i]!=0)
            {
                return false;
            }
        }
        return true;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        _TipPanel.gameObject.SetActive(true);
        _TipPanel.position = eventData.position;
        _TipPanel.transform.GetChild(0).GetComponent<Text>().text = ArmName;
        _TipPanel.transform.GetChild(1).GetComponent<Text>().text = ArmIntroduction;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _TipPanel.gameObject.SetActive(false);
    }
}
