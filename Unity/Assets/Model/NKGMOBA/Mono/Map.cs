using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ETModel
{
    public class Map: MonoBehaviour, IPointerDownHandler
    {
        private PolygonCollider2D polygonCollider2D;
        private Vector3 mapSize;
        private Vector2 miniMapSize;
        private Vector3 target;

        public Transform m_Com;

        private void Awake()
        {
            polygonCollider2D = GetComponent<PolygonCollider2D>();
            mapSize = new Vector3(100f, 0.01f, 100f); //地图实体大小
            miniMapSize = new Vector2(300, 300); //小地图大小
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!polygonCollider2D.OverlapPoint(eventData.position))
            {
                return;
            }

            this.m_Com.position = eventData.position;
            Debug.Log("点击了地图,点击的点为" + eventData.position);
            SetTargetPosition(this.m_Com.localPosition);
        }

        void SetTargetPosition(Vector2 vector2)
        {
            //等比例求点击点在实体地图中的位置
            target.x = -(vector2.x + 150) * mapSize.x / miniMapSize.x;
            target.z = -(vector2.y + 150) * mapSize.z / miniMapSize.y;
            target.y = 0;
            Debug.Log("映射到实体地图上就是" + target);
            Game.EventSystem.Run(EventIdType.SmallMapPathFinder, this.target);
        }
    }
}