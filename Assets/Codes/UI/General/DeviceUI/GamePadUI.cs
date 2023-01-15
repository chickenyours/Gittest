using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GamePadUI : MonoBehaviour
{
    private Transform GamePad;
    private Gamepad broad;
    private void Awake()
    {
        GamePad = transform.Find("MoveButton");
    }
    private void OnEnable()
    {
        broad = Gamepad.current;
    }
    private void Update()
    {
        GamePad.localPosition = broad.leftStick.ReadValue() * 20f;
    }
}
