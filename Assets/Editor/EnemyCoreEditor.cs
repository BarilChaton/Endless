using Endless.TypeOfEnemies;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyCore))]
public class EnemyCoreEditor : Editor
{
    private void OnSceneGUI()
    {
        EnemyCore fov = (EnemyCore)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.meleeRange);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        if (fov.canSeeTarget)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    // The various categories the editor will display the variables in
    public EnemyTypes.Types Types;

    public override void OnInspectorGUI()
    {
        EnemyCore enemyCore = (EnemyCore)target;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyType"));
        Types = enemyCore.enemyType;
        EditorGUILayout.Space();

        // What info to display based on unit
        switch (Types)
        {
            case EnemyTypes.Types.Base:
                DisplayBasicInfo();
                break;

            case EnemyTypes.Types.BasicMelee:
                DisplayBasicInfo();
                DisplayMeleeInfo();
                break;

            case EnemyTypes.Types.BasicRanged:
                DisplayBasicInfo();
                DisplayRangedInfo();
                break;

            case EnemyTypes.Types.BasicMeleeAndRanged:
                DisplayBasicInfo();
                DisplayRangedInfo();
                DisplayMeleeInfo();
                break;
        }
        EditorGUILayout.Space(30);
        enemyCore.showVisionInfo = EditorGUILayout.Toggle("Show Vision Information", enemyCore.showVisionInfo);
        if (enemyCore.showVisionInfo)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("angle"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("radius"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("player"));
        }

        EditorGUILayout.Space(10);
        enemyCore.showDebugInfo = EditorGUILayout.Toggle("Show Debug Information", enemyCore.showDebugInfo);
        if (enemyCore.showDebugInfo)
        {
            EditorGUILayout.LabelField("Attack Info", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("enemyHealth"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeAttackCd"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rangedAttackCd"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRangeTemp"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("shotReady"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("canSeeTarget"));
        }

        EditorGUILayout.Space(10);
        enemyCore.showSoundInfo = EditorGUILayout.Toggle("Show Sound Information", enemyCore.showSoundInfo);
        if (enemyCore.showSoundInfo)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("audioSource"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("aggroSound"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeAttackSound"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rangedAttackSound"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("dieSound"));
        }

        serializedObject.ApplyModifiedProperties();
    }

    // As the name says
    void DisplayBasicInfo()
    {
        EditorGUILayout.LabelField("Absolute Basics", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxHealth"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("armour"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Aggression Stuff", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("aggressionDistance"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hurtImpact"));
        EditorGUILayout.HelpBox("This field can remain empty btw", MessageType.None);
    }

    // As the name says
    void DisplayRangedInfo()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Ranged Information", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("attackRange"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rangedDamage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rangedAttackSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rangedAimTime"));
    }

    // As the name says
    void DisplayMeleeInfo()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Ranged Information", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeDamage"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeAttackSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("meleeRange"));
    }
}
