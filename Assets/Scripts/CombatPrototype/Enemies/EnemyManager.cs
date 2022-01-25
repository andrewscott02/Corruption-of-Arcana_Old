using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    //[HideInInspector]
    public List<Enemy> enemies = new List<Enemy>();
    //[HideInInspector]
    public List<Targetter> targetters;

    public CombatManager combatManager;

    public Text nameText;
    public Text attackText;
    public Text dmgText;
    public Image image;
    public Text descriptionText;
    public GameObject[] disable;

    private void Start()
    {
        SetupLists();

        foreach (var item in disable)
        {
            item.SetActive(false);
        }
    }

    public IEnumerator IDelaySetup(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetupLists();
    }

    public void SetupLists()
    {
        enemies.Clear();
        targetters.Clear();

        Enemy[] enemiesArray = GameObject.FindObjectsOfType<Enemy>();

        foreach (var item in enemiesArray)
        {
            Targetter targetter = item.GetComponentInChildren<Targetter>();

            if (!targetter.ally)
            {
                targetters.Add(item.GetComponentInChildren<Targetter>());
            }
        }

        foreach (var item in enemiesArray)
        {
            enemies.Add(item);

            item.GetComponent<EnemyStats>().enemyManager = this;
            
            EnemyDescription description = item.GetComponent<EnemyDescription>();

            description.nameText = nameText;
            description.attackText = attackText;
            description.dmgText = dmgText;
            description.image = image;
            description.descriptionText = descriptionText;
            description.disable = disable;
        }
    }

    public void StartEnemyTurn()
    {
        float interval = 1;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null)
                enemies[i].Invoke("TakeTurn", (((enemies[i].hitCount * 0.6f) * i) + 0.4f) * interval);
        }

        Invoke("EndEnemyTurn", interval * (0.4f + enemies.Count));
    }

    private void EndEnemyTurn()
    {
        if (combatManager != null)
        {
            combatManager.EndTurn(false);
        }
    }

    public void EnemyKilled()
    {
        StartCoroutine(IDelaySetup(0.5f));

        if (enemies.Count <= 0)
        {
            combatManager.ShowEndScreen(true);
        }
    }

    public void TargetEnemies(bool visible)
    {
        foreach (var item in targetters)
        {
            item.SetVisibility(visible);
        }
    }

    public void EnemyInfo(Enemy controller)
    {
        if (controller != null)
        {
            //show enemy description
            //Debug.Log("Display enemy info: " + controller.displayName);
            controller.DisplayCard(true);
        }
        else
        {
            //remove enemy descriptions
            //Debug.Log("Remove info");
            foreach (var item in disable)
            {
                item.SetActive(false);
            }
        }
    }

    public void RemoveEnemyInfo()
    {
        //remove enemy descriptions
        Debug.Log("Remove info");
        foreach (var item in disable)
        {
            item.SetActive(false);
        }
    }
}