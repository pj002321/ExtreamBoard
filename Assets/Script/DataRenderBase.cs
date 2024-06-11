using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRenderBase : MonoBehaviour
{
    [Header("Sheet Address")][SerializeField] public string associatedSheet = "";
    [Header("Sheet Name")][SerializeField] public string associatedWorksheet = "";
    [Header("Start Row Number")][SerializeField] public int START_ROW_LENGTH = 2;
    [Header("End Row Number")][SerializeField] public int END_ROW_LENGTH = 5;

}
