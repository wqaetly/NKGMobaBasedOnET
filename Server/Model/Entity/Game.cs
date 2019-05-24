namespace ETModel
{
	public static class Game
	{
		private static Scene scene;

		public static Scene Scene
		{
			get
			{
				if (scene != null)
				{
					return scene;
				}
				scene = new Scene();
				return scene;
			}
		}

		private static EventSystem eventSystem;

		public static EventSystem EventSystem
		{
			get
			{
				return eventSystem ?? (eventSystem = new EventSystem());
			}
		}

		private static ObjectPool objectPool;

		public static ObjectPool ObjectPool
		{
			get
			{
				return objectPool ?? (objectPool = new ObjectPool());
			}
		}

		public static void Close()
		{
			scene.Dispose();
			scene = null;
			//Game.Close中EventSystem要在最后置空，否则Scene Dispose的时候会触发DestroySystem，再次new EventSystem
			objectPool = null;
			eventSystem = null;

		}
	}
}