using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private float initialPopToUpdate = 0f;

    [SerializeField]
    private float evolutionTime = 0f;
    [SerializeField]
    private float timeToUpdatePopulation = 0f;
    [SerializeField]
    private float decisionTime = 0f;

    [SerializeField]
    private float maxChildrenBar = 0f;
    [SerializeField]
    private float maxAdultsBar = 0f;
    [SerializeField]
    private float maxRetiredBar = 0f;
    [SerializeField]
    private float maxTotalBar = 0f;

    [Header("UI")]

    [SerializeField]
    private Text educationCostText = null;
    [SerializeField]
    private Text internalEconomyCostText = null;
    [SerializeField]
    private Text externalEconomyCostText = null;

    [SerializeField]
    private Text childrenText = null;
    [SerializeField]
    private Text adultsText = null;
    [SerializeField]
    private Text retiredText = null;
    [SerializeField]
    private Text totalText = null;

    [SerializeField]
    private Image childrenBar = null;
    [SerializeField]
    private Image adultsBar = null;
    [SerializeField]
    private Image retiredBar = null;
    [SerializeField]
    private Image totalBar = null;

    [SerializeField]
    private Text turnsText = null;
    [SerializeField]
    private Text moneyText = null;
    [SerializeField]
    private Text pauseText = null;

    [SerializeField]
    private Image pauseButtonImg = null;
    [SerializeField]
    private Sprite playSprite = null;
    [SerializeField]
    private Sprite pauseSprite = null;

    [SerializeField]
    private Button upgradeEducationButton = null;
    [SerializeField]
    private Button upgradeIntEconomyButton = null;
    [SerializeField]
    private Button upgradeExtEconomyButton = null;

    [SerializeField]
    private Button repairEducationButton = null;
    [SerializeField]
    private Button repairIntEconomyButton = null;
    [SerializeField]
    private Button repairExtEconomyButton = null;

    [SerializeField]
    private Text educationLevelText = null;
    [SerializeField]
    private Text intEcoLevelText = null;
    [SerializeField]
    private Text extEcoLevelText = null;

    [SerializeField]
    private Image educationBar = null;
    [SerializeField]
    private Image intEcoBar = null;
    [SerializeField]
    private Image extEcoBar = null;

    [SerializeField]
    private Sprite perfectSprite = null;
    [SerializeField]
    private Sprite normalSprite = null;
    [SerializeField]
    private Sprite ruinedSprite = null;

    [SerializeField]
    private GameObject[] gameOverMenus = new GameObject[0];

    private float timeElapsed;
    private float updateTimeElapsed;
    private int currentTurn;
    private bool isEvolutionTime;
    private bool isGamePaused;
    private int updatedCount;

    private float populationToUpdate = 0f;

    private int edDetTurns = 0;
    private int intEcoDetTurns = 0;
    private int extEcoDetTurns = 0;

    private int totalPopulation;

    private bool applyPassTurnBonus;

    // Start is called before the first frame update
    void Start()
    {
        populationToUpdate = initialPopToUpdate;

        children.Initialize();
        adults.Initialize();
        retired.Initialize();
        money.Initialize();

        childrenText.text = children.currentPopulation.ToString();
        adultsText.text = adults.currentPopulation.ToString();
        retiredText.text = retired.currentPopulation.ToString();

        childrenBar.fillAmount = Mathf.Clamp(children.currentPopulation / maxChildrenBar, 0f, 1f);
        adultsBar.fillAmount = Mathf.Clamp(adults.currentPopulation / maxAdultsBar, 0f, 1f);
        retiredBar.fillAmount = Mathf.Clamp(retired.currentPopulation / maxRetiredBar, 0f, 1f);

        UpdateTotalPopulation();

        moneyText.text = string.Format("{0} €", money.currentMoney);

        educationCostText.text = string.Format("{0} €", (education.currentLevel + 1) * education.levelUpgradeCost);
        internalEconomyCostText.text = string.Format("{0} €", (internalEconomy.currentLevel + 1) * internalEconomy.levelUpgradeCost);
        externalEconomyCostText.text = string.Format("{0} €", (externalEconomy.currentLevel + 1) * externalEconomy.levelUpgradeCost);

        educationLevelText.text = education.currentLevel.ToString();
        intEcoLevelText.text = internalEconomy.currentLevel.ToString();
        extEcoLevelText.text = externalEconomy.currentLevel.ToString();

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
        // CHEAT
        if (Input.GetKeyDown(KeyCode.P))
        {
            money.currentMoney += 500000;
        }

        if (money.currentMoney < (education.currentLevel + 1) * education.levelUpgradeCost)
            educationCostText.color = Color.red;
        else
            educationCostText.color = Color.yellow;

        if (money.currentMoney < (internalEconomy.currentLevel + 1) * internalEconomy.levelUpgradeCost)
            internalEconomyCostText.color = Color.red;
        else
            internalEconomyCostText.color = Color.yellow;

        if (money.currentMoney < (externalEconomy.currentLevel + 1) * externalEconomy.levelUpgradeCost)
            externalEconomyCostText.color = Color.red;
        else
            externalEconomyCostText.color = Color.yellow;

        if (education.currentDeteriorationLevel == UpgradableSector.DeteriorationLevel.PERFECT)
        {
            educationBar.sprite = perfectSprite;
        }
        if (education.currentDeteriorationLevel == UpgradableSector.DeteriorationLevel.NORMAL)
        {
            educationBar.sprite = normalSprite;
        }
        if (education.currentDeteriorationLevel == UpgradableSector.DeteriorationLevel.RUINED)
        {
            educationBar.sprite = ruinedSprite;
        }

        if (internalEconomy.currentDeteriorationLevel == UpgradableSector.DeteriorationLevel.PERFECT)
        {
            intEcoBar.sprite = perfectSprite;
        }
        if (internalEconomy.currentDeteriorationLevel == UpgradableSector.DeteriorationLevel.NORMAL)
        {
            intEcoBar.sprite = normalSprite;
        }
        if (internalEconomy.currentDeteriorationLevel == UpgradableSector.DeteriorationLevel.RUINED)
        {
            intEcoBar.sprite = ruinedSprite;
        }

        if (externalEconomy.currentDeteriorationLevel == UpgradableSector.DeteriorationLevel.PERFECT)
        {
            extEcoBar.sprite = perfectSprite;
        }
        if (externalEconomy.currentDeteriorationLevel == UpgradableSector.DeteriorationLevel.NORMAL)
        {
            extEcoBar.sprite = normalSprite;
        }
        if (externalEconomy.currentDeteriorationLevel == UpgradableSector.DeteriorationLevel.RUINED)
        {
            extEcoBar.sprite = ruinedSprite;
        }

        if (!isGamePaused)
            timeElapsed += Time.deltaTime;
        else
            return;

        if (isEvolutionTime)
        {
            upgradeEducationButton.image.fillAmount = Mathf.Clamp(timeElapsed / evolutionTime, 0f, 1f);
            upgradeIntEconomyButton.image.fillAmount = Mathf.Clamp(timeElapsed / evolutionTime, 0f, 1f);
            upgradeExtEconomyButton.image.fillAmount = Mathf.Clamp(timeElapsed / evolutionTime, 0f, 1f);
            repairEducationButton.image.fillAmount = Mathf.Clamp(timeElapsed / evolutionTime, 0f, 1f);
            repairIntEconomyButton.image.fillAmount = Mathf.Clamp(timeElapsed / evolutionTime, 0f, 1f);
            repairExtEconomyButton.image.fillAmount = Mathf.Clamp(timeElapsed / evolutionTime, 0f, 1f);

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

                if (education.currentLevel > 0)
                    edDetTurns++;
                if (internalEconomy.currentLevel > 0)
                    intEcoDetTurns++;
                if (externalEconomy.currentLevel > 0)
                    extEcoDetTurns++;

                if (currentTurn < maxTurnsNumber)
                    NewTurn();
                else
                    GameOver(0);
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

        totalText.text = totalPopulation.ToString();
        totalBar.fillAmount = Mathf.Clamp(totalPopulation / maxTotalBar, 0f, 1f);
    }

    private void UpdatePopulation()
    {
        UpdateChildren();
        UpdateAdults();
        UpdateRetired();

        UpdateTotalPopulation();

        updatedCount++;
    }

    private void NewTurn()
    {
        if (totalPopulation <= 0)
        {
            GameOver(1);
        }

        applyPassTurnBonus = false;
        isEvolutionTime = false;
        currentTurn++;

        //if (currentTurn == 10 || currentTurn == 20 || currentTurn == 30)
        //    populationToUpdate += initialPopToUpdate;


        turnsText.text = string.Format("{0}/{1}", currentTurn, maxTurnsNumber);

        // Check deterioration level
        if (edDetTurns >= turnsToIncreaseDeterioration)
        {
            education.IncreaseDeterioration();
            edDetTurns = 0;
        }
        if (intEcoDetTurns >= turnsToIncreaseDeterioration)
        {
            internalEconomy.IncreaseDeterioration();
            intEcoDetTurns = 0;
        }
        if (extEcoDetTurns >= turnsToIncreaseDeterioration)
        {
            externalEconomy.IncreaseDeterioration();
            extEcoDetTurns = 0;
        }

        updatedCount = 0;
    }

    private void StartEvolutionPhase()
    {
        children.currentDeathProb = children.initialDeathProb;
        children.currentOutProb = children.initialOutProb;
        children.currentAdultProb = children.initialAdultProb;

        adults.currentDeathProb = adults.initialDeathProb;
        adults.currentOutProb = adults.initialOutProb;
        adults.currentProcreateProb = adults.initialProcreateProb;
        adults.currentRetiredProb = adults.initialRetiredProb;

        retired.currentDeathProb = retired.initialDeathProb;
        retired.currentLotteryProb = retired.initialLotteryProb;

        money.currentMoneyUpgrade = money.initialMoneyUpgrade;

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
        int popToCheck = children.currentPopulation <= 5 ?
            children.currentPopulation :
            (int)Mathf.Clamp(Mathf.FloorToInt(children.currentPopulation * populationToUpdate) + 1, 0f, children.currentPopulation);
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

        childrenText.text = children.currentPopulation.ToString();
        childrenBar.fillAmount = Mathf.Clamp(children.currentPopulation / maxChildrenBar, 0f, 1f);
    }

    private void UpdateAdults()
    {
        int popToCheck = adults.currentPopulation <= 5 ?
            adults.currentPopulation :
            (int)Mathf.Clamp(Mathf.FloorToInt(adults.currentPopulation * populationToUpdate) + 1, 0f, adults.currentPopulation);
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

        adultsText.text = adults.currentPopulation.ToString();
        adultsBar.fillAmount = Mathf.Clamp(adults.currentPopulation / maxAdultsBar, 0f, 1f);
    }

    private void UpdateRetired()
    {
        int popToCheck = retired.currentPopulation <= 5 ?
            retired.currentPopulation :
            (int)Mathf.Clamp(Mathf.FloorToInt(retired.currentPopulation * populationToUpdate) + 1, 0f, retired.currentPopulation);
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

        retiredText.text = retired.currentPopulation.ToString();
        retiredBar.fillAmount = Mathf.Clamp(retired.currentPopulation / maxRetiredBar, 0f, 1f);
    }

    private void UpdateMoney()
    {
        float moneyEarned = (money.moneyPerPerson * money.currentMoneyUpgrade) * totalPopulation;

        if (applyPassTurnBonus)
            moneyEarned *= money.passTurnBonus;

        money.currentMoney += (int)moneyEarned;

        moneyText.text = string.Format("{0} €", money.currentMoney);
    }

    private void GameOver(int finalIndex)
    {
        isGamePaused = true;

        gameOverMenus[finalIndex].SetActive(true);
    }

    #region UpgradeMethods
    public void UpgradeSector(UpgradableSector sector)
    {
        if (isEvolutionTime)
            return;

        int cost = (sector.currentLevel + 1) * sector.levelUpgradeCost;

        if (sector.currentLevel < sector.maxLevel &&
            money.currentMoney >= cost)
        {
            sector.UpgradeSector();

            if (sector.Equals(education))
            {
                educationCostText.text = string.Format("{0} €", (education.currentLevel + 1) * education.levelUpgradeCost);
                educationLevelText.text = education.currentLevel.ToString();
            }
            if (sector.Equals(internalEconomy))
            {
                internalEconomyCostText.text = string.Format("{0} €", (internalEconomy.currentLevel + 1) * internalEconomy.levelUpgradeCost);
                intEcoLevelText.text = internalEconomy.currentLevel.ToString();
            }
            if (sector.Equals(externalEconomy))
            {
                externalEconomyCostText.text = string.Format("{0} €", (externalEconomy.currentLevel + 1) * externalEconomy.levelUpgradeCost);
                extEcoLevelText.text = externalEconomy.currentLevel.ToString();
            }

            if (sector.currentLevel == sector.firstMilestoneLevel || sector.currentLevel == sector.secondMilestoneLevel
                || sector.currentLevel == sector.thirdMilestoneLevel || sector.currentLevel == sector.ultimateMilestoneLevel)
            {
                sector.Repair();

                if (sector.Equals(education))
                    edDetTurns = 0;
                if (sector.Equals(internalEconomy))
                    intEcoDetTurns = 0;
                if (sector.Equals(externalEconomy))
                    extEcoDetTurns = 0;
            }

            money.currentMoney -= cost;
            moneyText.text = string.Format("{0} €", money.currentMoney);
            StartEvolutionPhase();
        }
    }

    public void RepairSector(UpgradableSector sector)
    {
        if (isEvolutionTime || sector.currentLevel <= 0)
            return;

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
            moneyText.text = string.Format("{0} €", money.currentMoney);
            sector.Repair();

            if (sector.Equals(education))
                edDetTurns = 0;
            if (sector.Equals(internalEconomy))
                intEcoDetTurns = 0;
            if (sector.Equals(externalEconomy))
                extEcoDetTurns = 0;

            StartEvolutionPhase();
        }
    }

    public void PassTurn()
    {
        applyPassTurnBonus = true;
        StartEvolutionPhase();
    }

    public void PauseResume()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            pauseText.gameObject.SetActive(true);
            pauseButtonImg.sprite = playSprite;

            upgradeEducationButton.enabled = false;
            upgradeIntEconomyButton.enabled = false;
            upgradeExtEconomyButton.enabled = false;

            repairEducationButton.enabled = false;
            repairIntEconomyButton.enabled = false;
            repairExtEconomyButton.enabled = false;
        }
        else
        {
            pauseText.gameObject.SetActive(false);
            pauseButtonImg.sprite = pauseSprite;

            upgradeEducationButton.enabled = true;
            upgradeIntEconomyButton.enabled = true;
            upgradeExtEconomyButton.enabled = true;

            repairEducationButton.enabled = true;
            repairIntEconomyButton.enabled = true;
            repairExtEconomyButton.enabled = true;
        }
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
}
