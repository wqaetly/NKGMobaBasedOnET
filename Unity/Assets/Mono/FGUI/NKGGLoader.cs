//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年6月29日 18:21:36
//------------------------------------------------------------

using ET;
using FairyGUI;
using UnityEngine;

namespace ET
{
    /// <summary>
    /// 使用时对GLoader的Url赋值为目标纹理asset的ab包全路径即可
    /// </summary>
    public class NKGGLoader: GLoader
    {
        protected override void LoadExternal()
        {
            LoadSprite().Coroutine();
        }

        private async ETVoid LoadSprite()
        {
            Sprite sprite = await XAssetLoader.LoadAssetAsync<Sprite>(this.url);
            if (sprite != null)
                onExternalLoadSuccess(new NTexture(sprite));
            else
                onExternalLoadFailed();
        }

        protected override void FreeExternal(NTexture texture)
        {
            //释放外部载入的资源
            XAssetLoader.UnLoadAsset(this.url);
        }
    }
}