using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;


public interface IGameModel : IModel
{
    BindableProperty<int> score { get; }
    BindableProperty<int> shootTime { get; }  
    BindableProperty<int> direction { get; }
    BindableProperty<float> mouseWorldX { get; }
    BindableProperty<float> mouseWorldY { get; }
}

public class GameModel : AbstractModel,IGameModel
{
    /// <summary>
    /// 储存玩家移动的方向
    /// </summary>
    BindableProperty<int> IGameModel.direction { get; } = new BindableProperty<int>(0);
    BindableProperty<int> IGameModel.shootTime { get; } = new BindableProperty<int>(1);
    BindableProperty<int> IGameModel.score { get; } = new BindableProperty<int>(0);
    //保存鼠标坐标
    public BindableProperty<float> mouseWorldX { get; } = new BindableProperty<float>(Input.mousePosition.x);
    public BindableProperty<float> mouseWorldY { get; } = new BindableProperty<float>(Input.mousePosition.y);

    protected override void OnInit()
    {
        PublicMono.instance.OnUpdate += Update;
    }
    //不断更新数据
    void Update()
    {
        mouseWorldX.Value = Input.mousePosition.x;
        mouseWorldY.Value = Input.mousePosition.y;
    }
}

