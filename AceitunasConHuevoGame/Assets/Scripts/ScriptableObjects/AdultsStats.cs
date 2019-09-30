using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AdultsStats", menuName = "ScriptableObjects/AdultsStats")]
public class AdultsStats : ScriptableObject
{
    [SerializeField]
    private int initialPopulation = 0;
    public int currentPopulation;

    [Range(0f, 100f)]
    public float initialDeathProb = 0f;
    public float currentDeathProb;

    [Range(0f, 100f)]
    public float initialOutProb = 0f;
    public float currentOutProb;

    [Range(0f, 100f)]
    public float initialProcreateProb = 0f;
    public float currentProcreateProb;

    [Range(0f, 100f)]
    public float initialRetiredProb = 0f;
    public float currentRetiredProb;

    public void Initialize()
    {
        currentPopulation = initialPopulation;
        currentDeathProb = initialDeathProb;
        currentOutProb = initialOutProb;
        currentProcreateProb = initialProcreateProb;
        currentRetiredProb = initialRetiredProb;
    }
}

