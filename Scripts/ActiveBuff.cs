using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBuff
{
    public string StatName { get; set; } // Nome da estatística a ser alterada
    public float Value { get; set; } // Valor do buff
    public int RemainingTurns { get; set; } // Duração em turnos
}
