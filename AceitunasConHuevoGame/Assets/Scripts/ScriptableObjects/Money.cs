using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Money", menuName = "ScriptableObjects/Money")]
public class Money : ScriptableObject
{
    [SerializeField]
    private int initialMoney = 0;
    [SerializeField]
    private float initialMoneyUpgrade = 0f;
    public int moneyPerPerson;
    public int lotteryPrize;

    [HideInInspector]
    public int currentMoney;
    [HideInInspector]
    public float currentMoneyUpgrade;

    public void Initialize()
    {
        currentMoney = initialMoney;
        currentMoneyUpgrade = initialMoneyUpgrade;
    }
}
