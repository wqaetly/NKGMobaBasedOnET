//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年5月23日 11:05:41
//------------------------------------------------------------

using System;
using UnityEngine;

namespace ETModel
{
    [ObjectSystem]
    public class UserInputComponentStartSystem: StartSystem<UserInputComponent>
    {
        public override void Start(UserInputComponent self)
        {
            self.Start();
        }
    }

    [ObjectSystem]
    public class UserInputComponentUpdateSystem: UpdateSystem<UserInputComponent>
    {
        public override void Update(UserInputComponent self)
        {
            self.Update();
        }
    }

    public class UserInputComponent: Component
    {
        public bool RightMouseDown { get; set; }
        public bool RightMouseUp { get; set; }

        public bool ADown_long { get; set; }
        public bool ADown { get; set; }
        public bool AUp { get; set; }

        public bool ADouble { get; set; }
        public double ALastClickTime { get; set; }

        public double DLastClickTime { get; set; }
        public bool DDown { get; set; }
        public bool DUp { get; set; }

        public bool DDouble { get; set; }
        public bool DDown_long { get; set; }

        public bool JDown_long { get; set; }
        public bool JDown { get; set; }
        public bool JUp { get; set; }
        public bool JDouble { get; set; }
        public double JLastClickTime { get; set; }

        public bool WDown_long { get; set; }
        public bool WDown { get; set; }
        public bool WUp { get; set; }

        public bool WDouble { get; set; }
        public double WLastClickTime { get; set; }

        public bool SpaceDown_long { get; set; }
        public bool SpaceDown { get; set; }
        public bool SpaceUp { get; set; }
        public double SpaceLastClickTime { get; set; }

        public long currentTime;

        public long startTime;

        public void Update()
        {
            ResetAllState();

            this.currentTime = TimeHelper.Now() - this.startTime;

            this.CheckKey();
            this.CheckKeyUp();
            this.CheckKeyDown();
        }

        /// <summary>
        /// 检查按键抬起
        /// </summary>
        private void CheckKeyUp()
        {
            if (Input.GetMouseButtonUp(1))
            {
                this.RightMouseDown = false;
                this.RightMouseUp = true;
            }
            
            if (Input.GetKeyUp(KeyCode.A))
            {
                ADown_long = false;
                this.ADouble = false;
                this.AUp = true;
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                DDown_long = false;
                this.DDouble = false;
                this.DUp = true;
            }

            if (Input.GetKeyUp(KeyCode.J))
            {
                this.JDown_long = false;
                this.JDouble = false;
                this.JUp = true;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                this.SpaceDown_long = false;
                this.SpaceUp = true;
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                this.WDown_long = false;
                this.WUp = true;
            }
        }

        /// <summary>
        /// 检查按键落下
        /// </summary>
        void CheckKeyDown()
        {
            if (Input.GetMouseButtonDown(1))
            {
                this.RightMouseDown = true;
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                this.ADown = true;

                if ((this.currentTime - this.ALastClickTime) / 1000f <= 0.5f)
                {
                    this.ADouble = true;
                }
                else
                {
                    this.ADouble = false;
                }

                this.ALastClickTime = this.currentTime;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                this.WDown = true;

                if ((this.currentTime - this.WLastClickTime) / 1000f <= 0.5f)
                {
                    this.WDouble = true;
                }
                else
                {
                    this.WDouble = false;
                }

                this.WLastClickTime = this.currentTime;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                this.DDown = true;

                if ((this.currentTime - this.DLastClickTime) / 1000f <= 0.5f)
                {
                    this.DDouble = true;
                }
                else
                {
                    this.DDouble = false;
                }

                this.DLastClickTime = this.currentTime;
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                this.JDown = true;

                if ((this.currentTime - this.JLastClickTime) / 1000f <= 0.5f)
                {
                    Log.Info("A双击");
                }
                else
                {
                    Log.Info("未双击");
                }

                this.JLastClickTime = this.currentTime;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                this.SpaceDown = true;

                if ((this.currentTime - this.SpaceLastClickTime) / 1000f <= 0.5f)
                {
                    Log.Info("A双击");
                }
                else
                {
                    Log.Info("未双击");
                }

                this.SpaceLastClickTime = this.currentTime;
            }
        }

        /// <summary>
        /// 检查按键输入
        /// </summary>
        private void CheckKey()
        {
            if (Input.GetKey(KeyCode.A))
            {
                ADown_long = true;
            }

            if (Input.GetKey(KeyCode.D))
            {
                DDown_long = true;
            }

            if (Input.GetKey(KeyCode.J))
            {
                JDown_long = true;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                SpaceDown_long = true;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                WDown_long = true;
            }
        }

        private void ResetAllState()
        {
            this.RightMouseDown = false;
            this.RightMouseUp = false;

            this.AUp = false;
            this.ADown = false;

            this.DDown = false;
            this.DUp = false;

            this.JUp = false;
            this.JDown = false;

            this.WUp = false;
            this.WDown = false;

            this.SpaceUp = false;
            this.SpaceDown = false;
        }

        public void Start()
        {
            this.startTime = TimeHelper.Now();
        }
    }
}