using System;
using System.Collections.Generic;

namespace ET
{
    public class MonoListComponent<T> : IDisposable
    {

        private bool isDispose;

        public static MonoListComponent<T> Create()
        {
            var listT = new MonoListComponent<T>();
            listT.isDispose = false;
            return listT;
        }

        public List<T> List { get; } = new List<T>();


        public void Dispose()
        {
            if (isDispose)
            {
                return;
            }
            isDispose = true;
            List.Clear();
        }
    }

}