using UnityEngine;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    public DataManager.WindStormTankData windStormTankData;
    private GameObject TankName;
    private GameObject RightPanel;
    private GameObject Grid;

    private DataManager.TankArmData _tankArmData;
    // Start is called before the first frame update
    private void Awake()
    {
        TankName = GameObject.Find("TankName");
        RightPanel = GameObject.Find("RightPanel");
        Grid= Resources.Load<GameObject>("Prefabs/UI/ArmGrid");
    }

    void Start()
    {
        windStormTankData = DataManager.Instance.ReadJsonData<DataManager.WindStormTankData>("Data/PlayerTank/WindStormTank");
        TankName.GetComponent<Text>().text = windStormTankData.TankName;
        _tankArmData=DataManager.Instance.ReadJsonData<DataManager.TankArmData>("Data/PlayerTank/TankArm");
        foreach (DataManager.TankArmItem eve in _tankArmData.TankArm)
        {
            if (eve.ArmOwner==windStormTankData.id)
            {
                GameObject _gameObject=GameObject.Instantiate(Grid);
                ArmGrid _armGrid = _gameObject.GetComponent<ArmGrid>();
                GameObject imgGameObject=_gameObject.transform.GetChild(0).gameObject;
                Image _image = imgGameObject.GetComponent<Image>();
                _armGrid.id = eve.id;
                _armGrid.ArmName = eve.ArmName;
                _armGrid.isHeavyArm = eve.isHeavyArm;
                _armGrid.ArmIntroduction = eve.ArmIntroduction;
                _image.overrideSprite=Resources.Load<Sprite>("Icon/"+eve.id);
                _image.color = new Color(255, 255, 255);
                _gameObject.transform.SetParent(RightPanel.transform);
            }
        }
    }
}
