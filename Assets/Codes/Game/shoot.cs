using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerControl;
using QFramework;
using System;

public class shoot : MonoBehaviour,IController
{
    private static int shoots = 0;
    /// <summary>
    /// 飞行速度
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// 发射角度偏差
    /// </summary>
    public float moveDeviation;
    /// <summary>
    /// 发射方向
    /// </summary>
    private float direction;
    /// <summary>
    /// 碰撞遮罩
    /// </summary>
    private LayerMask mLayerMask;
    /// <summary>
    /// 公开发射次数
    /// </summary>
    public int Shoots { get { return shoots; } }
    /// <summary>
    /// 公开一个是否属于对象池系统的标签
    /// </summary>
    private bool mIsInPoolSystem = true;
    public bool IsInPoolSystem => mIsInPoolSystem;
    public Action destoryToDo;
    private void Start()
    {
        mLayerMask = LayerMask.GetMask("ground", "Target");
        shoots++;
    }
    private void OnEnable()
    {
        direction = this.GetModel<IGameModel>().direction.Value;
    }
    private void FixedUpdate()
    {
        this.transform.Translate(new Vector3(moveSpeed * Time.deltaTime,0f,0f));
        var coll = Physics2D.OverlapBox(transform.position, transform.localScale, 0, mLayerMask);
        if (coll)
        {
            if (coll.gameObject.CompareTag("Target"))
            {
                this.SendCommand<ShowPassCommand>();
                Destroy(coll.gameObject);
                    //door.SetActive(true);
            }
            this.GetSystem<IAudioMgrSystem>().PlaySound("bulletKilled");
            //发生遮罩碰撞后若属于对象池系统中则回收，否则销毁
            if (mIsInPoolSystem) this.GetSystem<IObjectPoolSystem>().Recovery(gameObject);
            else Destroy(gameObject); 
        } 
    }
    private void OnDisable()
    {
        destoryToDo?.Invoke();
    }
    private void ShowShootTime()
    {
        int a = this.GetModel<IGameModel>().shootTime.Value++;
        print(a);
    }
    IArchitecture IBelongToArchitecture.GetArchitecture()
    {
        return Game.Interface;
    }
}
