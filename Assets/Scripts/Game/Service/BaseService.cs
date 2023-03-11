namespace Game.Service
{
    public abstract class BaseService
    {
        /// <summary>
        /// 當玩家進入遊戲時呼叫
        /// i.e., 登入進到遊戲主畫面
        /// </summary>
        public abstract void OnEnterGame();

        /// <summary>
        /// 所有 Service OnEnterGame 結束後
        /// </summary>
        public abstract void OnAfterEnterGame();

        /// <summary>
        /// 切換場景前呼叫
        /// </summary>
        public abstract void OnSceneChange();

        /// <summary>
        /// 切換場景後呼叫
        /// </summary>
        public abstract void OnSceneChanged();

        /// <summary>
        /// 當玩家離開遊戲時呼叫
        /// i.e., 登出回到首頁
        /// </summary>
        public abstract void OnLeaveGame();

        /// <summary>
        /// 暫停/回復遊戲時呼叫
        /// i.e., 縮小/打開畫面
        /// </summary>
        public abstract void OnApplicationPause(bool pause);

        /// <summary>
        /// 結束遊戲時呼叫
        /// i.e., 關閉程式
        /// </summary>
        public abstract void OnApplicationQuit();
    }
}