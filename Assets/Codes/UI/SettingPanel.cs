using QFramework;
using UnityEngine.UI;
using UnityEngine;

public class SettingPanel : GameControll
{
    //保存了及定义单例
    public static GameObject intence;
    //保存了IAudioModel接口引用以便获取数据
    private IAudioModel mAudioModel;
    //保存了BgmSlider和SoundSlier的Value数据
    private Text BgmSliderValue;
    private Text SoundsliderValue;
    private void Awake()
    {
        //判断SettingPanel是否有一个
        if (intence != null && intence != gameObject) Destroy(gameObject);
        else intence = this.gameObject;
        //更新访问方式
        this.GetModel<IUIModel>().isSettingPanelOpen.Value = true;
        //获取组件和数据信息
        mAudioModel = this.GetModel<IAudioModel>();
        transform.Find("BackBin").gameObject.GetComponent<Button>().onClick.AddListener(OnCloseBin);
        //设置BgmSlider
        var BgmSlider = transform.Find("BgmSlider").GetComponent<Slider>();
        BgmSliderValue = BgmSlider.transform.Find("Value").GetComponent<Text>();
        BgmSlider.onValueChanged.AddListener(OnBgmVolumeChange);
        BgmSlider.value = mAudioModel.BgmVolume.Value;
        BgmSliderValue.text = Mathf.Round(BgmSlider.value * 100).ToString();
        //设置SoundSlider
        var Soundslider = transform.Find("SoundSlider").GetComponent<Slider>();
        SoundsliderValue = Soundslider.transform.Find("Value").GetComponent<Text>();
        Soundslider.onValueChanged.AddListener(OnSoundVolumeChange);
        Soundslider.value = mAudioModel.SoundVolume.Value;
        SoundsliderValue.text = Mathf.Round(Soundslider.value * 100).ToString();
    }
    private void OnCloseBin()
    {
        Destroy(gameObject);
    }
    private void OnDisable()
    {
        this.GetModel<IUIModel>().isSettingPanelOpen.Value = false;
    }
    private void OnBgmVolumeChange(float arg)
    {
        mAudioModel.BgmVolume.Value = arg;
        BgmSliderValue.text = Mathf.Round( arg * 100).ToString();
    }
    private void OnSoundVolumeChange(float arg)
    {
        mAudioModel.SoundVolume.Value = arg;
        SoundsliderValue.text = Mathf.Round(arg * 100).ToString();
    }
}
