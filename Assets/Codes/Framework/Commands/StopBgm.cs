using QFramework;


class StopBgm : AbstractCommand
{
    protected override void OnExecute()
    {
        this.SendEvent<StopBgmEvent>();
    }
}

