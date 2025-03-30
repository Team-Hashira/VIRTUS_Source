namespace Hashira.EffectSystem
{
    public interface ILoopEffect
    {
        /// <summary>
        /// 스킬을 재시작할 때 세팅해야 할 것
        /// </summary>
        public void Reset();
    }
}
