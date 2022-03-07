using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainArm : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private GameObject _MainPanel;
    private Transform _TipPanel;
    private void Awake()
    {
        _MainPanel = GameObject.Find("EquipmentPanel");
        _TipPanel = _MainPanel.transform.Find("TipPanel");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        _TipPanel.gameObject.SetActive(true);
        _TipPanel.position = eventData.position;
        _TipPanel.transform.GetChild(0).GetComponent<Text>().text =
            _MainPanel.GetComponent<MainPanel>().windStormTankData.SkillName;
        _TipPanel.transform.GetChild(1).GetComponent<Text>().text =
            _MainPanel.GetComponent<MainPanel>().windStormTankData.SkillIntroduction;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _TipPanel.gameObject.SetActive(false);
    }
}
