using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    private GameObject GameButton;
    private Button StartGameButton;
    private Transform _EquipmentPanel;
    private void Awake()
    {
        GameButton = GameObject.Find("GameButton");
        StartGameButton = transform.GetComponent<Button>();
        _EquipmentPanel = GameButton.transform.parent.Find("EquipmentPanel");
    }

    // Start is called before the first frame update
    void Start()
    {
        StartGameButton.onClick.AddListener(onButtonClick);
    }
    
    private void onButtonClick()
    {
        GameButton.SetActive(false);
        _EquipmentPanel.gameObject.SetActive(true);
    }
}
