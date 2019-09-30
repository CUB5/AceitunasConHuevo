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

    [Header("Buildings animations")]

    [SerializeField]
    public GameObject[] firstBuildingsToHide = new GameObject[0];

    [SerializeField]
    public GameObject[] firstBuildingsToShow = new GameObject[0];

    [SerializeField]
    public GameObject[] firstEffectsObjects = new GameObject[0];

    [SerializeField]
    public GameObject[] secondBuildingsToHide = new GameObject[0];

    [SerializeField]
    public GameObject[] secondBuildingsToShow = new GameObject[0];

    [SerializeField]
    public GameObject[] secondEffectsObjects = new GameObject[0];

    [SerializeField]
    public GameObject[] thirdBuildingsToHide = new GameObject[0];

    [SerializeField]
    public GameObject[] thirdBuildingsToShow = new GameObject[0];

    [SerializeField]
    public GameObject[] thirdEffectsObjects = new GameObject[0];

    [SerializeField]
    public GameObject[] ultimateBuildingsToHide = new GameObject[0];

    [SerializeField]
    public GameObject[] ultimateBuildingsToShow = new GameObject[0];

    [SerializeField]
    public GameObject[] ultimateEffectsObjects = new GameObject[0];

    public void UpgradeSector()
    {
        currentLevel++;

        if (currentLevel == firstMilestoneLevel)
        {
            ShowHideEffectObjects(firstEffectsObjects, true);
            ShowHideEffectObjects(firstBuildingsToHide, false);
            ShowHideEffectObjects(firstBuildingsToShow, true);
            Repair();
        }

        if (currentLevel == secondMilestoneLevel)
        {
            ShowHideEffectObjects(secondEffectsObjects, true);
            ShowHideEffectObjects(secondBuildingsToHide, false);
            ShowHideEffectObjects(secondBuildingsToShow, true);
            Repair();
        }

        if (currentLevel == thirdMilestoneLevel)
        {
            ShowHideEffectObjects(thirdEffectsObjects, true);
            ShowHideEffectObjects(thirdBuildingsToHide, false);
            ShowHideEffectObjects(thirdBuildingsToShow, true);
            Repair();
        }

        if (currentLevel == ultimateMilestoneLevel)
        {
            ShowHideEffectObjects(ultimateEffectsObjects, true);
            ShowHideEffectObjects(ultimateBuildingsToHide, false);
            ShowHideEffectObjects(ultimateBuildingsToShow, true);
            Repair();
        }
    }

    private void ShowHideEffectObjects(GameObject[] objectsList, bool activeValue)
    {
        foreach (GameObject go in objectsList)
        {
            go.SetActive(activeValue);
        }
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
