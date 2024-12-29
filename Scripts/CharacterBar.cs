using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterBar : MonoBehaviour
{
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI(gameObject.GetComponent<CharacterStats>().Health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI(int amount)
    {
        text.text = "HP: " + amount;
    }
}
