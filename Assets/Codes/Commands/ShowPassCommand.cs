using QFramework;


public class ShowPassCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.SendEvent<ShowPassEvent>();
    }
}

