using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentActionState;
    [SerializeField] private TMP_Text currentAvailableEnergy;

    public void Initialize(ActionData actionData)
    {
        gameObject.SetActive(true);
        SetCurrentAvailableEnergy($"Energy: {actionData.PlayerStats.Mana}");
    }

    public void SetCurrentActionState(string state)
    {
        currentActionState.text = state;
    }

    public void SetCurrentAvailableEnergy(string amount)
    {
        currentAvailableEnergy.text = amount;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
