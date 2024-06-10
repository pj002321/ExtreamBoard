using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using System;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public struct StageData
{
    public int id;
    public string name;
    public int x;
    public int y;
    public int _curstage;
    public int curstage
    {
        get { return _curstage; }
        set { _curstage = value; }
    }

    public StageData(string id, string name, string x, string y, string curstage)
    {
        int.TryParse(id, out this.id);
        this.name = name;
        int.TryParse(x, out this.x);
        int.TryParse(y, out this.y);
        int.TryParse(curstage, out this._curstage);
        Debug.Log(curstage + "CS");
    }
}

[CreateAssetMenu(fileName = "Reader", menuName = "Scriptable Object/DataReader", order = int.MaxValue)]
public class DataBase : DataRenderBase
{
    [Header("Object read from spreadsheet and serialized")][SerializeField] public List<StageData> DataList = new List<StageData>();

    internal void UpdateStats(List<GSTU_Cell> list, int itemID)
    {
        string id = null;
        string name = null;
        string x = null;
        string y = null;
        string curstage = null;

        for (int i = 0; i < list.Count; i++)
        {
            switch (list[i].columnId)
            {
                case "stageID":
                    {
                        id = list[i].value;
                        break;
                    }
                case "stageName":
                    {
                        name = list[i].value;
                        break;
                    }
                case "x":
                    {
                        x = list[i].value;
                        break;
                    }
                case "y":
                    {
                        y = list[i].value;
                        break;
                    }
                case "CurrentScene":
                    {
                        curstage = list[i].value;
                        break;
                    }
            }
        }

        DataList.Add(new StageData(id, name, x, y, curstage));
    }

    public void ReturnCurrentScene(int curstage)
    {
        if (DataList.Count > 0)
        {
            StageData stageData = DataList[0];
            stageData.curstage = curstage;
            DataList[0] = stageData;
        }
    }

    public int GetCurstage()
    {
        return DataList[0].curstage;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DataBase))]
public class StageDataReaderEditor : Editor
{
    DataBase data;

    void OnEnable()
    {
        data = (DataBase)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("\n\nRead Spreadsheet");

        if (GUILayout.Button("Data Load (Call API)"))
        {
            UpdateStats(UpdateMethodOne);
            data.DataList.Clear();
        }
    }

    void UpdateStats(UnityAction<GstuSpreadSheet> callback, bool mergedCells = false)
    {
        SpreadsheetManager.Read(new GSTU_Search(data.associatedSheet, data.associatedWorksheet), callback, mergedCells);
    }

    void UpdateMethodOne(GstuSpreadSheet ss)
    {
        for (int i = data.START_ROW_LENGTH; i <= data.END_ROW_LENGTH; ++i)
        {
            data.UpdateStats(ss.rows[i], i);
        }

        EditorUtility.SetDirty(target);
    }
}
#endif
