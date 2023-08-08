using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    Level level;
    List<bool> expandRule = new();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        
        level = target as Level;
        OnRulesGUI(level);

        serializedObject.ApplyModifiedProperties();
    }

    void OnRulesGUI(Level level)
    {
        EditorGUILayout.Space();
        GUILayout.Label("Rules");

        GUILayout.BeginVertical();
        for (int i = 0; i < level.rules.Count; ++i)
        {
            while (i >= expandRule.Count) expandRule.Add(false);

            expandRule[i] = EditorGUILayout.Foldout(expandRule[i], "Rule " + i);

            if (!expandRule[i]) continue;

            EditorGUI.indentLevel++;

            if (level.rules[i] == null)
            {
                GUILayout.Label("Null");
                continue;
            }
            
            level.rules[i].startTime     = EditorGUILayout.FloatField(  "开始时间", level.rules[i].startTime    );
            level.rules[i].enemyType     = (GameObject)EditorGUILayout.ObjectField( "怪物类型", level.rules[i].enemyType, typeof(GameObject), false);
            level.rules[i].maxHp         = EditorGUILayout.FloatField(  "最大血量", level.rules[i].maxHp        );
            level.rules[i].bulletType    = (GameObject)EditorGUILayout.ObjectField( "子弹类型", level.rules[i].bulletType, typeof(GameObject), false);
            level.rules[i].spawnBatch    = EditorGUILayout.IntField(    "生成批次", level.rules[i].spawnBatch   );
            level.rules[i].enemyPerBatch = EditorGUILayout.IntField(    "每批数量", level.rules[i].enemyPerBatch);
            level.rules[i].timeSpace     = EditorGUILayout.FloatField(  "批次间隔", level.rules[i].timeSpace    );
            if(level.rules[i].spawnBatch < 0)
                level.rules[i].spawnTime     = EditorGUILayout.FloatField(  "持续时间", level.rules[i].spawnTime);

            EditorGUI.indentLevel++;
            EditorGUILayout.Foldout(expandRule[i], "道具设置");
            level.rules[i].itemRule.itemType = (Item)EditorGUILayout.ObjectField( "道具类型", level.rules[i].itemRule.itemType, typeof(Item), false);
            level.rules[i].itemRule.dropRate = EditorGUILayout.FloatField(  "掉落概率", level.rules[i].itemRule.dropRate);

            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }
        GUILayout.EndVertical();

        //if(GUILayout.Button("Add Rule"))
        //{
        //    level.rules.Add(new SpawnRule());
        //    expandRule.Add(true);
        //}
        //if(GUILayout.Button("Delete Rule"))
        //{
        //    if(level.rules.Count > 0)
        //    {
        //        level.rules.RemoveAt(level.rules.Count - 1);
        //        expandRule.RemoveAt(expandRule.Count - 1);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("List no element");
        //    }
        //}
    }
}
