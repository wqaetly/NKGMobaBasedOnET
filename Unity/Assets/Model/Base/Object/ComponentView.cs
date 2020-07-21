using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEngine;

namespace ETModel
{
    public class ComponentView: SerializedMonoBehaviour
    {
        public object Component;

        [InfoBox("注意，这将十分消耗性能。", InfoMessageType.Warning)]
        [LabelText("是否强制获取所有非公有成员")]
        [OnValueChanged("TryGetAllPrivateMember")]
        public bool GetAllPrivateMember = false;

        [HideIf("GetAllPrivateMember")]
        [InfoBox("注意，这将十分消耗性能。默认递归一次，可更改下面的递归次数来改变递归深度", InfoMessageType.Warning)]
        [LabelText("是否强制获取所有非公有成员(递归)")]
        [OnValueChanged("TryGetAllPrivateMemberDeeply")]
        public bool GetAllPrivateMemberDeeply = false;

        [InfoBox("开启递归反射时生效")]
        [LabelText("反射层次数")]
        [PropertyRange(1, 4)]
        public int ReflectCount = 1;

        [ShowIf("GetAllPrivateMember")]
        [LabelText("所有非公有成员")]
        public Dictionary<string, object> AllPrivateMember;

        [ShowIf("GetAllPrivateMemberDeeply")]
        [LabelText("所有非公有成员（递归）")]
        public Dictionary<string, object> AllPrivateMember_Deeply;

        public void TryGetAllPrivateMember()
        {
            if (this.GetAllPrivateMember)
            {
                if (this.AllPrivateMember == null)
                {
                    AllPrivateMember = new Dictionary<string, object>();
                }

                this.AllPrivateMember.Clear();
                StartGetAllPrivateMember(this.AllPrivateMember, this.Component);
            }
        }

        public void TryGetAllPrivateMemberDeeply()
        {
            if (this.GetAllPrivateMemberDeeply)
            {
                if (this.AllPrivateMember_Deeply == null)
                {
                    this.AllPrivateMember_Deeply = new Dictionary<string, object>();
                }

                this.AllPrivateMember_Deeply.Clear();
                StartGetAllPrivateDeeply(this.AllPrivateMember_Deeply, this.Component);
            }
        }

        public void StartGetAllPrivateMember(Dictionary<string, object> targetDic, object targetObject)
        {
            //获取类中的非public字段
            FieldInfo[] privateFields = targetObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var VARIABLE in privateFields)
            {
                targetDic.Add(VARIABLE.GetNiceName(), VARIABLE.GetValue(targetObject));
            }

            //获取类中的非public屬性
            PropertyInfo[] privatePropertyInfos = targetObject.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var VARIABLE in privatePropertyInfos)
            {
                targetDic.Add(VARIABLE.GetNiceName(), VARIABLE.GetValue(targetObject));
            }
        }

        public void StartGetAllPrivateDeeply(Dictionary<string, object> targetDic, object targetObject)
        {
            //获取类中的非public字段
            FieldInfo[] privateFields = targetObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var VARIABLE in privateFields)
            {
                targetDic.Add(VARIABLE.GetNiceName(), GetAllPrivateMemberInternal(VARIABLE.GetValue(targetObject), ReflectCount));
            }

            //获取类中的非public屬性
            PropertyInfo[] privatePropertyInfos = targetObject.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var VARIABLE in privatePropertyInfos)
            {
                targetDic.Add(VARIABLE.GetNiceName(), GetAllPrivateMemberInternal(VARIABLE.GetValue(targetObject), ReflectCount));
            }
        }

        public object GetAllPrivateMemberInternal(object targetObject, int recursiveCount)
        {
            if (recursiveCount <= 0 || targetObject == null) return targetObject;
            recursiveCount--;
            Dictionary<string, object> temp = new Dictionary<string, object>();
            //获取类中的非public字段
            FieldInfo[] privateFields = targetObject.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var VARIABLE in privateFields)
            {
                temp.Add(VARIABLE.GetNiceName(), GetAllPrivateMemberInternal(VARIABLE.GetValue(targetObject), recursiveCount));
            }

            //TODO 多层获取属性会报错，待处理
            return temp;
        }
    }
}