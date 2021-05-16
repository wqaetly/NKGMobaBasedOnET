using System;
using System.Collections.Generic;
using System.Linq;

namespace ETModel
{
    public abstract class ACategoryBase : Object
    {
        public abstract Type ConfigType { get; }
    }

    public interface ICategory<T> where T : IConfig
    {
        T GetOne();
        List<T> GetAll();
        T TryGet(int type);
    }

    /// <summary>
    /// 管理该所有的配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ACategory<T> : ACategoryBase, ICategory<T> where T : IConfig
    {
        protected Dictionary<long, T> dict;

        public override void BeginInit()
        {
            this.dict = new Dictionary<long, T>();

            string configStr = ConfigHelper.GetText(typeof(T).Name);

            foreach (string str in configStr.Split(new[] {"\n"}, StringSplitOptions.None))
            {
                try
                {
                    string str2 = str.Trim();
                    if (str2 == "")
                    {
                        continue;
                    }

                    T t = ConfigHelper.ToObject<T>(str2);
                    this.dict.Add(t.Id, t);
                }
                catch (Exception e)
                {
                    throw new Exception($"parser json fail: {str}", e);
                }
            }
        }

        public override Type ConfigType
        {
            get { return typeof(T); }
        }

        public override void EndInit()
        {
        }

        /// <summary>
        /// 尝试获取一行配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T TryGet(int id)
        {
            if (this.dict.TryGetValue(id, out var config))
            {
                return  config;
            }

            return default;
        }

        /// <summary>
        /// 获取第一个配置
        /// </summary>
        /// <returns></returns>
        public T GetOne()
        {
            return  this.dict.Values.First();
        }

        /// <summary>
        /// 返回一个包含此分类所有的配置行
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            return this.dict.Values.ToList();
        }
    }
}