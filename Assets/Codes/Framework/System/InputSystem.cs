using UnityEngine;
using QFramework;
using UnityEngine.InputSystem;

public interface IPlayerInputSystem : ISystem
{
    void Enable();
    void Disable();
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
    private MoveInputEvent mMoveInputEvent;
    private ShootInputEvent mShootEvent;
    private JumpInputEvent mJumpInputEvent;
    //实例化InputControl 
    private GameControls mGameControll = new GameControls();
    protected override void OnInit()
    {
        mGameControll.GamePlay.SetCallbacks(this);
        mGameControll.GamePlay.Enable();
    }

    void IPlayerInputSystem.Disable()
    {
        mGameControll.GamePlay.Disable();
    }
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
            mMoveInputEvent.inputX = (int)input.x;
            mMoveInputEvent.inputY = (int)input.y;
        } 
        else if (context.canceled)
        {
            mMoveInputEvent.inputX = 0;
            mMoveInputEvent.inputY = 0;
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
