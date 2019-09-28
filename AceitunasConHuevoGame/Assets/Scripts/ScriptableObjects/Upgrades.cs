using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrades", menuName = "ScriptableObjects/Upgrades")]
public class Upgrades : ScriptableObject
{
    [Header("Death upgrades")]

    public float childrenDeathUpgrade;
    public float adultsDeathUpgrade;
    public float retiredDeathUpgrade;

    [Header("Going out upgrades")]

    public float childrenOutUpgrade;
    public float adultsOutUpgrade;

    [Header("Procreate upgrades")]

    public float adultsProcreateUpgrade;

    [Header("Next age upgrades")]

    public float childrenToAdultsUpgrade;
    public float adultsToRetiredUpgrade;

    [Header("Money upgrades")]

    public float moneyPerPersonUpgrade;
}
