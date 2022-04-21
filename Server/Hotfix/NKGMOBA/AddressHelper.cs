using System.Collections.Generic;


namespace ET
{
	/// <summary>
	/// 可以方便的获取对应的StartSceneConfig
	/// </summary>
	public static class AddressHelper
	{
		/// <summary>
		/// 随机分配一个网关服
		/// </summary>
		/// <param name="zone"></param>
		/// <returns></returns>
		public static StartSceneConfig GetGate(int zone)
		{
			List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Gates[zone];
			
			int n = RandomHelper.RandomNumber(0, zoneGates.Count);

			return zoneGates[n];
		}
		
		/// <summary>
		/// 随机分配一个大厅服
		/// </summary>
		/// <param name="zone"></param>
		/// <returns></returns>
		public static StartSceneConfig GetLobby(int zone)
		{
			List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Lobbys[zone];
			
			int n = RandomHelper.RandomNumber(0, zoneGates.Count);

			return zoneGates[n];
		}
	}
}
