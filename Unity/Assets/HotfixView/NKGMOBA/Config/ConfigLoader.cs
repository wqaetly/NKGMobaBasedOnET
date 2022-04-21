using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class ConfigLoader: IConfigLoader
    {
        public void GetAllConfigBytes(Dictionary<string, byte[]> output)
        {
            var types = Game.EventSystem.GetTypes(typeof(ConfigAttribute));

            foreach (var kv in types)
            {
                output[kv.Name] = XAssetLoader.LoadAsset<TextAsset>(XAssetPathUtilities.GetNormalConfigPath(kv.Name)).bytes;
            }
        }

        public byte[] GetOneConfigBytes(string configName)
        {
            return XAssetLoader.LoadAsset<TextAsset>(XAssetPathUtilities.GetNormalConfigPath(configName)).bytes;;
        }
    }
}