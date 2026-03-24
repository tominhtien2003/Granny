using System.Collections.Generic;
using UnityEngine;

public static partial class Consts
{
	public static string game_id = "game_id";
	public static string game_lv = "game_lv";
	public static string check_point = "check_point";
	public static string time_played = "time_played";

	public const string MapID = "map_index";
	public const string SkinID = "skin_index";
	public const string RewardID = "reward_index";
	public const string PetID = "pet_index";
	public const string ItemName = "item_name";
	public const string ItemID = "item_index";
	public const string SpeedLevel = "speed_level";
	public const string EnergyLevel = "energy_level";
	public const string bodyPartID = "body_part_index";
	public const string ascendLevel = "ascend_level";

	public static string Rebirth = "Rebirth";
	public static string SlotID = "slot_id";

	public static string CheckPointID = "checkpoint_index";
	public static string GloveName = "glove_name";
	public static string RaceID = "race_map";
	public const string LuckyLevel = "lucky_level";

	public static string game = "game";

	public static string ComingSoon = "Coming Soon!";

	public static string coins = "coins";
	public static string gems = "gems";

	public static string boat = "boat";
	public static string wing = "wing";

    #region STEAL_BRAINROT

	public static string noti_machine = "noti_machine";


	public static string Sell = "SELL";
	public static string Buy = "BUY";
	public static string Steal = "STEAL";
	public static string offlineCash = "(Offline Cash: <color=green>${0}{1}</color>)";
	public static void Notify_FullSlot()
	{
		//UINotify.Instance.Notify(LocalizationManager.GetTranslation("notify_fullslot"));
	}
	public static void Notify_NeedMoster()
	{
		//UINotify.Instance.Notify(LocalizationManager.GetTranslation("notify_needmoster"));
	}
	public static void Notify_Gear_Money()
	{
		//UINotify.Instance.Notify(LocalizationManager.GetTranslation("notify_needmoney"));
	}
	public static void Notify_Stolen()
	{
		//UINotify.Instance.Notify(LocalizationManager.GetTranslation("notify_stolen"));
	}
	public static void Notify_BaseLocked()
	{
		//UINotify.Instance.Notify(LocalizationManager.GetTranslation("notify_baselocked"));
	}


	public static string Bonus_Text = "CASH MULTI x{0}";
	//public static string Bonus_Text = "bonus_text";

	public static Color Gold_Text_Color = new Color(1, .6f, 0, 1);
	public static Color RainBow_Text_Color = new Color(1, 0, .2f, 1);

	public static Color RainBow_Outlinable_Color = new Color(.45f, .25f, .45f, .5f);
	public static Color Lengendary_Outlinable_Color = new Color(1, 1, 0, 1f);

	public static Vector2 sizeIconCollection = new Vector2(174, 174);
	public static Vector2 sizeIconRebirth = new Vector2(90, 90);

	public static readonly int UVIndex = Shader.PropertyToID("_UVIndex");
	
	public static List<string> baseNames = new List<string>
	{
		"ALEX",
		"BLAKE",
		"CHRIS",
		"DANNY",
		"ELLA",
		"FINN",
		"GRACE",
		"HUNTER",
		"IVY",
		"JACK",
		"KATE",
		"LEO",
		"MIA",
		"NICK",
		"OLIVIA",
		"PETER",
		"QUINN",
		"RYAN",
		"SOPHIE",
		"TYLER",
		"UMA",
		"VICTOR",
		"WILLOW",
		"XANDER",
		"YARA",
		"ZACK",
		"LUNA",
		"NOAH",
		"BELLA",
		"JAYDEN"
	};

	private static int _countName;

	public static string BaseName()
	{
		if (baseNames.Count == 0) return "";
		string nameBase = baseNames[UnityEngine.Random.Range(0, baseNames.Count)];
		if (baseNames.Contains(nameBase)) baseNames.Remove(nameBase);
		_countName++;
		return nameBase;
	}
	
    #endregion

    #region SCHOOL RUN

	public const float School_SpeedInit = 0;
	public const float School_EnergyInit = 1000;
	public const float School_TimeTrain = 2f;

	public const float School_BaseDrain = 1;
	public const float School_SpeedDrainMultiplier = .03f;
	public const int School_IncreaseSpeed = 10;
	public const int School_LengthTrain = 50000;

    #endregion

	public static void NotiNoAds()
	{
		//if (UINotify.Instance)
		//{
		//	UINotify.Instance.Notify(no_ads);
		//}
	}

}
