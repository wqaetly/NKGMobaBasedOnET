//------------------------------------------------------------
// Author: 烟雨迷离半世殇
// Mail: 1778139321@qq.com
// Data: 2019年4月23日 21:24:10
//------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using System.IO;
using MonKey;

namespace ETEditor
{
    /// <summary>
    /// 右键的菜单小工具集合
    /// </summary>
    public class RightKeyTools
    {
        [Command("ETEditor_SplitPngToMultiple", "将图集中的小图导出成文件", DefaultValidation = DefaultValidation.AT_LEAST_ONE_ASSET, Category = "ETEditor")]
        static void ProcessToSprite()
        {
            if (Selection.activeObject == null) return;
            Texture2D image = Selection.activeObject as Texture2D; //获取选择的对象
            if (image == null) return;

            string imagePath = AssetDatabase.GetAssetPath(image);
            string rootPath = System.IO.Path.GetDirectoryName(imagePath); //获取路径名称
            TextureImporter texImp = AssetImporter.GetAtPath(imagePath) as TextureImporter; //获取图片的AssetImporter
            texImp.isReadable = true;
            texImp.SaveAndReimport();
            AssetDatabase.CreateFolder(rootPath, image.name); //创建文件夹
            foreach (SpriteMetaData metaData in texImp.spritesheet) //遍历小图集
            {
                Texture2D myimage = new Texture2D((int) metaData.rect.width, (int) metaData.rect.height);

                for (int y = (int) metaData.rect.y; y < metaData.rect.y + metaData.rect.height; y++) //Y轴像素
                {
                    for (int x = (int) metaData.rect.x; x < metaData.rect.x + metaData.rect.width; x++)
                        myimage.SetPixel(x - (int) metaData.rect.x, y - (int) metaData.rect.y, image.GetPixel(x, y));
                }

                if (myimage.format != TextureFormat.ARGB32 && myimage.format != TextureFormat.RGB24)
                {
                    Texture2D newTexture = new Texture2D(myimage.width, myimage.height);
                    newTexture.SetPixels(myimage.GetPixels(0), 0);
                    myimage = newTexture;
                }

                var pngData = myimage.EncodeToPNG();
                File.WriteAllBytes(rootPath + "/" + image.name + "/" + metaData.name + ".png", pngData);
            }

            AssetDatabase.Refresh();
        }
    }
}