using UnityEngine;

public static class GameData 
{
    private static int _metals=0;
    private static int _coins=0;
    private static int _gems=0;

    static GameData()
    {
        _metals = PlayerPrefs.GetInt("Metals", 0);
        _coins= PlayerPrefs.GetInt("Coins", 0);
        _gems = PlayerPrefs.GetInt("Gems", 0);
    }

    public static int Metals
    {
        get { return _metals; }
        set { PlayerPrefs.SetInt("Metals", (_metals = value)); }
    }

    public static int Coins
    {
        get { return _coins; }
        set { PlayerPrefs.SetInt("Coins", (_coins = value)); }
    }
    public static int Gems
    {
        get { return _gems; }
        set { PlayerPrefs.SetInt("Gems", (_gems = value)); }
    }
}
