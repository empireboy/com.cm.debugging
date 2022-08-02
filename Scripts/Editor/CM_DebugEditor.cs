using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CM_DebugEditor : EditorWindow
{
    public static readonly string debugTitle = "CM Debug";
    public static readonly string debugSOName = "CM_Debug";
    public static readonly string unityDebugName = "Unity Debug";

    private static CM_DebugSO _debugSO;

    private static string _newCategory;

    private static Vector2 _scrollPosition;

    private static GUIStyle _titleStyle;

    [MenuItem("CM/Debug")]
    private static void Init()
    {
        CM_DebugEditor window = (CM_DebugEditor)GetWindow(typeof(CM_DebugEditor), false, debugTitle);

        UpdateCategories();

        window.Show();
    }

    private void OnEnable()
    {
        if (!_debugSO)
            _debugSO = Resources.Load<CM_DebugSO>(debugSOName);

        InitStyles();

        UpdateCategories();
    }

    private void OnGUI()
    {
        if (!_debugSO)
            return;

        EditorGUILayout.Space();

        DrawTitle();

        EditorGUILayout.Space();

        DrawCategories();

        DrawToggleButtons();

        EditorGUILayout.Space();

        DrawAddCategoryButton();

        EditorGUILayout.Space();

        DrawLogFormat();

        EditorGUILayout.Space();

        EditorUtility.SetDirty(_debugSO);
    }

    private void DrawTitle()
    {
        EditorGUILayout.LabelField(debugTitle, _titleStyle, GUILayout.MaxHeight(25));
    }

    private void DrawCategories()
    {
        EditorGUILayout.LabelField("Categories", EditorStyles.boldLabel);

        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        EditorGUILayout.BeginVertical("Box");

        for (int i = 0; i < _debugSO.categories.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            CM_DebugSO.Category category = _debugSO.categories[i];

            category.enabled = EditorGUILayout.Toggle(category.name, category.enabled);

            if (category.enabled != CM_Debug.IsCategoryEnabled(category.name))
            {
                CM_Debug.EnableCategory(category.name, category.enabled);

                if (category.name == unityDebugName)
                    Debug.unityLogger.logEnabled = category.enabled;
            }

            GUILayout.FlexibleSpace();

            if (category.name != unityDebugName)
            {
                if (GUILayout.Button("X", GUILayout.Width(25f)))
                {
                    _debugSO.categories.RemoveAt(i);

                    UpdateCategories();
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    private void DrawToggleButtons()
    {
        EditorGUILayout.BeginHorizontal();

        // Enabling and disabling of all categories
        if (GUILayout.Button("Enable All"))
        {
            for (int i = 0; i < _debugSO.categories.Count; i++)
            {
                CM_DebugSO.Category category = _debugSO.categories[i];

                category.enabled = true;

                if (category.enabled != CM_Debug.IsCategoryEnabled(category.name))
                    UpdateCategories();
            }
        }

        if (GUILayout.Button("Disable All"))
        {
            for (int i = 0; i < _debugSO.categories.Count; i++)
            {
                CM_DebugSO.Category category = _debugSO.categories[i];

                category.enabled = false;

                if (category.enabled != CM_Debug.IsCategoryEnabled(category.name))
                    UpdateCategories();
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawAddCategoryButton()
    {
        EditorGUILayout.BeginHorizontal();

        // Add new category
        _newCategory = EditorGUILayout.TextField("New Category", _newCategory);

        if (GUILayout.Button("Add"))
        {
            _debugSO.categories.Add(new CM_DebugSO.Category(_newCategory, true));
            _newCategory = "";

            UpdateCategories();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawLogFormat()
    {
        EditorGUILayout.HelpBox("To format, use {0} for category and {1} for message.", MessageType.Info);

        EditorGUILayout.Space();

        _debugSO.logFormat = EditorGUILayout.TextField("Log Format", _debugSO.logFormat);

        if (!string.Equals(_debugSO.logFormat, CM_Debug.logFormat))
            CM_Debug.logFormat = _debugSO.logFormat;
    }

    private static void UpdateCategories()
    {
        if (!_debugSO)
            return;

        // Add Unity Debug category as first
        if ((_debugSO.categories.Count <= 0) || (_debugSO.categories[0].name != unityDebugName))
        {
            _debugSO.categories.Insert(0, new CM_DebugSO.Category(unityDebugName, true));
        }

        if (_debugSO.categories[0].name == unityDebugName)
            Debug.unityLogger.logEnabled = _debugSO.categories[0].enabled;

        // Add all categories to CM_Debug
        Dictionary<string, bool> categoriesDictionary = new Dictionary<string, bool>();

        foreach (CM_DebugSO.Category category in _debugSO.categories)
            categoriesDictionary.Add(category.name, category.enabled);

        CM_Debug.SetCategories(categoriesDictionary);
    }

    private static void InitStyles()
    {
        _titleStyle = new GUIStyle
        {
            fontSize = 22,
            fontStyle = FontStyle.Bold
        };

        _titleStyle.normal.textColor = Color.white;
    }
}