using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRenderBase : MonoBehaviour
{
    [Header("시트의 주소")][SerializeField] public string associatedSheet = "1IszLCglWtg5SZp7QOguPS5WbcfZxyQJWk9XkpoIJ0dY";
    [Header("스프레드 시트의 시트 이름")][SerializeField] public string associatedWorksheet = "stage";
    [Header("읽기 시작할 행 번호")][SerializeField] public int START_ROW_LENGTH = 2;
    [Header("읽을 마지막 행 번호")][SerializeField] public int END_ROW_LENGTH = 5;

}
