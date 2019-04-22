using System;
using UnityEngine;

namespace ETModel
{
    public static class CheckForUpdateUIFactory
    {
        public static UI Create()
        {
	        try
	        {
				GameObject bundleGameObject = ((GameObject)ResourcesHelper.Load("KV")).Get<GameObject>(UIType.CheckForUpdate);
				GameObject go = UnityEngine.Object.Instantiate(bundleGameObject);
				go.layer = LayerMask.NameToLayer(LayerNames.UI);
				UI ui = ComponentFactory.Create<UI, string, GameObject>(UIType.CheckForUpdate, go, false);

				ui.AddComponent<CheckForUpdateUIComponent>();
				return ui;
	        }
	        catch (Exception e)
	        {
				Log.Error(e);
		        return null;
	        }
		}

	    public static void Remove(string type)
	    {
		    
	    }
    }
}