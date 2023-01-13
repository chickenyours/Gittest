using System;
using QFramework;

namespace QFramework
{
    public interface IAudioModel : IModel
    {
        BindableProperty<float> SoundVolume { get; }
        BindableProperty<float> BgmVolume { get; }
    }
    class AudioModel : AbstractModel, IAudioModel
    {
        public BindableProperty<float> SoundVolume { get; } = new BindableProperty<float>(0);
        public BindableProperty<float> BgmVolume { get; } = new BindableProperty<float>(0);
        protected override void OnInit()
        {

        }
    }
}
