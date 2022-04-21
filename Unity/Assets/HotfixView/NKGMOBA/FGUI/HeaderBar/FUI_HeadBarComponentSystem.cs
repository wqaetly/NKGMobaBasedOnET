using FairyGUI;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class HeroHeadBarComponentAwakeSystem : AwakeSystem<HeroHeadBarComponent, FUI_HeadBar>
    {
        public override void Awake(HeroHeadBarComponent self, FUI_HeadBar headBar)
        {
            self.Hero = self.GetParent<Unit>();
            UnitAttributesDataComponent unitAttributesDataComponent =
                self.Hero.GetComponent<UnitAttributesDataComponent>();
            self.m_HeadBar = headBar;
            self.m_HeadBar.m_Bar_HP.self.value = unitAttributesDataComponent.GetAttribute(NumericType.MaxHp);
            self.m_HeadBar.m_Bar_MP.self.max = unitAttributesDataComponent.GetAttribute(NumericType.MaxMp);
            self.m_HeadBar.m_Bar_MP.self.value = unitAttributesDataComponent.GetAttribute(NumericType.MaxMp);

            self.m_HeadBarGapRender = self.m_HeadBar.m_Img_Gap.displayObject.gameObject.GetComponent<Renderer>();

            self.m_HeadBar.m_Img_Gap.material = new Material(XAssetLoader
                .LoadAsset<Material>(XAssetPathUtilities.GetMaterialPath("Mat_LifeBarGap")));

            //因为FGUI的GImage并不会在当前帧构建顶点数据，所以只能使用监听的方式
            self.m_HeadBar.m_Img_Gap.displayObject.graphics.meshModifier += InitHPBarGap;

            void InitHPBarGap()
            {
                self.SetDensityOfBar(self.Hero.GetComponent<UnitAttributesDataComponent>()
                    .GetAttribute(NumericType.MaxHp));
                self.m_HeadBar.m_Img_Gap.displayObject.graphics.meshModifier -= InitHPBarGap;
            }
        }
    }

    public class HeroHeadBarComponentUpdateSystem : LateUpdateSystem<HeroHeadBarComponent>
    {
        public override void LateUpdate(HeroHeadBarComponent self)
        {
            // 游戏物体的世界坐标转屏幕坐标
            self.m_Hero2Screen =
                Camera.main.WorldToScreenPoint(new Vector3(self.Hero.ViewPosition.x, self.Hero.ViewPosition.y,
                    self.Hero.ViewPosition.z));

            // 屏幕坐标转FGUI全局坐标
            self.m_HeadBarScreenPos.x = self.m_Hero2Screen.x;
            self.m_HeadBarScreenPos.y = Screen.height - self.m_Hero2Screen.y;

            // FGUI全局坐标转头顶血条本地坐标
            self.m_HeadBar.GObject.position = GRoot.inst.GlobalToLocal(self.m_HeadBarScreenPos);

            // 血条本地坐标修正
            self.m_HeadBar.GObject.x -= self.GetOffsetX(self.m_HeadBarScreenPos);
            self.m_HeadBar.GObject.y -= 180;
        }
    }


    public static class FUI_HeadBarComponentSystem
    {
        /// <summary>
        /// 得到偏移的x
        /// </summary>
        /// <param name="barPos">血条的屏幕坐标</param>
        /// <returns></returns>
        public static float GetOffsetX(this HeroHeadBarComponent self, Vector2 barPos)
        {
            float final = 100 + (Screen.width / 2.0f - barPos.x) * 0.05f;
            return final;
        }

        public static void SetDensityOfBar(this HeroHeadBarComponent self, float maxHP)
        {
            float actual = 0;
            if (maxHP % 100 - 0 <= 0.1f)
            {
                actual = maxHP / 100 + 1;
            }
            else
            {
                actual = maxHP / 100 + 2;
            }

            self.m_HeadBar.m_Bar_HP.self.max = maxHP;

            Vector2[] uv = self.m_HeadBar.m_Img_Gap.displayObject.gameObject.GetComponent<MeshFilter>().sharedMesh.uv;

            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

            self.m_HeadBarGapRender.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetFloat(HeroHeadBarComponent.UVStart, uv[0].x);
            materialPropertyBlock.SetFloat(HeroHeadBarComponent.UVFactor, 1 / (uv[2].x - uv[0].x));
            materialPropertyBlock.SetFloat(HeroHeadBarComponent.PerSplitWidth, 100 / (maxHP / 100));
            self.m_HeadBarGapRender.SetPropertyBlock(materialPropertyBlock);
        }
    }
}