using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int maxTurnsNumber = 0;
    [SerializeField]
    private int turnsToIncreaseDeterioration = 0;

    [SerializeField]
    private Money money = null;

    [SerializeField]
    private ChildrenStats children = null;
    [SerializeField]
    private AdultsStats adults = null;
    [SerializeField]
    private RetiredStats retired = null;

    [SerializeField]
    private UpgradableSector education = null;
    [SerializeField]
    private UpgradableSector internalEconomy = null;
    [SerializeField]
    private UpgradableSector externalEconomy = null;

    [Range(0f, 1f)]
    [SerializeField]
    private float populationToUpdate = 0f;

    [SerializeField]
    private float evolutionTime = 0f;
    [SerializeField]
    private float timeToUpdatePopulation = 0f;
    [SerializeField]
    private float decisionTime = 0f;

    private float timeElapsed;
    private float updateTimeElapsed;
    private int currentTurn;
    private bool isEvolutionTime;
    private bool isGamePaused;
    private int updatedCount;

    private int edDetTurns;
    private int intEcoDetTurns;
    private int extEcoDetTurns;

    private int totalPopulation;

    private bool applyPassTurnBonus;

    // Start is called before the first frame update
    void Start()
    {
        children.Initialize();
        adults.Initialize();
        retired.Initialize();
        money.Initialize();

        UpdateTotalPopulation();

        timeElapsed = 0f;
        updateTimeElapsed = 0f;
        currentTurn = 0;

        isGamePaused = false;

        education.currentLevel = 0;
        internalEconomy.currentLevel = 0;
        externalEconomy.currentLevel = 0;

        NewTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGamePaused)
            timeElapsed += Time.deltaTime;

        if (isEvolutionTime)
        {
            updateTimeElapsed += Time.deltaTime;

            if (updateTimeElapsed >= timeToUpdatePopulation)
            {
                UpdatePopulation();
                updateTimeElapsed = 0f;
            }

            if (timeElapsed >= evolutionTime)
            {
                if (updatedCount < (evolutionTime / timeToUpdatePopulation))
                    UpdatePopulation();

                UpdateMoney();
                isEvolutionTime = false;
                updateTimeElapsed = 0f;
                timeElapsed = 0f;

                if (education.currentLevel >= 0)
                    edDetTurns++;
                if (internalEconomy.currentLevel >= 0)
                    edDetTurns++;
                if (education.currentLevel >= 0)
                    edDetTurns++;

                if (currentTurn < maxTurnsNumber)
                    NewTurn();
                else
                    GameOver();
            }
        }
        else
        {
            if (timeElapsed >= decisionTime)
            {
                applyPassTurnBonus = true;
                StartEvolutionPhase();
            }
        }
    }

    private void UpdateTotalPopulation()
    {
        totalPopulation = children.currentPopulation + adults.currentPopulation + retired.currentPopulation;
    }

    private void UpdatePopulation()
    {
        Debug.Log("UpdatePopulation at " + timeElapsed);

        UpdateChildren();
        UpdateAdults();
        UpdateRetired();

        UpdateTotalPopulation();

        updatedCount++;
    }

    private void NewTurn()
    {
        applyPassTurnBonus = false;
        isEvolutionTime = false;
        currentTurn++;
        Debug.Log("Turn " + currentTurn);
        Debug.Log("NewTurn at " + timeElapsed);

        // Check deterioration level
        if (edDetTurns >= turnsToIncreaseDeterioration)
            education.IncreaseDeterioration();
        if (intEcoDetTurns >= turnsToIncreaseDeterioration)
            internalEconomy.IncreaseDeterioration();
        if (extEcoDetTurns >= turnsToIncreaseDeterioration)
            externalEconomy.IncreaseDeterioration();

        updatedCount = 0;
    }

    private void StartEvolutionPhase()
    {
        Debug.Log("StartEvolutionPhase at " + timeElapsed);
        // Calculates the upgrades
        if (education.currentLevel >= 1)
            CalculateSectorUpgrades(education);
        if (internalEconomy.currentLevel >= 1)
            CalculateSectorUpgrades(internalEconomy);
        if (externalEconomy.currentLevel >= 1)
            CalculateSectorUpgrades(externalEconomy);

        timeElapsed = 0f;
        isEvolutionTime = true;
    }

    /// <summary>
    /// Applies the upgrades to the population based on the deterioration level of each sector
    /// </summary>
    /// <param name="upgrades"></param>
    /// <param name="deteriorationFactor"></param>
    private void ApplyUpgrades(Upgrades upgrades, float deteriorationFactor)
    {
        // CHILDREN
        children.currentDeathProb += upgrades.childrenDeathUpgrade * deteriorationFactor;
        children.currentOutProb += upgrades.childrenOutUpgrade * deteriorationFactor;
        children.currentAdultProb += upgrades.childrenToAdultsUpgrade * deteriorationFactor;

        // ADULTS
        adults.currentDeathProb += upgrades.adultsDeathUpgrade * deteriorationFactor;
        adults.currentOutProb += upgrades.adultsOutUpgrade * deteriorationFactor;
        adults.currentProcreateProb += upgrades.adultsProcreateUpgrade * deteriorationFactor;
        adults.currentRetiredProb += upgrades.adultsToRetiredUpgrade * deteriorationFactor;

        // RETIRED
        retired.currentDeathProb += upgrades.retiredDeathUpgrade * deteriorationFactor;
        retired.currentLotteryProb += upgrades.retiredDeathUpgrade * deteriorationFactor;

        // MONEY
        money.currentMoneyUpgrade += upgrades.moneyPerPersonUpgrade * deteriorationFactor;
    }

    /// <summary>
    /// Calculates the upgrades to apply to the population based on the milestones reached
    /// </summary>
    /// <param name="sector"></param>
    private void CalculateSectorUpgrades(UpgradableSector sector)
    {
        // FIRST MILESTONE            
        if (sector.currentLevel >= sector.firstMilestoneLevel)
        {
            switch (sector.currentDeteriorationLevel)
            {
                case UpgradableSector.DeteriorationLevel.PERFECT:
                    ApplyUpgrades(sector.firstMilestoneUpgrades, sector.perfect);
                    break;
                case UpgradableSector.DeteriorationLevel.NORMAL:
                    ApplyUpgrades(sector.firstMilestoneUpgrades, sector.normal);
                    break;
                default:
                    ApplyUpgrades(sector.firstMilestoneUpgrades, sector.ruined);
                    break;
            }

            // SECOND MILESTONE
            if (sector.currentLevel >= sector.secondMilestoneLevel)
            {
                switch (sector.currentDeteriorationLevel)
                {
                    case UpgradableSector.DeteriorationLevel.PERFECT:
                        ApplyUpgrades(sector.secondMilestoneUpgrades, sector.perfect);
                        break;
                    case UpgradableSector.DeteriorationLevel.NORMAL:
                        ApplyUpgrades(sector.secondMilestoneUpgrades, sector.normal);
                        break;
                    default:
                        ApplyUpgrades(sector.secondMilestoneUpgrades, sector.ruined);
                        break;
                }

                // THIRD MILESTONE
                if (sector.currentLevel >= sector.thirdMilestoneLevel)
                {
                    switch (sector.currentDeteriorationLevel)
                    {
                        case UpgradableSector.DeteriorationLevel.PERFECT:
                            ApplyUpgrades(sector.thirdMilestoneUpgrades, sector.perfect);
                            break;
                        case UpgradableSector.DeteriorationLevel.NORMAL:
                            ApplyUpgrades(sector.thirdMilestoneUpgrades, sector.normal);
                            break;
                        default:
                            ApplyUpgrades(sector.thirdMilestoneUpgrades, sector.ruined);
                            break;
                    }

                    // ULTIMATE MILESTONE
                    if (sector.currentLevel >= sector.ultimateMilestoneLevel)
                    {
                        switch (sector.currentDeteriorationLevel)
                        {
                            case UpgradableSector.DeteriorationLevel.PERFECT:
                                ApplyUpgrades(sector.ultimateMilestoneUpgrades, sector.perfect);
                                break;
                            case UpgradableSector.DeteriorationLevel.NORMAL:
                                ApplyUpgrades(sector.ultimateMilestoneUpgrades, sector.normal);
                                break;
                            default:
                                ApplyUpgrades(sector.ultimateMilestoneUpgrades, sector.ruined);
                                break;
                        }
                    }
                }
            }
        }
    }

    private void UpdateChildren()
    {
        int popToCheck = Mathf.FloorToInt(children.currentPopulation * populationToUpdate);
        for (int i = 0; i < popToCheck; i++)
        {
            float deathValue = Random.Range(1f, 100f);
            float outValue = Random.Range(1f, 100f);
            float adultValue = Random.Range(1f, 100f);

            if (deathValue <= children.currentDeathProb)
            {
                children.currentPopulation--;
            }
            else
            {
                if (outValue <= children.currentOutProb)
                {
                    children.currentPopulation--;
                }
                else
                {
                    if (adultValue <= children.currentAdultProb)
                    {
                        children.currentPopulation--;
                        adults.currentPopulation++;
                    }
                }
            }
        }
    }

    private void UpdateAdults()
    {
        int popToCheck = Mathf.FloorToInt(adults.currentPopulation * populationToUpdate);
        for (int i = 0; i < popToCheck; i++)
        {
            float deathValue = Random.Range(1f, 100f);
            float outValue = Random.Range(1f, 100f);
            float procreateValue = Random.Range(1f, 100f);
            float retiredValue = Random.Range(1f, 100f);

            if (deathValue <= adults.currentDeathProb)
            {
                adults.currentPopulation--;
            }
            else
            {
                if (outValue <= adults.currentOutProb)
                {
                    adults.currentPopulation--;
                }
                else
                {
                    if (procreateValue <= adults.currentProcreateProb)
                    {
                        children.currentPopulation++;
                    }

                    if (retiredValue <= adults.currentRetiredProb)
                    {
                        adults.currentPopulation--;
                        retired.currentPopulation++;
                    }
                }
            }
        }
    }

    private void UpdateRetired()
    {
        int popToCheck = Mathf.FloorToInt(retired.currentPopulation * populationToUpdate);
        for (int i = 0; i < popToCheck; i++)
        {
            float deathValue = Random.Range(1f, 100f);
            float lotteryValue = Random.Range(1f, 100f);

            if (deathValue <= retired.currentDeathProb)
            {
                retired.currentPopulation--;
            }
            else
            {
                if (lotteryValue <= retired.currentLotteryProb)
                {
                    money.currentMoney += money.lotteryPrize;
                }
            }
        }
    }

    private void UpdateMoney()
    {
        float moneyEarned = (money.moneyPerPerson * money.currentMoneyUpgrade) * totalPopulation;

        if (applyPassTurnBonus)
            moneyEarned *= money.passTurnBonus;

        money.currentMoney += (int)moneyEarned;
    }

    private void GameOver()
    {
        // TODO: GAME OVER
        isGamePaused = true;
    }

    #region UpgradeMethods
    public void UpgradeSector(UpgradableSector sector)
    {
        int cost = (sector.currentLevel + 1) * sector.levelUpgradeCost;

        if (sector.currentLevel < sector.maxLevel &&
            money.currentMoney >= cost)
        {
            sector.UpgradeSector();
            money.currentMoney -= cost;
            StartEvolutionPhase();
        }
    }

    public void RepairSector(UpgradableSector sector)
    {
        int cost = 0;

        switch (sector.currentDeteriorationLevel)
        {
            case UpgradableSector.DeteriorationLevel.NORMAL:
                cost = sector.repairingCostPerLevel;
                break;
            case UpgradableSector.DeteriorationLevel.RUINED:
                cost = sector.repairingCostPerLevel * 2;
                break;
            default:
                return;
        }

        if (money.currentMoney >= cost)
        {
            money.currentMoney -= cost;
            sector.Repair();

            if (sector.Equals(education))
                edDetTurns = 0;
            if (sector.Equals(internalEconomy))
                intEcoDetTurns = 0;
            if (sector.Equals(externalEconomy))
                extEcoDetTurns = 0;
        }
    }

    public void PassTurn()
    {
        applyPassTurnBonus = true;
        StartEvolutionPhase();
    }
    #endregion
}
