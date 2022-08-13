
namespace BlackTree
{
    public enum CDType
    {
        /// <summary>
        /// 게스트계정
        /// </summary>
        Guest = 1,
        /// <summary>
        /// Apple GameCenter
        /// </summary>
        IOS_GameCenter = 2,
        /// <summary>
        /// GooglePlay
        /// </summary>
        GooglePlay = 3,
    }
    public enum OSType
    {
        /// <summary>
        /// 안드로이드
        /// </summary>
        Android = 1,
        /// <summary>
        /// 아이폰
        /// </summary>
        IOS = 2,
        /// <summary>
        /// 윈도우 - Unity Editor
        /// </summary>
        Windows = 3,
    }
    public enum MarketType
    {
        GooglePlay = 1,
        IOS = 200,
    }
    public enum PushAllowedType
    {
        /// <summary>
        /// 모든푸시 거절
        /// </summary>
        Deny = 0,
        /// <summary>
        /// 모든푸시 허용
        /// </summary>
        Allowed = 1,
        /// <summary>
        /// 야간푸시 거절
        /// </summary>
        NightTimeDeny = 2,
    }

    public enum CycleType
    {
        none,
        day,
        week,
        month,
        year,
        limited,
    }
    public enum PriceType
    {
        payment,
        Free,
        Ad,
        pvp_point,
        Gem_Count,
        Gem,
        mileage,
    }
    public enum LayoutType
    {
        none,
        normal,
        ex,
    }
}
