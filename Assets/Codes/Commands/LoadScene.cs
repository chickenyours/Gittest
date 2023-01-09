using QFramework;
using UnityEngine.SceneManagement;


public class LoadScene : AbstractCommand
{
    private readonly string sceneName;

    public LoadScene(string value)
    {
        sceneName = value;
    }
    protected override void OnExecute()
    {
        this.GetSystem<ITimeSystem>().DisPose();
        this.GetSystem<IObjectPoolSystem>().DisPose();
        SceneManager.LoadScene(sceneName);
    }
}
