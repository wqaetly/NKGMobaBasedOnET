using ETModel;

namespace ETHotfix
{
	[Config((int)(AppType.Gate | AppType.Map))]
	public partial class Server_SkillCanvasConfigCategory : ACategory<Server_SkillCanvasConfig>
	{
	}

	public class Server_SkillCanvasConfig: IConfig
	{
		public long Id { get; set; }
		public long NPBehaveId;
		public long BelongToSkillId;
	}
}
