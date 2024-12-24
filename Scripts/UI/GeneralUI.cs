using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{
    [SerializeField] private TMP_Text currentActionState;

    public void Initialize()
    {
        gameObject.SetActive(true);
    }

    public void SetCurrentActionState(string state)
    {
        currentActionState.text = state;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
