namespace Box2DSharp.Ropes
{
    ///
    public class RopeTuning
    {
        public RopeTuning()
        {
            StretchingModel = StretchingModel.PbdStretchingModel;
            BendingModel = BendingModel.PbdAngleBendingModel;
            Damping = 0.0f;
            StretchStiffness = 1.0f;
            BendStiffness = 0.5f;
            BendHertz = 1.0f;
            BendDamping = 0.0f;
            Isometric = false;
            FixedEffectiveMass = false;
            WarmStart = false;
        }

        public StretchingModel StretchingModel;

        public BendingModel BendingModel;

        public float Damping;

        public float StretchStiffness;

        public float StretchHertz;

        public float StretchDamping;

        public float BendStiffness;

        public float BendHertz;

        public float BendDamping;

        public bool Isometric;

        public bool FixedEffectiveMass;

        public bool WarmStart;
    };
}