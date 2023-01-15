using QFramework;

public class InitGameCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<IInputDeviceMgrSystem>().EnableDevice();
        this.GetSystem<IPlayerInputSystem>().Enable();
    }
}