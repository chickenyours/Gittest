using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

public class SetScore : MonoBehaviour,IController
{
    private Text score;
    private void Start()
    {
        score = GameObject.Find("ScoreText").GetComponent<Text>();
        this.GetModel<IGameModel>().score.RegisterWithInitValue(ScoreChange)
            .UnRegisterWhenGameObjectDestroyed(gameObject);
    }
    private void ScoreChange(int score)
    {
        this.score.text = score.ToString();
    }
    public void AddScore(int value)
    {
        int a = int.Parse(score.text) + value;
        score.text = a.ToString();
    }
    public void AddScore(string value)
    {
        score.text = value;
    }

    IArchitecture IBelongToArchitecture.GetArchitecture()
    {
        return Game.Interface;
    }
}
