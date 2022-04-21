using UnityEngine;

namespace ET
{
    public class B2S_DebuggerComponentAwakeSystem : AwakeSystem<B2S_DebuggerComponent>
    {
        public override void Awake(B2S_DebuggerComponent self)
        {
            GameObject gameObject = new GameObject($"B2S_DebuggerComponent----{self.GetParent<Unit>().Id}");
            gameObject.transform.SetParent(self.GetParent<Unit>().GetComponent<GameObjectComponent>()
                .GameObject.transform);
            gameObject.transform.localPosition = Vector3.zero;
            self.GoSupportor = gameObject;
        }
    }

    public class B2S_DebuggerComponentFixedUpdate : FixedUpdateSystem<B2S_DebuggerComponent>
    {
        public override void FixedUpdate(B2S_DebuggerComponent self)
        {
            foreach (var tobeRemovedProcessor in self.TobeRemovedProcessors)
            {
                UnityEngine.Object.Destroy(self.AllLinerRendersDic[tobeRemovedProcessor].gameObject);
                self.AllLinerRendersDic.Remove(tobeRemovedProcessor);
                self.AllVexs.Remove(tobeRemovedProcessor);
            }

            self.TobeRemovedProcessors.Clear();

            foreach (var debuggerProcessor in self.AllLinerRendersDic)
            {
                if (debuggerProcessor.Key.IsDisposed)
                {
                    self.TobeRemovedProcessors.Add(debuggerProcessor.Key);
                }
                else
                {
                    self.RefreshBox2dDebugInfo(debuggerProcessor.Key);
                }
            }
        }
    }

    public static class B2S_DebuggerComponentSystems
    {
        public static B2S_DebuggerProcessor AddBox2dCollider(this B2S_DebuggerComponent self, Unit colliderUnit)
        {
            B2S_DebuggerProcessor b2SDebuggerProcessor;
            if (self.AllLinerRendersDic.TryGetValue(colliderUnit, out b2SDebuggerProcessor))
            {
                return b2SDebuggerProcessor;
            }
            else
            {
                GameObject gameObject = new GameObject();
                b2SDebuggerProcessor = gameObject.AddComponent<B2S_DebuggerProcessor>();
                gameObject.transform.SetParent(self.GoSupportor.transform);
                gameObject.transform.localPosition = Vector3.zero;
                B2S_ColliderComponent b2SColliderComponent = colliderUnit.GetComponent<B2S_ColliderComponent>();

                // 用于Transform变换的矩阵，方便计算缩放，旋转和平移
                Matrix4x4 transformMatrix4X4 = Matrix4x4.TRS(colliderUnit.Position,
                    colliderUnit.Rotation,
                    Vector3.one);
                Vector3[] finalVexs = null;
                switch (b2SColliderComponent.B2S_ColliderDataStructureBase)
                {
                    case B2S_BoxColliderDataStructure b2SBoxColliderDataStructure:
                        finalVexs = new Vector3[4];
                        finalVexs[0] = transformMatrix4X4.MultiplyPoint(new Vector3(
                            -b2SBoxColliderDataStructure.hx + b2SBoxColliderDataStructure.finalOffset.X, 1,
                            b2SBoxColliderDataStructure.hy + b2SBoxColliderDataStructure.finalOffset.Y));
                        finalVexs[1] = transformMatrix4X4.MultiplyPoint(new Vector3(
                            b2SBoxColliderDataStructure.hx + b2SBoxColliderDataStructure.finalOffset.X, 1,
                            b2SBoxColliderDataStructure.hy + b2SBoxColliderDataStructure.finalOffset.Y));
                        finalVexs[2] = transformMatrix4X4.MultiplyPoint(new Vector3(
                            -b2SBoxColliderDataStructure.hx + b2SBoxColliderDataStructure.finalOffset.X, 1,
                            -b2SBoxColliderDataStructure.hy + b2SBoxColliderDataStructure.finalOffset.Y));
                        finalVexs[3] = transformMatrix4X4.MultiplyPoint(new Vector3(
                            b2SBoxColliderDataStructure.hx + b2SBoxColliderDataStructure.finalOffset.X, 1,
                            -b2SBoxColliderDataStructure.hy + b2SBoxColliderDataStructure.finalOffset.Y));
                        break;
                    case B2S_CircleColliderDataStructure b2SCircleColliderDataStructure:
                        var step = Mathf.RoundToInt(360f / B2S_DebuggerComponent.CircleDrawPointCount);
                        finalVexs = new Vector3[B2S_DebuggerComponent.CircleDrawPointCount + 1];
                        for (int i = 0; i <= 360; i += step)
                        {
                            finalVexs[i / step] = transformMatrix4X4.MultiplyPoint(new Vector3(
                                b2SCircleColliderDataStructure.radius *
                                Mathf.Cos(i * 1.0f * Mathf.Deg2Rad) + b2SCircleColliderDataStructure.finalOffset.X, 1,
                                b2SCircleColliderDataStructure.radius *
                                Mathf.Sin(i * 1.0f * Mathf.Deg2Rad) + b2SCircleColliderDataStructure.finalOffset.Y));
                        }

                        break;

                    case B2S_PolygonColliderDataStructure b2SPolygonColliderDataStructure:
                        finalVexs = new Vector3[b2SPolygonColliderDataStructure.pointCount + 1];
                        int index = 0;
                        for (int i = 0; i < b2SPolygonColliderDataStructure.finalPoints.Count; i++)
                        {
                            for (int j = 0; j < b2SPolygonColliderDataStructure.finalPoints[i].Count; j++, index++)
                            {
                                finalVexs[index] = transformMatrix4X4.MultiplyPoint(new Vector3(
                                    b2SPolygonColliderDataStructure.finalPoints[i][j].X +
                                    b2SPolygonColliderDataStructure.finalOffset.X, 1,
                                    b2SPolygonColliderDataStructure.finalPoints[i][j].Y +
                                    b2SPolygonColliderDataStructure.finalOffset.Y));
                            }

                            if (i == b2SPolygonColliderDataStructure.finalPoints.Count - 1)
                            {
                                finalVexs[index] = transformMatrix4X4.MultiplyPoint(new Vector3(
                                    b2SPolygonColliderDataStructure.finalPoints[0][0].X +
                                    b2SPolygonColliderDataStructure.finalOffset.X, 1,
                                    b2SPolygonColliderDataStructure.finalPoints[0][0].Y +
                                    b2SPolygonColliderDataStructure.finalOffset.Y));
                            }
                        }

                        break;
                }

                self.AllVexs.Add(colliderUnit, finalVexs);
                b2SDebuggerProcessor.Init(finalVexs);
                self.AllLinerRendersDic.Add(colliderUnit, b2SDebuggerProcessor);
                return b2SDebuggerProcessor;
            }
        }

        public static void RefreshBox2dDebugInfo(this B2S_DebuggerComponent self, Unit colliderUnitToRefresh)
        {
            B2S_ColliderComponent b2SColliderComponent = colliderUnitToRefresh.GetComponent<B2S_ColliderComponent>();
            Vector3[] finalVexs = self.AllVexs[colliderUnitToRefresh];
            // 用于Transform变换的矩阵，方便计算缩放，旋转和平移
            Matrix4x4 transformMatrix4X4 = Matrix4x4.TRS(colliderUnitToRefresh.Position, colliderUnitToRefresh.Rotation,
                Vector3.one);

            switch (b2SColliderComponent.B2S_ColliderDataStructureBase)
            {
                case B2S_BoxColliderDataStructure b2SBoxColliderDataStructure:
                    finalVexs[0] = transformMatrix4X4.MultiplyPoint(new Vector3(
                        -b2SBoxColliderDataStructure.hx + b2SBoxColliderDataStructure.finalOffset.X, 1,
                        b2SBoxColliderDataStructure.hy + b2SBoxColliderDataStructure.finalOffset.Y));
                    finalVexs[1] = transformMatrix4X4.MultiplyPoint(new Vector3(
                        b2SBoxColliderDataStructure.hx + b2SBoxColliderDataStructure.finalOffset.X, 1,
                        b2SBoxColliderDataStructure.hy + b2SBoxColliderDataStructure.finalOffset.Y));
                    finalVexs[2] = transformMatrix4X4.MultiplyPoint(new Vector3(
                        -b2SBoxColliderDataStructure.hx + b2SBoxColliderDataStructure.finalOffset.X, 1,
                        -b2SBoxColliderDataStructure.hy + b2SBoxColliderDataStructure.finalOffset.Y));
                    finalVexs[3] = transformMatrix4X4.MultiplyPoint(new Vector3(
                        b2SBoxColliderDataStructure.hx + b2SBoxColliderDataStructure.finalOffset.X, 1,
                        -b2SBoxColliderDataStructure.hy + b2SBoxColliderDataStructure.finalOffset.Y));
                    break;
                case B2S_CircleColliderDataStructure b2SCircleColliderDataStructure:
                    var step = Mathf.RoundToInt(360f / B2S_DebuggerComponent.CircleDrawPointCount);
                    for (int i = 0; i <= 360; i += step)
                    {
                        finalVexs[i / step] = transformMatrix4X4.MultiplyPoint(new Vector3(
                            b2SCircleColliderDataStructure.radius *
                            Mathf.Cos(i * 1.0f * Mathf.Deg2Rad) + b2SCircleColliderDataStructure.finalOffset.X, 1,
                            b2SCircleColliderDataStructure.radius *
                            Mathf.Sin(i * 1.0f * Mathf.Deg2Rad) + b2SCircleColliderDataStructure.finalOffset.Y));
                    }

                    break;

                case B2S_PolygonColliderDataStructure b2SPolygonColliderDataStructure:
                    int index = 0;
                    for (int i = 0; i < b2SPolygonColliderDataStructure.finalPoints.Count; i++)
                    {
                        for (int j = 0; j < b2SPolygonColliderDataStructure.finalPoints[i].Count; j++, index++)
                        {
                            finalVexs[index] = transformMatrix4X4.MultiplyPoint(new Vector3(
                                b2SPolygonColliderDataStructure.finalPoints[i][j].X +
                                b2SPolygonColliderDataStructure.finalOffset.X, 1,
                                b2SPolygonColliderDataStructure.finalPoints[i][j].Y +
                                b2SPolygonColliderDataStructure.finalOffset.Y));
                        }

                        if (i == b2SPolygonColliderDataStructure.finalPoints.Count - 1)
                        {
                            finalVexs[index] = transformMatrix4X4.MultiplyPoint(new Vector3(
                                b2SPolygonColliderDataStructure.finalPoints[0][0].X +
                                b2SPolygonColliderDataStructure.finalOffset.X, 1,
                                b2SPolygonColliderDataStructure.finalPoints[0][0].Y +
                                b2SPolygonColliderDataStructure.finalOffset.Y));
                        }
                    }

                    break;
            }
        }
    }
}