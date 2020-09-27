namespace ETModel
{
	[Config((int)(AppType.ClientH |  AppType.ClientM))]
	public partial class Client_NPBehaveConfigCategory : ACategory<Client_NPBehaveConfig>
	{
	}

	public class Client_NPBehaveConfig: IConfig
	{
		public long Id { get; set; }
		public long NPBehaveId;
	}
}
