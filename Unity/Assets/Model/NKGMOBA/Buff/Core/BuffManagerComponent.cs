//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年9月16日 21:12:08
//------------------------------------------------------------

using System.Collections.Generic;

namespace ETModel
{
    [ObjectSystem]
    public class BuffManagerCompoenntUpdateSystem: UpdateSystem<BuffManagerComponent>
    {
        public override void Update(BuffManagerComponent self)
        {
            self.Update();
        }
    }

    public class BuffManagerComponent: Component
    {
        LinkedList<BuffBase> m_Buffs = new LinkedList<BuffBase>();

        public void Update()
        {
            //指向首链表地址
            LinkedListNode<BuffBase> current = m_Buffs.First;
            //轮询链表
            while (current != null)
            {
                BuffBase buff = current.Value;
                if (buff.MBuffState == BuffState.Waiting)
                {
                    buff.OnExecute();
                }
                else if(buff.MBuffState == BuffState.Running)
                {
                    buff.OnUpdate();
                }
                else
                {
                    buff.OnFinished();
                    current = current.Next;
                    LinkedListNode<BuffBase> next = current.Next;
                    m_Buffs.Remove(current);
                    current = next;
                    continue;
                }
                current = current.Next;
            }
        }

        public void AddBuff(BuffBase buff)
        {
            
            m_Buffs.AddLast(buff);
        }
    }
}