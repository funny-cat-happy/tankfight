using System;
using UnityEngine;

public class playerinput : MonoBehaviour
{
    public static playerinput Instance;
    public inputbutton pause = new inputbutton(KeyCode.Escape);
    public inputbutton fire = new inputbutton(KeyCode.J);
    public inputbutton skill = new inputbutton(KeyCode.K);
    public inputbutton up_position = new inputbutton(KeyCode.W);
    public inputbutton down_position = new inputbutton(KeyCode.S);
    public inputbutton left_position = new inputbutton(KeyCode.A);
    public inputbutton right_position = new inputbutton(KeyCode.D);
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        pause.Get();
        fire.Get();
        up_position.Get();
        down_position.Get();
        right_position.Get();
        left_position.Get();
        skill.Get();
    }
    //移动设备调用
    // public inputbutton up_position = new inputbutton(KeyCode.W);
    // public inputbutton down_position = new inputbutton(KeyCode.S);
    // public inputbutton left_position = new inputbutton(KeyCode.A);
    // public inputbutton right_position = new inputbutton(KeyCode.D);
    // private Vector2 _direction;
    // private void Update()
    // {
    //     _direction = JoyStick.Instance.JoyStickTouchPosition;
    //     if (_direction == Vector2.zero)
    //     {
    //         up_position.Up = true;
    //     }
    // }
}
