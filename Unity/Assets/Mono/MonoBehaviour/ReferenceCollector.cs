using System;
using System.Collections.Generic;
using UnityEngine;
//Object并非C#基础中的Object，而是 UnityEngine.Object
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;

#endif

//使其能在Inspector面板显示，并且可以被赋予相应值
[Serializable]
public class ReferenceCollectorData
{
	public string key;
    //Object并非C#基础中的Object，而是 UnityEngine.Object
    public Object gameObject;
}
//继承IComparer对比器，Ordinal会使用序号排序规则比较字符串，因为是byte级别的比较，所以准确性和性能都不错
public class ReferenceCollectorDataComparer: IComparer<ReferenceCollectorData>
{
	public int Compare(ReferenceCollectorData x, ReferenceCollectorData y)
	{
		return string.Compare(x.key, y.key, StringComparison.Ordinal);
	}
}

//继承ISerializationCallbackReceiver后会增加OnAfterDeserialize和OnBeforeSerialize两个回调函数，如果有需要可以在对需要序列化的东西进行操作
//ET在这里主要是在OnAfterDeserialize回调函数中将data中存储的ReferenceCollectorData转换为dict中的Object，方便之后的使用
//注意UNITY_EDITOR宏定义，在编译以后，部分编辑器相关函数并不存在
public class ReferenceCollector: MonoBehaviour, ISerializationCallbackReceiver
{
    //用于序列化的List
	public List<ReferenceCollectorData> data = new List<ReferenceCollectorData>();
    //Object并非C#基础中的Object，而是 UnityEngine.Object
    private readonly Dictionary<string, Object> dict = new Dictionary<string, Object>();

#if UNITY_EDITOR
    //添加新的元素
	public void Add(string key, Object obj)
	{
		int i;
        //遍历data，看添加的数据是否存在相同key
		for (i = 0; i < data.Count; i++)
		{
			if (data[i].key == key)
			{
				break;
			}
		}
        //不等于data.Count意为已经存在于data List中，直接赋值即可
        if (i != data.Count)
        {
	        data[i].gameObject = obj;
        }
		else
		{
			data.Add(new ReferenceCollectorData(){key = key, gameObject = obj});
		}
        //应用与更新
		EditorUtility.SetDirty(this);
		AssetDatabase.SaveAssetIfDirty(this);
	}
    //删除元素，知识点与上面的添加相似
	public void Remove(string key)
	{
		int i;
		for (i = 0; i < data.Count; i++)
		{
			if (data[i].key == key)
			{
				break;
			}
		}
		if (i != data.Count)
		{
			this.data.RemoveAt(i);
		}
		EditorUtility.SetDirty(this);
		AssetDatabase.SaveAssetIfDirty(this);
	}

	public void Clear()
	{
		this.data.Clear();
		EditorUtility.SetDirty(this);
		AssetDatabase.SaveAssetIfDirty(this);
	}

	public void RenameTargetReferenceCollectorData()
	{
		
	}
	
	public void Sort()
	{
		data.Sort(new ReferenceCollectorDataComparer());
		EditorUtility.SetDirty(this);
		AssetDatabase.SaveAssetIfDirty(this);
	}
	
	public void DelNullReference()
	{
		for (int i = data.Count - 1; i >= 0; i--)
		{
			if (data[i].gameObject == null)
			{
				Remove(data[i].key);
			}
		}
	}
#endif
    //使用泛型返回对应key的gameobject
	public T Get<T>(string key) where T : class
	{
		Object dictGo;
		if (!dict.TryGetValue(key, out dictGo))
		{
			return null;
		}
		return dictGo as T;
	}

	public Object GetObject(string key)
	{
		Object dictGo;
		if (!dict.TryGetValue(key, out dictGo))
		{
			return null;
		}
		return dictGo;
	}

	public void OnBeforeSerialize()
	{
	}
    //在反序列化后运行
	public void OnAfterDeserialize()
	{
		dict.Clear();
		foreach (ReferenceCollectorData referenceCollectorData in data)
		{
			if (!dict.ContainsKey(referenceCollectorData.key))
			{
				dict.Add(referenceCollectorData.key, referenceCollectorData.gameObject);
			}
		}
	}
}
