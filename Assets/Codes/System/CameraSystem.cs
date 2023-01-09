using UnityEngine;
using QFramework;

public interface ICameraSystem : ISystem
{
    void SetTarget(Transform transform);
}

public class CameraSystem : AbstractSystem,ICameraSystem
{
    private Transform followTarget;

    protected override void OnInit()
    {
        PublicMono.instance.OnLateUpdate += Update;
    }

    void ICameraSystem.SetTarget(Transform transform)
    {
        followTarget = transform;
    }

    void Update()
    {
        if (!followTarget) return;
        Camera.main.transform.localPosition = new Vector3(followTarget.localPosition.x, followTarget.localPosition.y, -10);
    }
}
