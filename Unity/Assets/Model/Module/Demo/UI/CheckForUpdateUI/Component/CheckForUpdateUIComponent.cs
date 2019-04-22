using UnityEngine;
using UnityEngine.UI;

namespace ETModel
{
	[ObjectSystem]
	public class CheckForUpdateUIComponentAwakeSystem : AwakeSystem<CheckForUpdateUIComponent>
	{
		public override void Awake(CheckForUpdateUIComponent self)
		{
			self.UpdateInfo = self.GetParent<UI>().GameObject.Get<GameObject>("UpdateInfo").GetComponent<Text>();
			self.UpdateSlider = self.GetParent<UI>().GameObject.Get<GameObject>("Slider").GetComponent<Slider>();
		}
	}

	[ObjectSystem]
	public class CheckForUpdateUIComponentStartSystem : StartSystem<CheckForUpdateUIComponent>
	{
		public override void Start(CheckForUpdateUIComponent self)
		{
			StartAsync(self).Coroutine();
		}
		
		public async ETVoid StartAsync(CheckForUpdateUIComponent self)
		{
			TimerComponent timerComponent = Game.Scene.GetComponent<TimerComponent>();
			long instanceId = self.InstanceId;
			while (true)
			{
				await timerComponent.WaitAsync(100);

				if (self.InstanceId != instanceId)
				{
					return;
				}

				BundleDownloaderComponent bundleDownloaderComponent = Game.Scene.GetComponent<BundleDownloaderComponent>();
				if (bundleDownloaderComponent == null)
				{
					continue;
				}

				if (bundleDownloaderComponent.CheckResBegin)
				{
					if (!bundleDownloaderComponent.CheckResCompleted)
					{
						self.UpdateInfo.text = "正在为您检查资源更新："+$"{bundleDownloaderComponent.CheckUpdateResProgress}%";
						self.UpdateSlider.value = bundleDownloaderComponent.CheckUpdateResProgress;
					}
					else
					{
						self.UpdateInfo.text = "正在为您更新资源："+$"{bundleDownloaderComponent.UpdateResProgress}%";
						self.UpdateSlider.value = bundleDownloaderComponent.UpdateResProgress;
					}
				}
			}
		}
	}

	public class CheckForUpdateUIComponent : Component
	{
		public Text UpdateInfo;
		public Slider UpdateSlider;

		public override void Dispose()
		{
			base.Dispose();
			this.UpdateInfo.text = "资源准备就绪，祝您游戏愉快";
			Log.Info(this.UpdateInfo.text);
		}
	}
}
