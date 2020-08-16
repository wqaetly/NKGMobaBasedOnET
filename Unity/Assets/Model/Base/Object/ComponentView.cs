
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace ETModel
{
    public class ComponentView : SerializedMonoBehaviour
    {
        [HideInInspector] public object Component;

#if UNITY_EDITOR
        [InfoBox("注意，这将十分消耗性能。默认递归一次，可更改下面的递归次数来改变递归深度", InfoMessageType.Warning)]
        [LabelText("是否强制获取所有成员(递归)")]
        [OnValueChanged("TryGetAllPrivateMemberDeeply")]
        public bool GetAllPrivateMemberDeeply;

        [OnValueChanged("TryGetAllPrivateMemberDeeply")] [LabelText("父获取深度")] [PropertyRange(0, 6)]
        public int FatherCount = 0;

        [OnValueChanged("TryGetAllPrivateMemberDeeply")] [LabelText("反射深度")] [PropertyRange(0, 6)]
        public int ReflectCount = 0;

        [OnValueChanged("TryGetAllPrivateMemberDeeply")] [LabelText("反射深度_数组")] [PropertyRange(0, 6)]
        public int ReflectCount_Array = 0;

        [ShowIf("GetAllPrivateMemberDeeply")] [LabelText("类型名称")]
        public string TypeName;

        [ShowIf("GetAllPrivateMemberDeeply")] [LabelText("所有成员")]
        public Dictionary<string, object> AllMembers_Deeply;

        public void TryGetAllPrivateMemberDeeply()
        {
            if (this.GetAllPrivateMemberDeeply)
            {
                if (this.AllMembers_Deeply == null)
                {
                    this.AllMembers_Deeply = new Dictionary<string, object>();
                }

                this.AllMembers_Deeply.Clear();
                Type currentType = this.Component.GetType();
                TypeName = currentType.GetNiceName();
                StartGetAllMembersDeeply(this.AllMembers_Deeply, this.Component);
                Type fatherType = currentType.BaseType;
                for (int i = 0; i < FatherCount; i++)
                {
                    StartGetAllMembersDeeply(this.AllMembers_Deeply, this.Component, fatherType);
                    fatherType = fatherType.BaseType;
                }
            }
        }

        public void StartGetAllMembersDeeply(Dictionary<string, object> targetDic, object targetObject,
            Type targetType = null)
        {
            FieldInfo[] fields;
            if (targetType != null)
            {
                fields = targetType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            }
            else
            {
                //获取类中的字段
                fields = targetObject.GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            }

            foreach (var fieldInfo in fields)
            {
                targetDic[fieldInfo.GetNiceName()] =
                    GetAllMembersInternal(fieldInfo.GetValue(targetObject), ReflectCount);
            }

            //获取类中的屬性
            PropertyInfo[] propertyInfos = targetObject.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var propertyInfo in propertyInfos)
            {
                targetDic[propertyInfo.GetNiceName()] =
                    GetAllMembersInternal(propertyInfo.GetValue(targetObject), ReflectCount);
            }
        }

        public object GetAllMembersInternal(object targetObject, int recursiveCount)
        {
            if (recursiveCount <= 0 || targetObject == null) return targetObject;
            recursiveCount--;
            Dictionary<string, object> temp = targetObject.ObjectToMap(this.ReflectCount_Array);
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (var VARIABLE in temp)
            {
                result.Add(VARIABLE.Key, GetAllMembersInternal(VARIABLE.Value, recursiveCount));
            }

            if (result.Count == 0)
            {
                return targetObject;
            }

            return result;
        }
#endif
    }
}
