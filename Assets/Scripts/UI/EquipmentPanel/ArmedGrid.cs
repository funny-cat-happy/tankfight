using UnityEngine;
using UnityEngine.EventSystems;
using Image = UnityEngine.UI.Image;

public class ArmedGrid : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private GameObject _RightPanel;
    private GameObject currentDragObject;
    private Image ArmImage;
    private GameObject _ArmIcon;
    private Transform _EquipmentPanel;
    private void Awake()
    {
        _RightPanel = GameObject.Find("RightPanel");
        ArmImage = transform.GetChild(0).GetComponent<Image>();
        _EquipmentPanel = GameObject.Find("EquipmentPanel").transform;
        _ArmIcon = Resources.Load<GameObject>("Prefabs/UI/ArmIcon");
    }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
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
        if (eventData.pointerCurrentRaycast.gameObject.name=="RightPanel"||eventData.pointerCurrentRaycast.gameObject.name=="ArmGridImg")
        {
            for (int i = 0; i < _RightPanel.transform.childCount; i++)
            {
                if (_RightPanel.transform.GetChild(i).GetChild(0).GetComponent<Image>().overrideSprite==ArmImage.overrideSprite)
                {
                    _RightPanel.transform.GetChild(i).gameObject.SetActive(true);
                    ArmImage.color = new Color(32/255.0f, 30/255.0f, 30/255.0f);
                    ArmImage.overrideSprite = null;
                    break;
                }
            }

            if (transform.name=="SecondaryArm1")
            {
                BagManager.Instance.ArmId[0] = 0;
            }
            if (transform.name=="SecondaryArm2")
            {
                BagManager.Instance.ArmId[1] = 0;
            }

            BagManager.Instance.isHeavyArm = false;
        }
        Destroy(currentDragObject);
    }
}
