using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(Stat))]
public class StatOnInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject);
        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(PlayerStat))]
public class PlayerStatOnInspector : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawPropertiesExcluding(serializedObject);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif