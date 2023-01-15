/*输入设备管理系统，实现在输入设备发生变化时反应输入设备的状态和执行对应变化后的输入设备的委托
 * 
 * 
 * 
 */









using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;
using UnityEngine.InputSystem;
using UnityEngine;

namespace QFramework
{
    //实现System外部接口
    public interface IInputDeviceMgrSystem : ISystem
    {   
        /// <summary>
        /// 激活输入状态管理器
        /// </summary>
        void EnableDevice();
        /// <summary>
        /// 失活输入激活管理器
        /// </summary>
        void DisableDevice();
        /// <summary>
        /// 向对应输入设备传入委托，当当前输入设备转向该输入设备，则执行委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        void RegisterDevice<T>(Action fun);
    }
    public class InputDeviceMgrSystem : AbstractSystem, IInputDeviceMgrSystem
    {
        //储存注册的对应输入设备
        private Dictionary<Type, Action> mRegisteredDevices;
        //储存当前输入设备
        private InputDevice mCurDevice;
        protected override void OnInit()
        {
            mRegisteredDevices = new Dictionary<Type, Action>();
        }
        //注册对应输入设备的委托事件
        void IInputDeviceMgrSystem.RegisterDevice<T>(Action fun)
        {
            Type type = typeof(T);
            if (mRegisteredDevices.ContainsKey(type)) mRegisteredDevices[type] += fun;
            else mRegisteredDevices.Add(type, fun);
        }
        //失效，向InputSystem注销输入设备变化委托
        void IInputDeviceMgrSystem.DisableDevice()
        {
            InputSystem.onActionChange -= DetectCurrentInputDevice;
            InputSystem.onDeviceChange -= OnDeviceChange;
        }
        //激活，向InputSystem注册输入设备变化委托
        void IInputDeviceMgrSystem.EnableDevice()
        {
            //注意！：onDeviceChange 是 onActionChange 的子集，由于两者都是对设备变化（SwitchDevice）处理，
            //但处理内容不一样，OnDeviceChange是处理设备接入情况（针对异常设备），DetectCurrentInputDevice是外部已知输入设备的输入变化（针对不同设备输入）
            InputSystem.onActionChange += DetectCurrentInputDevice;
            InputSystem.onDeviceChange += OnDeviceChange;
        }
        //输入事件委托，即当发生按键输入（会触发InputSystem.onActionChange）执行函数。
        private void DetectCurrentInputDevice(object o,InputActionChange change)
        {
            if (change != InputActionChange.ActionPerformed) return;    //判断设备是否输入
            SwitchDevice((o as InputAction).activeControl.device);      //获取输入的设备类型，传入SwitchDevice
        }
        //输入设备变化委托，当输入设备发生变化时，
        private void OnDeviceChange(InputDevice device,InputDeviceChange change)
        {
            switch (change)
            {
                case InputDeviceChange.Reconnected:
                    Debug.Log("重新连接");
                    SwitchDevice(device);
                    break;
                case InputDeviceChange.Disabled:
                    Debug.Log("连接断开");
//当游戏处于编辑状态或Pc状态下编译代码块
#if UNITY_STANDLONE || UNITY_EDITOR
                    //当连接断开时，使用Keyboard替代
                    if (device is Gamepad)
                    {
                        //device = Keyboard.current; //current是设备接受状态，如果识别设备接入,则
                        device = InputSystem.GetDevice<Keyboard>();
                    }
                    SwitchDevice(device);
#endif
                    break;
            }

        }
        //当切换输入设备，执行切换的输入设备的委托
        private void SwitchDevice(InputDevice device)
        {
            //判断输入类型
            if (device == null && device == mCurDevice) return;
            Type type = null;
            if (device is Keyboard) type = typeof(Keyboard);
            else if (device is Gamepad) type = typeof(Gamepad);
            else if (device is Pointer) type = typeof(Pointer);
            else if (device is Joystick) type = typeof(Joystick);
            //判断设备类型是否规范和是否有对应的注册事件
            if (device == null || mRegisteredDevices.TryGetValue(type, out var CallBack)) return;
            mCurDevice = device;
            CallBack?.Invoke();
        }
    }
}
