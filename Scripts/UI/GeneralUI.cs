using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentActionState;
    [SerializeField] private TMP_Text currentAvailableEnergy;
    [SerializeField] private TMP_Text currentSpentEnergy;
    private ActionData actionData;

    public void Initialize(ActionData data)
    {
        actionData = data;
        InitializeEnemiesStatsUI();
        gameObject.SetActive(true);
        SetPlayerCurrentAvailableEnergyUI();
        SetCurrentSpentEnergyUI(actionData.PlayerTurnSpentEnergy);
    }

    public void SetCurrentActionState(string state)
    {
        currentActionState.text = state;
    }

    public void SetPlayerCurrentAvailableEnergyUI()
    {
        currentAvailableEnergy.text = $"Energy: {actionData.PlayerStats.Mana}";
    }

    public void SetCurrentSpentEnergyUI(int amount)
    {
        currentSpentEnergy.text = $"Spent Mana: {amount}";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void InitializeEnemiesStatsUI()
    {
        foreach(var enemy in actionData.EnemiesStats)
        {
           enemy.battlerGameobject.GetComponent<CharacterBar>().UpdateUI(enemy.Health);
        }
    }
}
