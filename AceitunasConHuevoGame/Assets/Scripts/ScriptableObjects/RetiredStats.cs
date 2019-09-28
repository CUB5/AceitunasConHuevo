using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RetiredStats", menuName = "ScriptableObjects/RetiredStats")]
public class RetiredStats : ScriptableObject
{
    [SerializeField]
    private int initialPopulation = 0;

    public int currentPopulation;

    [Range(0f, 100f)]
    [SerializeField]
    private float initialDeathProb = 0f;

    public float currentDeathProb;

    [Range(0f, 100f)]
    [SerializeField]
    private float initialLotteryProb = 0f;

    public float currentLotteryProb;

    public void Initialize()
    {
        currentPopulation = initialPopulation;
        currentDeathProb = initialDeathProb;
        currentLotteryProb = initialLotteryProb;
    }
}
