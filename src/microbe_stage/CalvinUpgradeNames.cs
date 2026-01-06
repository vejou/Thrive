using System;

/// <summary>
///   Names of upgrades for the various ATP processing methods ("Calvin" cycles)
/// </summary>
public static class CalvinUpgradeNames
{
    /// <summary>
    ///   Technically this is "none" but internally upgrades don't save the "none" upgrade in the upgrades list.
	/// </summary>
	public const string NOCALVIN_UPGRADE_NAME = "nocalvin";

	public const string GLUCOSE_UPGRADE_NAME = "glucose";

	public const string STARCH_UPGRADE_NAME = "starch";

	public static CalvinType GetCalvinTypeFromUpgrades(this IReadOnlyOrganelleUpgrades? upgrades, string organelle)
	{
		if (upgrades == null || upgrades.UnlockedFeatures.Count < 1)
			return CalvinType.NoCalvin;

		foreach (var feature in upgrades.UnlockedFeatures)
		{
			if (TryGetCalvinTypeFromName(feature, organelle, out var type))
				return type;
		}

		return CalvinType.NoCalvin;
	}

	public static CalvinType CalvinTypeFromName(string name, string organelle)
	{
		if (TryGetCalvinTypeFromName(name, organelle, out var result))
			return result;

        throw new ArgumentException("Name doesn't match any calvin upgrade name, name was " + name + " and organelle was " + organelle);
	}

	public static bool TryGetCalvinTypeFromName(string name, string organelle, out CalvinType type)
	{
		string name2 = name;
        if (name2 == "none")
		{
			var toxinDefinition = SimulationParameters.Instance.GetOrganelleType(organelle);
			name2 = toxinDefinition.DefaultUpgradeName;
		}
		switch (name2)
		{
			case GLUCOSE_UPGRADE_NAME:
				type = CalvinType.Glucose;
				return true;
			case STARCH_UPGRADE_NAME:
				type = CalvinType.Starch;
				return true;
			case NOCALVIN_UPGRADE_NAME:
				type = CalvinType.NoCalvin;
				return true;
		}
		type = CalvinType.NoCalvin;
		return false;
	}

	public static string CalvinNameFromType(CalvinType calvinType)
	{
		switch (calvinType)
		{
			case CalvinType.Glucose:
				return GLUCOSE_UPGRADE_NAME;
			case CalvinType.NoCalvin:
				return NOCALVIN_UPGRADE_NAME;
			case CalvinType.Starch:
				return STARCH_UPGRADE_NAME;
			default:
				throw new ArgumentOutOfRangeException(nameof(calvinType), calvinType, null);
		}
	}
}
