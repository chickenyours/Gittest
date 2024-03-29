﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using QFramework;

namespace PlayerControl
{
    public class playerControl : GameControll
    {
        private Rigidbody2D mrig;
        //获取护卫
        private Transform mEyeUp;
        private Transform mEyeDown;
        public float movespeadMax = 15f;
        public float moveAc = 0.2f;
        public float moveDx = 0.1f;
        public float jumpspead = 20f;
        public int direction = 1;
        private Transform directionSprite;
        private float directionX = 0.7f;
        /// <summary>
        /// 公开表示射击时间间隔
        /// </summary>
        public float fireInterval;
        private float FireInterval { get {return 1f / fireInterval ; } set { fireInterval = value; } }
        /// <summary>
        /// 内存储射击间隔时间
        /// </summary>
        private float fireCurrentTime;

        //储存位移输入变量
        private int mInputX;
        private int mInputY;
        //储存跳跃的变量
        private bool isJump;
        //储存射击的变量
        private bool mAttackInput;

        //获取输入执行判断接口
        private IUIModel mUIModel;
        private void Awake()
        {
            this.SendCommand<InitGameCommand>();
        }
        private void Start()
        {
            //mUIModel = this.GetModel<IUIModel>();
            mrig = gameObject.GetComponent<Rigidbody2D>();
            //获取子对象
            directionSprite = transform.Find("Direction");
            directionSprite.localPosition = new Vector2(directionX,0);
            mEyeUp = transform.Find("eyeUp");
            mEyeDown = transform.Find("eyeDown");
            //定义参数属性
            fireCurrentTime = 0f;
            this.GetSystem<ICameraSystem>().SetTarget(this.transform);
            this.GetSystem<IAudioMgrSystem>().PlayBgm("in the autumn sky");
            //注册输入事件
            this.RegisterEvent<MoveInputEvent>(e =>
            {
                mInputX = e.inputX;
                mInputY = e.inputY;
            });
            this.RegisterEvent<JumpInputEvent>(e =>
            {
                isJump = true;
            });
            this.RegisterEvent<ShootInputEvent>(e =>
            {
                mAttackInput = e.isTrigger;
            });
        }
        private void Update()
        {
            if (mAttackInput)
            {
                if(fireCurrentTime >= FireInterval)
                {
                    //获取Target的坐标
                    Vector3 mousePosition = new Vector3(
                        this.GetModel<IGameModel>().mousePosition.Value.x, 
                        this.GetModel<IGameModel>().mousePosition.Value.y,
                        0);
                    mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    mousePosition.z = 0;
                    //在eyeUp生成子弹
                    this.GetSystem<IObjectPoolSystem>().Get("Items/Butter", o =>
                    {
                        shoot bullet = o.GetComponent<shoot>();
                        //设置子弹飞行时间和委托
                        Timer timer_ = bullet.GetSystem<ITimeSystem>().Add(3f, false, (timer) =>
                        {
                            bullet.GetSystem<IObjectPoolSystem>().Recovery(bullet.gameObject);
                        });
                        //bullet脚本破坏时委托
                        bullet.destoryToDo = () =>
                        {
                            //回收计时器
                            bullet.GetSystem<ITimeSystem>().Recover(timer_);
                        };
                        o.transform.localPosition = mEyeUp.position;
                        //定位方向
                        Vector2 direction = mousePosition - mEyeUp.transform.position;
                        float directionAngle = Vector2.Angle(direction, Vector2.right);
                        directionAngle = direction.y > 0 ? directionAngle : -directionAngle;
                        o.transform.rotation = Quaternion.Euler(0,0,directionAngle + Random.Range(-1f, 1f) * bullet.moveDeviation);
                        //o.transform.Rotate(0,0, Random.Range(-1f, 1f) * bullet.moveDeviation);
                    });
                    //在eyeDown生成子弹
                    this.GetSystem<IObjectPoolSystem>().Get("Items/Butter", o =>
                    {
                        shoot bullet = o.GetComponent<shoot>();
                        //设置子弹飞行时间和委托
                        Timer timer_ = bullet.GetSystem<ITimeSystem>().Add(3f, false, (timer) =>
                        {
                            bullet.GetSystem<IObjectPoolSystem>().Recovery(bullet.gameObject);
                        });
                        //bullet脚本破坏时委托
                        bullet.destoryToDo = () =>
                        {
                            //回收计时器
                            bullet.GetSystem<ITimeSystem>().Recover(timer_);
                        };
                        o.transform.localPosition = mEyeDown.position;
                        Vector2 direction = mousePosition - mEyeDown.transform.position;
                        float directionAngle = Vector2.Angle(direction, Vector2.right);
                        directionAngle = direction.y > 0 ? directionAngle : -directionAngle;
                        o.transform.rotation = Quaternion.Euler(0, 0, directionAngle + Random.Range(-1f, 1f) * bullet.moveDeviation);
                    });
                    this.GetSystem<IAudioMgrSystem>().PlaySound("fire");
                    fireCurrentTime = 0f;
                }
                else { fireCurrentTime += Time.deltaTime; }
            }
        }
        private void FixedUpdate()
        {
            if ( isJump )
            {
                if(Mathf.Abs(mrig.velocity.y) <= float.Epsilon)
                {
                    this.GetSystem<IAudioMgrSystem>().PlaySound("jump");
                    mrig.velocity = new Vector2(mrig.velocity.x, jumpspead);
                }
                isJump = false;
            }
            //获取输入并改变位移
            mrig.velocity = mInputX != 0 ? 
                new Vector2(Mathf.Clamp(mrig.velocity.x + mInputX*moveAc,-movespeadMax,movespeadMax), mrig.velocity.y) :  
                new Vector2(Mathf.MoveTowards(mrig.velocity.x,0,moveDx),mrig.velocity.y);
            //修改角色朝向
            direction = mrig.velocity.x != 0 && mrig.velocity.x < 0 ? -1 : mrig.velocity.x == 0 ? direction: 1;
            this.GetModel<IGameModel>().direction.Value = direction;
            //改变脸部朝向
            directionSprite.localPosition = new Vector2(directionX * direction,directionSprite.localPosition.y);
        }
        private void LateUpdate()
        {
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Door"))
            {
                this.SendCommand<LoadScene>(new LoadScene("GameOver"));
                this.GetSystem<IAudioMgrSystem>().PlaySound("passGame");
                this.SendCommand<StopBgm>();
            }
            if (other.CompareTag("Reward"))
            {
                if (GameObject.Find("Golds").GetComponentsInChildren<Transform>(true).Length <= 2)
                {
                    this.GetModel<IGameModel>().score.Value = 666;
                }
                else
                {
                    //setScore.AddScore(1);
                    this.GetModel<IGameModel>().score.Value++;
                    //if(this.GetModel<IGameModel>().score.Value == 3 )
                    //{
                    //    this.SendCommand<StopBgm>();
                    //}
                    //else if(this.GetModel<IGameModel>().score.Value == 4)
                    //{
                    //    this.GetSystem<IAudioMgrSystem>().PlayBgm("SadBoi Q");
                    //}
                    //else if(this.GetModel<IGameModel>().score.Value == 5)
                    //{
                    //    this.GetSystem<IAudioMgrSystem>().PlayBgm("Time to Pretend");
                    //}

                }
                this.GetSystem<IAudioMgrSystem>().PlaySound("eatGold"); 
                Destroy(other.gameObject);
            }
        }
    }
}