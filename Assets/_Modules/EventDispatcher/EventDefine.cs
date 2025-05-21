using UnityEngine;

public partial class EventDefine : IEventParam
{

    public struct OnLoadScene : IEventParam { }

    public struct OnGamePaused : IEventParam
    {
        public bool isPaused;
    }

    public struct OnWinGame : IEventParam { }
    public struct OnLoseGame : IEventParam { }
}