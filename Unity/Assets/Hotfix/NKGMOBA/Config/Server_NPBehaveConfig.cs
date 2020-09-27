using ETModel;

namespace ETHotfix
{
	[Config((int)(AppType.Gate | AppType.Map))]
	public partial class Server_NPBehaveConfigCategory : ACategory<Server_NPBehaveConfig>
	{
	}

	public class Server_NPBehaveConfig: IConfig
	{
		public long Id { get; set; }
		public long NPBehaveId;
	}
}
