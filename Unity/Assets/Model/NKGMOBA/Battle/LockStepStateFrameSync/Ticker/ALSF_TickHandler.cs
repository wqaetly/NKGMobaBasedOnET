namespace ET
{
    public abstract class ALSF_TickHandler<T> : ILSF_TickHandler where T : Entity
    {
        public virtual void OnLSF_TickStart(T entity, uint frame, long deltaTime)
        {
        }

        public abstract void OnLSF_Tick(T entity, uint currentFrame, long deltaTime);

        public virtual void OnLSF_TickEnd(T entity, uint frame, long deltaTime)
        {
        }

        public void LSF_TickStart(Entity entity, uint frame, long deltaTime)
        {
            OnLSF_TickStart(entity as T, frame, deltaTime);
        }

        public void LSF_Tick(Entity entity, uint currentFrame, long deltaTime)
        {
            OnLSF_Tick(entity as T, currentFrame, deltaTime);
        }

        public void LSF_TickEnd(Entity entity, uint frame, long deltaTime)
        {
            OnLSF_TickEnd(entity as T, frame, deltaTime);
        }

        public virtual bool OnLSF_CheckConsistency(T entity, uint frame, ALSF_Cmd stateToCompare)
        {
            return true;
        }

#if !SERVER
        /// <summary>
        /// 视图层Tick，帧率可为60，90，120，144等
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deltaTime"></param>
        public virtual void OnLSF_ViewTick(T entity, long deltaTime)
        {
        }

        public virtual void OnLSF_RollBackTick(T entity, uint frame, ALSF_Cmd stateToCompare)
        {
        }

        /// <summary>
        /// 只有本地玩家才会进行一致性检测
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="frame"></param>
        /// <param name="stateToCompare"></param>
        /// <returns></returns>
        public bool LSF_CheckConsistency(Entity entity, uint frame, ALSF_Cmd stateToCompare)
        {
            Unit unit = entity.GetParent<Unit>();
            if (unit != null && unit != unit.BelongToRoom.GetComponent<UnitComponent>().MyUnit)
            {
                return true;
            }

            return OnLSF_CheckConsistency(entity as T, frame, stateToCompare);
        }

        /// <summary>
        /// 视图层Tick，帧率可为60，90，120，144等,只有本地玩家能在追帧的模式下进行tick
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="deltaTime"></param>
        public void LSF_ViewTick(Entity entity, long deltaTime)
        {
            Unit unit = entity.GetParent<Unit>();

            if (unit != null)
            {
                LSF_Component lsfComponent = unit.BelongToRoom.GetComponent<LSF_Component>();
                if (unit != unit.BelongToRoom.GetComponent<UnitComponent>().MyUnit && lsfComponent.IsInChaseFrameState)
                {
                    return;
                }
            }

            OnLSF_ViewTick(entity as T, deltaTime);
        }

        /// <summary>
        /// 只有本地玩家有回滚流程
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="frame"></param>
        /// <param name="stateToCompare"></param>
        public void LSF_RollBackTick(Entity entity, uint frame, ALSF_Cmd stateToCompare)
        {
            Unit unit = entity.GetParent<Unit>();

            if (unit != null && unit != unit.BelongToRoom.GetComponent<UnitComponent>().MyUnit)
            {
                return;
            }

            OnLSF_RollBackTick(entity as T, frame, stateToCompare);
        }
#endif
    }
}