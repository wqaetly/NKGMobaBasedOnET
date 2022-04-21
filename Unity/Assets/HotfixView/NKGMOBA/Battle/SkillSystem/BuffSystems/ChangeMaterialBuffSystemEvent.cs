using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class ChangeMaterialBuffSystemExcuteEvent : AEvent<EventType.ChangeMaterialBuffSystemExcuteEvent>
    {
        protected override async ETTask Run(EventType.ChangeMaterialBuffSystemExcuteEvent a)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = a.Target.GetComponent<GameObjectComponent>().GameObject
                .Get<GameObject>("Materials").GetComponent<SkinnedMeshRenderer>();

            List<Material> currentMats = new List<Material>();
            skinnedMeshRenderer.GetSharedMaterials(currentMats);

            foreach (var changeMaterialName in a.ChangeMaterialBuffData.TheMaterialNameWillBeAdded)
            {
                currentMats.Add(a.Target.GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<ReferenceCollector>().Get<Material>(changeMaterialName));
            }

            skinnedMeshRenderer.sharedMaterials = currentMats.ToArray();

            await ETTask.CompletedTask;
        }
    }

    public class ChangeMaterialBuffSystemFinishEvent : AEvent<EventType.ChangeMaterialBuffSystemFinishEvent>
    {
        protected override async ETTask Run(EventType.ChangeMaterialBuffSystemFinishEvent a)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = a.Target.GetComponent<GameObjectComponent>().GameObject
                .Get<GameObject>("Materials").GetComponent<SkinnedMeshRenderer>();

            List<Material> currentMats = new List<Material>();
            skinnedMeshRenderer.GetSharedMaterials(currentMats);

            foreach (var changeMaterialName in a.ChangeMaterialBuffData.TheMaterialNameWillBeAdded)
            {
                for (int i = currentMats.Count - 1; i >= 0; i--)
                {
                    if (currentMats[i].name == changeMaterialName)
                    {
                        currentMats.RemoveAt(i);
                    }
                }
            }

            skinnedMeshRenderer.sharedMaterials = currentMats.ToArray();
            await ETTask.CompletedTask;
        }
    }
}