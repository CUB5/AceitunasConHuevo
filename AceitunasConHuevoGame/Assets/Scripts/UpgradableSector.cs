using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradableSector : MonoBehaviour
{
    public enum DeteriorationLevel
    {
        PERFECT,
        NORMAL,
        RUINED
    }

    public Upgrades firstMilestoneUpgrades;
    public Upgrades secondMilestoneUpgrades;
    public Upgrades thirdMilestoneUpgrades;
    public Upgrades ultimateMilestoneUpgrades;

    public int firstMilestoneLevel;
    public int secondMilestoneLevel;
    public int thirdMilestoneLevel;
    public int ultimateMilestoneLevel;

    public int maxLevel;

    public int levelUpgradeCost;
    public int repairingCostPerLevel;

    [Header("Deterioration")]

    [Range(0f, 1f)]
    public float perfect;
    [Range(0f, 1f)]
    public float normal;
    [Range(0f, 1f)]
    public float ruined;

    [HideInInspector]
    public int currentLevel;
    [HideInInspector]
    public DeteriorationLevel currentDeteriorationLevel;

    public void UpgradeSector()
    {
        currentLevel++;

        if (currentLevel == secondMilestoneLevel || currentLevel == thirdMilestoneLevel || currentLevel == ultimateMilestoneLevel)
            Repair();
    }

    public void IncreaseDeterioration()
    {
        switch (currentDeteriorationLevel)
        {
            case DeteriorationLevel.PERFECT:
                currentDeteriorationLevel = DeteriorationLevel.NORMAL;
                break;
            case DeteriorationLevel.NORMAL:
                currentDeteriorationLevel = DeteriorationLevel.RUINED;
                break;
            default:
                break;
        }
    }

    public void Repair()
    {
        currentDeteriorationLevel = DeteriorationLevel.PERFECT;
    }
}
