using UnityEngine;

public class inputbutton
{
    public KeyCode keyCode;
    public bool Up;
    public bool Down;
    public bool Hold;

    public inputbutton(KeyCode keyCode)
    {
        this.keyCode = keyCode;
    }
    
    public void Get()
    {
        Down = Input.GetKeyDown(this.keyCode);
        Up = Input.GetKeyUp(this.keyCode);
        Hold = Input.GetKey(this.keyCode);
    }
    //移动设备调用
    // private Vector2 _direction;
    // public inputbutton(Vector2 direction)
    // {
    //     this._direction = direction;
    // }
    //
    // public void Get()
    // {
    //     Down = JoyStick.Instance.JoyStickDown;
    //     Up = JoyStick.Instance.JoyStickUp;
    //     Hold = JoyStick.Instance.JoyStickHold;
    // }
}
