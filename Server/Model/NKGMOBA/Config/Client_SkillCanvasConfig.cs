namespace ETModel
{
	[Config((int)(AppType.ClientH |  AppType.ClientM))]
	public partial class Client_SkillCanvasConfigCategory : ACategory<Client_SkillCanvasConfig>
	{
	}

	public class Client_SkillCanvasConfig: IConfig
	{
		public long Id { get; set; }
		public long NPBehaveId;
		public long BelongToSkillId;
	}
}
