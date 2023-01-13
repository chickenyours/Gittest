using UnityEngine;
using QFramework;

public interface ICameraSystem : ISystem
{
    void SetTarget(Transform transform);
}

public class CameraSystem : AbstractSystem,ICameraSystem
{
    //跟随目标
    private Transform followTargetObject;
    //跟随点的临时坐标
    private Vector3 mTarget;
    //定义死角
    private float minX = -100f, minY = -100f, maxX = 100f, maxY = 100f;
    //定义追踪速度
    private float mFollowSpeed = 3f;
    protected override void OnInit()
    {
        PublicMono.Instance.OnFixedUpdate += Update;
        mTarget.z = -10;
    }

    void ICameraSystem.SetTarget(Transform transform)
    {
        followTargetObject = transform;
    }

    void Update()
    {
        if (!followTargetObject) return;
        mTarget.x = Mathf.Clamp(followTargetObject.position.x,minX,maxX);
        mTarget.y = Mathf.Clamp(followTargetObject.position.y, minY, maxY);
        Transform cm = Camera.main.transform;
        if ((mTarget - cm.position).sqrMagnitude < 0.01) return;
        cm.position = Vector3.Lerp(Camera.main.transform.position,mTarget, mFollowSpeed * Time.deltaTime);
        //Camera.main.transform.localPosition = new Vector3(followTargetObject.localPosition.x, followTargetObject.localPosition.y, -10);
    }
}
