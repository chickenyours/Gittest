using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.InputSystem;


public interface IGameModel : IModel
{
    BindableProperty<int> score { get; }
    BindableProperty<int> shootTime { get; }  
    BindableProperty<int> direction { get; }
    BindableProperty<Vector2> mousePosition { get; }
}

public class GameModel : AbstractModel,IGameModel
{
    
    //储存玩家移动的方向
    BindableProperty<int> IGameModel.direction { get; } = new BindableProperty<int>(0);
    BindableProperty<int> IGameModel.shootTime { get; } = new BindableProperty<int>(1);
    BindableProperty<int> IGameModel.score { get; } = new BindableProperty<int>(0);
    //储存鼠标坐标
    public BindableProperty<Vector2> mousePosition { get; } = new BindableProperty<Vector2>(Mouse.current.position.ReadValue());
    //向单例注册Update委托
    protected override void OnInit()
    {
        PublicMono.Instance.OnUpdate += Update;
    }
    //不断更新数据
    void Update()
    {
        mousePosition.Value = Mouse.current.position.ReadValue();
    }
}

