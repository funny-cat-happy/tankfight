using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IDragHandler
{
    // Start is called before the first frame update
    private RectTransform _touchposition;
    private RectTransform _control;
    private RectTransform _panel;
    private RectTransform _background;
    public Vector2 JoyStickTouchPosition;
    public static JoyStick Instance;
    public bool JoyStickUp;
    public bool JoyStickDown;
    public bool JoyStickHold;
    private void Awake()
    {
        Instance = this;
    }

    // private void Update()
    // {
    //     if (JoyStickTouchPosition == Vector2.zero)
    //     {
    //         up_position.Up = true;
    //     }
    // }

    void Start()
    {
        _control = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        _touchposition = transform.GetComponent<RectTransform>();
        _panel = transform.GetComponentInParent<RectTransform>();
        _background = transform.GetChild(0).GetComponent<RectTransform>();
        _background.gameObject.SetActive(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _background.gameObject.SetActive(true);
        Vector2 localposition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_touchposition, eventData.position,
            eventData.pressEventCamera, out localposition);
        _background.localPosition = localposition;
        JoyStickDown = true;
        JoyStickUp = false;
        JoyStickHold = false;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _control.localPosition = Vector2.zero;
        _background.gameObject.SetActive(false);
        JoyStickTouchPosition = Vector2.zero;
        JoyStickDown = false;
        JoyStickUp = true;
        JoyStickHold = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localposition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_background, eventData.position,
            eventData.pressEventCamera, out localposition);
        if (localposition.magnitude>120)
        {
            _control.localPosition = localposition.normalized * 120;
        }
        else
        {
            _control.localPosition = localposition;
        }
        JoyStickTouchPosition = localposition.normalized;
        JoyStickHold = true;
    }
    
}
