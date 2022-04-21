using System.Net;

namespace ET
{
    public partial class StartProcessConfig
    {
        private IPEndPoint innerIPPort;

        public long SceneId;

        public IPEndPoint InnerIPPort
        {
            get
            {
                if (this.innerIPPort == null)
                {
                    this.innerIPPort = NetworkHelper.ToIPEndPoint($"{this.InnerIP}:{this.InnerPort}");
                }

                return this.innerIPPort;
            }
        }

        public string InnerIP => this.StartMachineConfig.InnerIP;

        public string OuterIP => this.StartMachineConfig.OuterIP;
        
        public string OuterIPForClient => this.StartMachineConfig.OutPortForClient;

        public StartMachineConfig StartMachineConfig
        {
            get
            {
                // 如果是开发模式，就加载开发模式的IP（本地IP）
                if (GlobalDefine.DevelopMode)
                {
                    return StartMachineConfigCategory.Instance.Get(2);
                }
                else
                {
                    return StartMachineConfigCategory.Instance.Get(this.MachineId);
                }
            }
        }

        public override void EndInit()
        {
            InstanceIdStruct instanceIdStruct = new InstanceIdStruct((int)this.Id, 0);
            this.SceneId = instanceIdStruct.ToLong();
            Log.Info($"StartProcess info: {this.MachineId} {this.Id} {this.SceneId}");
        }
    }
}