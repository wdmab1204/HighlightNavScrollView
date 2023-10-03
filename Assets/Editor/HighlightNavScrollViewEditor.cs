using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using HNSV;
using UnityEngine.UIElements;
using UnityEditor.AnimatedValues;

[CustomEditor(typeof(HighlightNavScrollView))]
public class HighlightNavScrollViewEditor : ScrollRectEditor
{
    SerializedProperty elementalCount;
    SerializedProperty buttonGroup;
    SerializedProperty normalColor;
    SerializedProperty highlightColor;
    bool canShowBaseGUI;

    AnimBool showBaseGUI;

    protected override void OnEnable()
    {
        elementalCount = serializedObject.FindProperty("elementalCount");
        buttonGroup = serializedObject.FindProperty("buttonGroup");
        normalColor = serializedObject.FindProperty("normalColor");
        highlightColor = serializedObject.FindProperty("highlightColor");

        showBaseGUI = new AnimBool(Repaint);

        SetAnimBools(true);

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        showBaseGUI.valueChanged.RemoveListener(Repaint);
        base.OnDisable();
    }

    void SetAnimBools(bool instant)
    {
        SetAnimBool(showBaseGUI, canShowBaseGUI == true, instant);
    }

    void SetAnimBool(AnimBool a, bool value, bool instant)
    {
        if (instant)
            a.value = value;
        else
            a.target = value;
    }

    public override void OnInspectorGUI()
    {
        SetAnimBools(false);

        EditorGUILayout.PropertyField(elementalCount);
        EditorGUILayout.PropertyField(buttonGroup);
        EditorGUILayout.PropertyField(normalColor);
        EditorGUILayout.PropertyField(highlightColor);

        canShowBaseGUI = EditorGUILayout.Toggle("Show Base GUI", canShowBaseGUI);
        //if(canShowBaseGUI) base.OnInspectorGUI();

        if (EditorGUILayout.BeginFadeGroup(showBaseGUI.faded))
        {
            base.OnInspectorGUI();
        }
        EditorGUILayout.EndFadeGroup();

        serializedObject.ApplyModifiedProperties();
    }
}