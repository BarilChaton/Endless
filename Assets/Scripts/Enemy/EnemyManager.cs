using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyCore> enemiesInTrigger = new List<EnemyCore>();

    public void AddEnemy(EnemyCore enemy)
    {
        enemiesInTrigger.Add(enemy);
    }

    public void RemoveEnemy(EnemyCore enemy)
    {
        enemiesInTrigger.Remove(enemy);
    }
}
