using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChildrenStats", menuName = "ScriptableObjects/ChildrenStats")]
public class ChildrenStats : ScriptableObject
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
    private float initialOutProb = 0f;

    public float currentOutProb;

    [Range(0f, 100f)]
    [SerializeField]
    private float initialAdultProb = 0f;

    public float currentAdultProb;

    public void Initialize()
    {
        currentPopulation = initialPopulation;
        currentDeathProb = initialDeathProb;
        currentOutProb = initialOutProb;
        currentAdultProb = initialAdultProb;
    }
}
