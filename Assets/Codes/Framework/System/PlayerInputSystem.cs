using UnityEngine;
using QFramework;
using UnityEngine.InputSystem;

public interface IPlayerInputSystem : ISystem
{
    void Enable();
    void Disable();
}
public enum E_InputDevice
{
    Keyboard,
    Gamepad,
    Pointer,
}
//定义事件
public struct MoveInputEvent
{
    public int inputX;
    public int inputY;
}
public struct ShootInputEvent
{
    public bool isTrigger;
}
public struct JumpInputEvent
{

}
//实现Player的输入管理系统
public class PlayerInputSystem : AbstractSystem, IPlayerInputSystem, GameControls.IGamePlayActions
{
    //保存事件数据结构
    private MoveInputEvent mMoveInputEvent = new MoveInputEvent();
    private ShootInputEvent mShootEvent = new ShootInputEvent();
    private JumpInputEvent mJumpInputEvent = new JumpInputEvent();
    //实例化InputControl 
    private GameControls mGameControll = new GameControls();
    //储存当前输入状态
    private E_InputDevice mCurrentDevice;
    //储存遥感输入灵敏度阈值
    private float sensitive = 0.3f;
    protected override void OnInit()
    {
        //注册GamePlay的所有输入委托
        mGameControll.GamePlay.SetCallbacks(this);
        //注册InputDeviceMgrSystem所有输入设备变更委托
        this.GetSystem<IInputDeviceMgrSystem>().RegisterDevice<Keyboard>(() => { mCurrentDevice = E_InputDevice.Keyboard; });
        this.GetSystem<IInputDeviceMgrSystem>().RegisterDevice<Gamepad>(() => { mCurrentDevice = E_InputDevice.Gamepad; });
        this.GetSystem<IInputDeviceMgrSystem>().RegisterDevice<Pointer>(() => { mCurrentDevice = E_InputDevice.Pointer; });
    }
    //失活（禁用）输入系统
    void IPlayerInputSystem.Disable()
    {
        mGameControll.GamePlay.Disable();
    }
    //激活输入系统
    void IPlayerInputSystem.Enable()
    {
        mGameControll.GamePlay.Enable();
    }

    void GameControls.IGamePlayActions.OnJump(InputAction.CallbackContext context)
    {
        if (context.started) this.SendEvent(mJumpInputEvent);
    }

    void GameControls.IGamePlayActions.OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            Vector2 input = context.ReadValue<Vector2>();
            Debug.Log(input);
            //两种输入方式处理
            switch (mCurrentDevice)
            {
                case E_InputDevice.Keyboard:
                    var board = Keyboard.current;
                    if ((board.dKey.isPressed || board.rightArrowKey.isPressed) 
                        && (board.aKey.isPressed || board.leftArrowKey.isPressed))
                    {
                        switch (mMoveInputEvent.inputX)
                        {
                            
                            case -1: mMoveInputEvent.inputX = board.dKey.wasPressedThisFrame || board.rightArrowKey.wasPressedThisFrame ? 1 : 0; Debug.Log("Conflict"); break;
                            case 1: mMoveInputEvent.inputX = board.aKey.wasPressedThisFrame || board.leftArrowKey.wasPressedThisFrame ? -1 : 0; Debug.Log("Conflict"); break;
                        }
                    }
                    else
                    {
                        mMoveInputEvent.inputX = (int)input.x;
                        mMoveInputEvent.inputY = (int)input.y;
                    }
                    break;
                case E_InputDevice.Gamepad:
                    //处理遥感平滑操作问题，统一两极输出
                    mMoveInputEvent.inputX = Mathf.Abs(input.x) < sensitive ? 0 : input.x < 0 ? -1 : 1;
                    mMoveInputEvent.inputY = Mathf.Abs(input.y) < sensitive ? 0 : input.y < 0 ? -1 : 1;
                    break;
            }
        } 
        else if (context.canceled)
        {
            Debug.Log("context.canceled");
            switch (mCurrentDevice)
            {
                case E_InputDevice.Keyboard:
                    var board = Keyboard.current;   //获取外界键盘输入设备
                    switch (mMoveInputEvent.inputX)
                    {
                        case -1: mMoveInputEvent.inputX = board.dKey.wasPressedThisFrame || board.rightArrowKey.wasPressedThisFrame ? 1 : 0; break;
                        case  1: mMoveInputEvent.inputX = board.aKey.wasPressedThisFrame || board.leftArrowKey.wasPressedThisFrame ? -1 : 0; break;
                    }
                    break;
                case E_InputDevice.Gamepad:
                    mMoveInputEvent.inputX = 0;
                    mMoveInputEvent.inputY = 0;
                    break;
            }
        }
        this.SendEvent(mMoveInputEvent);
    }

    void GameControls.IGamePlayActions.OnShoot(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            mShootEvent.isTrigger = true;
        }
        else if (context.canceled)
        {
            mShootEvent.isTrigger = false;
        }
        this.SendEvent(mShootEvent);
    }
}
