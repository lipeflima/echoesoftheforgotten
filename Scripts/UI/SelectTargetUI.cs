using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTargetUI : MonoBehaviour
{
    private Action onComplete;
    [SerializeField] private Button nextButton;
    private ActionData actionData;
    private TargetManager targetManager;
    private bool isPanelActive = false;

    private void Awake()
    {
        targetManager = FindObjectOfType<TargetManager>();
    }

    private void Update()
    {
        if (!isPanelActive) return;

        SelectTargetControlls();
    }

    public void Initialize(Action onCompleteCallback)
    {
        isPanelActive = true;
        gameObject.SetActive(true);
        onComplete = onCompleteCallback;
        nextButton.interactable = true; 
        nextButton.onClick.AddListener(CompleteStep);
    }

    private void CompleteStep()
    {
        isPanelActive = false;
        gameObject.SetActive(false);
        onComplete?.Invoke();
    }

    private void SelectTargetControlls()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // Tecla ESQ
        {
            targetManager.ChangeTarget(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) // Tecla DIR
        {
            targetManager.ChangeTarget(1);
        }
    }
}
