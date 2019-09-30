using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Money", menuName = "ScriptableObjects/Money")]
public class Money : ScriptableObject
{
    [SerializeField]
    private int initialMoney = 0;

    public float initialMoneyUpgrade = 0f;
    public int moneyPerPerson;
    public int lotteryPrize;
    public int currentMoney;
    public float currentMoneyUpgrade;
    public float passTurnBonus;

    public void Initialize()
    {
        currentMoney = initialMoney;
        currentMoneyUpgrade = initialMoneyUpgrade;
    }
}
