using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class KeyBoradUI : MonoBehaviour
{
    //储存四个方向键的Image组件
    private Image mUpArrow;
    private Image mDownArrow;
    private Image mLeftArrow;
    private Image mRightArrow;
    //储存键盘
    private Keyboard board;
    private void Awake()
    {
        mUpArrow = transform.Find("UpArrow").gameObject.GetComponent<Image>();
        mDownArrow =  transform.Find("DownArrow").gameObject.GetComponent<Image>();
        mLeftArrow = transform.Find("LeftArrow").gameObject.GetComponent<Image>();
        mRightArrow = transform.Find("RightArrow").gameObject.GetComponent<Image>();
    }
    private void OnEnable()
    {
        board = Keyboard.current;
    }
    private void Update()
    {
        mUpArrow.color = board.wKey.isPressed || board.upArrowKey.isPressed ? new Color(0, 0, 0, 100) : new Color(255,255,255,255);
        mDownArrow.color = board.sKey.isPressed || board.downArrowKey.isPressed ? new Color(0, 0, 0, 100) : new Color(255, 255, 255, 255);
        mLeftArrow.color = board.aKey.isPressed || board.leftArrowKey.isPressed ? new Color(0, 0, 0, 100)  : new Color(255, 255, 255, 255);
        mRightArrow.color = board.dKey.isPressed || board.rightArrowKey.isPressed ? new Color(0, 0, 0, 100) : new Color(255, 255, 255, 255);
    }
}
