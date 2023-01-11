using System;
using QFramework;

    public interface IUIModel : IModel
    {
        BindableProperty<bool> isSettingPanelOpen { get; } 
    }
    public class UIModel : AbstractModel, IUIModel
    {
        public BindableProperty<bool> isSettingPanelOpen { get; } = new BindableProperty<bool>(false);
        protected override void OnInit()
        {
            
        }
    }

