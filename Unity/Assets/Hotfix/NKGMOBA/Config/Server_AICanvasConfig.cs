using ETModel;

namespace ETHotfix
{
	[Config((int)(AppType.Gate | AppType.Map))]
	public partial class Server_AICanvasConfigCategory : ACategory<Server_AICanvasConfig>
	{
	}

	public class Server_AICanvasConfig: IConfig
	{
		public long Id { get; set; }
		public long NPBehaveId;
	}
}
