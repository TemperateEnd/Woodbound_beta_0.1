using System.Collections;
using System.Collections.Generic;
using Utility.Vector;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathEnemy))]
public class PathEnemyEditor : Editor
{
    private PathEnemy _enemy;

    private void Awake()
    {
        _enemy = (PathEnemy)target;
    }

    private void OnSceneGUI()
    {
        for (int i = 0; i < _enemy.SmoothPath.Length; i++)
        {
            Vector3 current = _enemy.SmoothPath[i];
            Vector3 next = _enemy.SmoothPath[i + 1 > _enemy.SmoothPath.Length - 1 ? 0 : i + 1];

            Handles.color = Color.red;
            Handles.DrawLine(current, next);
        }

        for (int i = 0; i < _enemy.RoughPath.Length; i++)
        {
            Handles.color = Color.cyan;
            Handles.DrawLine(_enemy.RoughPath[i].Point, _enemy.RoughPath[i + 1 > _enemy.RoughPath.Length - 1 ? 0 : i + 1].Point);

            ShowSplineNode(i);
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        GUILayout.Label($"Baked Smooth Path has {_enemy.SmoothPath.Length} points");
        if (GUILayout.Button("Bake Smooth Path"))
        {
            _enemy.BakeSmooth();
        }
    }

    private void ShowSplineNode(int index)
    {
        EditorGUI.BeginChangeCheck();
        SplineNode node = _enemy.RoughPath[index];

        SerializableVector2 oldPoint = node.Point;
        SerializableVector2 newPoint = (SerializableVector2)Handles.PositionHandle(node.Point, Quaternion.identity);

        GUIStyle style = new GUIStyle
        {
            fontSize = 15,
        };

        style.normal.textColor = Color.white;

        Handles.Label(newPoint, new GUIContent(index.ToString()), style);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_enemy, "Move Point of Spline Node");
            _enemy.RoughPath[index].Point = newPoint;
        }

        ShowControlPoint(index, oldPoint, newPoint);
    }

    private void ShowControlPoint(int index, SerializableVector2 oldPoint, SerializableVector2 newPoint)
    {
        EditorGUI.BeginChangeCheck();
        SplineNode node = _enemy.RoughPath[index];

        SerializableVector2 left = (SerializableVector2)Handles.PositionHandle(node.LeftControl, Quaternion.identity);
        SerializableVector2 right = (SerializableVector2)Handles.PositionHandle(node.RightControl, Quaternion.identity);

        Handles.color = Color.white;
        Handles.Label(left, "LEFT");
        Handles.DrawLine(newPoint, left);
        Handles.Label(right, "RIGHT");
        Handles.DrawLine(newPoint, right);
        
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(_enemy, "Move Control Point of Spline Node");
            SerializableVector2 dir = newPoint - oldPoint;

            _enemy.RoughPath[index].LeftControl = left + dir;
            _enemy.RoughPath[index].RightControl = right + dir;
        }
    }
}
