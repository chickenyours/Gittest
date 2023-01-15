using QFramework;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceCheckUI : GameControll
{
    //储存获取KeyboardUI和GamePadUI对象脚本
    private KeyBoradUI mKeyBoardUI;
    private GamePadUI mGamePadUI;
    private void Awake()
    {
        //获取KeyboardUI和GamePadUI对象脚本
        mKeyBoardUI= transform.Find("KeyBoard").GetComponent<KeyBoradUI>();
        mGamePadUI = transform.Find("GamePad").GetComponent<GamePadUI>();
        //注册设备变更委托事件
        this.GetSystem<IInputDeviceMgrSystem>().RegisterDevice<Keyboard>(() =>
        {
            ChangeToKeyBoardUI();
        });
        this.GetSystem<IInputDeviceMgrSystem>().RegisterDevice<Gamepad>(() =>
        {
            ChangeToGamePadUI();
        });
    }
    private void Start()
    {
        
    }
    private void ChangeToKeyBoardUI()
    {
        mKeyBoardUI.gameObject.SetActive(true);
        mGamePadUI.gameObject.SetActive(false);
    }
    private void ChangeToGamePadUI()
    {
        mKeyBoardUI.gameObject.SetActive(false);
        mGamePadUI.gameObject.SetActive(true);
    }
}
