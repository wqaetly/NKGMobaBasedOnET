using UnityEngine;

namespace ET
{
    /// <summary>
    /// 技能释放时获取方向，目标信息的辅助类
    /// 之所以采用屏幕空间的信息做运算，是因为如果使用射线的方式会受限于地面碰撞的位置和技能指示器高度的会产生一些误差
    /// </summary>
    public static class SkillCastDirAndTargetHelper
    {
        /// <summary>
        /// 根据鼠标和施法者位置信息得出此次技能释放方向
        /// </summary>
        /// <returns></returns>
        public static Quaternion GetQuaFromMouseAndCaster(UnityEngine.Vector3 pos)
        {
            var sspos = Camera.main.WorldToScreenPoint(pos);

            return Quaternion.LookRotation(
                new Vector3(sspos.x, 0, sspos.y) - new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y)
            );
        }
    }
}