using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AttackSO))]
public class AttackSOEditor2D : Editor
{
    private void OnEnable()
    {
        SceneView.duringSceneGui += DrawHitboxGizmo;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DrawHitboxGizmo;
    }

    private void DrawHitboxGizmo(SceneView sceneView)
    {
        AttackSO attackSO = (AttackSO)target;

        Handles.color = Color.red; // Set the gizmo color.

        // Draw the hitbox gizmo using Handles.DrawWireCube2D.
        Vector2 center = attackSO.hitboxPosition;
        Vector2 size = attackSO.hitboxSize;
        Handles.DrawWireCube(center, size);

        // Handle user interaction and update hitbox parameters here if needed.
        EditorGUI.BeginChangeCheck();
        Vector2 newCenter = Handles.PositionHandle(center, Quaternion.identity);
        Vector2 newSize = Handles.ScaleHandle(size, center, Quaternion.identity, HandleUtility.GetHandleSize(center));
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(attackSO, "Modify Hitbox");
            attackSO.hitboxPosition = newCenter;
            attackSO.hitboxSize = newSize;
            EditorUtility.SetDirty(attackSO);
        }
    }
}