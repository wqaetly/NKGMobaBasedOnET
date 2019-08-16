namespace ETModel
{
    public class SessionPlayerComponent: Component
    {
        public Player Player;

        public override void Dispose()
        {
            base.Dispose();
            Game.Scene.GetComponent<OnlineComponent>().Remove(this.Player.PlayerID);
        }
    }
}