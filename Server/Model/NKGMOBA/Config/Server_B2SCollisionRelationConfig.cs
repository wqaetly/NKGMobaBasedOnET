namespace ETModel
{
	[Config((int)(AppType.Gate | AppType.Map))]
	public partial class Server_B2SCollisionRelationConfigCategory : ACategory<Server_B2SCollisionRelationConfig>
	{
	}

	public class Server_B2SCollisionRelationConfig: IConfig
	{
		public long Id { get; set; }
		public long B2S_CollisionRelationId;
	}
}
