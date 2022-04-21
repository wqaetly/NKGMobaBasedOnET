using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Sirenix.OdinInspector;
using Werewolf.StatusIndicators.Services;

namespace Werewolf.StatusIndicators.Components
{
    /// <summary>
    /// 缩放类型
    /// </summary>
    [System.Flags]
    public enum ScaleType: byte
    {
        /// <summary>
        /// 不缩放
        /// </summary>
        None = 0,

        /// <summary>
        /// 仅缩放长度
        /// </summary>
        Length = 1,

        /// <summary>
        /// 仅缩放宽度
        /// </summary>
        Width = 1 << 1,

        /// <summary>
        /// 缩放长度和宽度
        /// </summary>
        All = Length | Width
    }

    public class Splat: MonoBehaviour
    {
        [SerializeField]
        public ScaleType ScaleType = ScaleType.All;

        /// <summary>
        /// 指示器归属的Unit Transform
        /// </summary>
        [SerializeField]
        public Transform SplatBelongToUnit;

        /// <summary>
        /// 指示器的进度，用于满足根据进度填充指示器的需求
        /// </summary>
        [SerializeField]
        [Range(0, 1)]
        [OnValueChanged("OnProgressChanged")]
        protected float progress = 1;

        /// <summary>
        /// 缩放指示器的长度，当ScaleType & ScaleType.Length 结果为 ScaleType.Length 时有效
        /// </summary>
        [SerializeField]
        [OnValueChanged("OnLengthChanged")]
        protected float length = 1;

        /// <summary>
        /// 缩放指示器的宽度，当ScaleType & ScaleType.Width 结果为 ScaleType.Width 时有效
        /// </summary>
        [SerializeField]
        [OnValueChanged("OnWidthChanged")]
        protected float width = 1;

        /// <summary>
        /// 整体缩放指示器，仅当ScaleType为All时有效
        /// </summary>
        [SerializeField]
        [OnValueChanged("OnScaleChanged")]
        protected float scale = 1;

        /// <summary>
        /// 缩放系数，用于处理美术素材大小/位置不一的情况，乘以此缩放系数可得到unity单位长度的缩放
        /// </summary>
        public float ScaleFactor = 1;
        // Properties

        private static MaterialPropertyBlock _SharedMaterialPropertyBlock;

        private static MaterialPropertyBlock s_SharedMaterialPropertyBlock
        {
            get
            {
                if (_SharedMaterialPropertyBlock == null)
                {
                    _SharedMaterialPropertyBlock = new MaterialPropertyBlock();
                }

                return _SharedMaterialPropertyBlock;
            }
            set
            {
                _SharedMaterialPropertyBlock = value;
            }
        }

        public virtual void OnEnable()
        {
            
        }

        /// <summary>
        /// Set the progress bar of Spell Indicator.
        /// </summary>
        public float Progress
        {
            get
            {
                return progress;
            }
            set
            {
                this.progress = value;
                this.OnProgressChanged(progress);
            }
        }
        
        public float Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.length = value;
                if ((this.ScaleType & ScaleType.Length) == ScaleType.Length)
                {
                    this.OnLengthChanged(this.length);
                }
            }
        }
        
        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                this.width = value;
                if ((this.ScaleType & ScaleType.Width) == ScaleType.Width)
                {
                    this.OnWidthChanged(this.width);
                }
            }
        }
        
        public float Scale
        {
            get
            {
                return this.scale;
            }
            set
            {
                this.scale = value;
                if ((this.ScaleType & ScaleType.All) == ScaleType.All)
                {
                    this.OnWidthChanged(this.Scale);
                    this.OnLengthChanged(this.Scale);
                }
            }
        }

        public virtual void Update()
        {
        }

        public virtual void OnProgressChanged(float changedValue)
        {
            SetShaderFloat("_Fill", changedValue);
        }

        public virtual void OnWidthChanged(float changedValue)
        {
            var transform1 = this.transform;
            Vector3 localScale = transform1.localScale;
            transform1.localScale = new Vector3(changedValue * ScaleFactor, localScale.y, localScale.z);
        }

        public virtual void OnLengthChanged(float changedValue)
        {
            var transform1 = this.transform;
            Vector3 localScale = transform1.localScale;
            transform1.localScale = new Vector3(localScale.x, localScale.y, changedValue * ScaleFactor);
        }

#if UNITY_EDITOR

        public void OnProgressChanged()
        {
            this.Progress = this.progress;
        }

        public void OnWidthChanged()
        {
            this.Width = this.width;
        }

        public void OnLengthChanged()
        {
            this.Length = this.length;
        }

        public void OnScaleChanged()
        {
            this.Scale = this.scale;
        }

#endif

        /// <summary>
        /// Helper method for setting float property on all projectors/shaders for splat.
        /// </summary>
        public void SetShaderFloat(string property, float value)
        {
            Renderer renderer = this.GetComponent<Renderer>();
            if (renderer == null) return;

            this.GetComponent<Renderer>().GetPropertyBlock(s_SharedMaterialPropertyBlock);
            s_SharedMaterialPropertyBlock.SetFloat(property, value);
            this.GetComponent<Renderer>().SetPropertyBlock(s_SharedMaterialPropertyBlock);
        }
    }
}