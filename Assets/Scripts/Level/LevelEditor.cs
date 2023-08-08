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
            
            level.rules[i].startTime     = EditorGUILayout.FloatField(  "��ʼʱ��", level.rules[i].startTime    );
            level.rules[i].enemyType     = (GameObject)EditorGUILayout.ObjectField( "��������", level.rules[i].enemyType, typeof(GameObject), false);
            level.rules[i].maxHp         = EditorGUILayout.FloatField(  "���Ѫ��", level.rules[i].maxHp        );
            level.rules[i].bulletType    = (GameObject)EditorGUILayout.ObjectField( "�ӵ�����", level.rules[i].bulletType, typeof(GameObject), false);
            level.rules[i].spawnBatch    = EditorGUILayout.IntField(    "��������", level.rules[i].spawnBatch   );
            level.rules[i].enemyPerBatch = EditorGUILayout.IntField(    "ÿ������", level.rules[i].enemyPerBatch);
            level.rules[i].timeSpace     = EditorGUILayout.FloatField(  "���μ��", level.rules[i].timeSpace    );
            if(level.rules[i].spawnBatch < 0)
                level.rules[i].spawnTime     = EditorGUILayout.FloatField(  "����ʱ��", level.rules[i].spawnTime);

            EditorGUI.indentLevel++;
            EditorGUILayout.Foldout(expandRule[i], "��������");
            level.rules[i].itemRule.itemType = (Item)EditorGUILayout.ObjectField( "��������", level.rules[i].itemRule.itemType, typeof(Item), false);
            level.rules[i].itemRule.dropRate = EditorGUILayout.FloatField(  "�������", level.rules[i].itemRule.dropRate);

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
