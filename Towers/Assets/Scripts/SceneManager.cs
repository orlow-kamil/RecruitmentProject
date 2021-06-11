using UnityEngine;
using TMPro;
using System;
using Tower;
using System.Linq;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneManager>();
                if (instance == null)
                {
                    instance = new GameObject().AddComponent<SceneManager>();
                }
            }
            return instance;
        }
    }

    private static SceneManager instance;

    public event EventHandler<TowerCounterEventArgs> OnUpdateTowerScore;
    public event EventHandler<EventArgs> OnEndTowerSpawner;

    public GameObject BulletsParent => this.bulletsParent;
    public GameObject TowersParent => this.towersParent;

    [SerializeField] private int maxNumberTower = 100;
    [SerializeField] private TextMeshProUGUI towerCounterText;
    [SerializeField] private GameObject bulletsParent;
    [SerializeField] private GameObject towersParent;

    private TowerCounterEventArgs towerEventArgs;

    private void Start()
    {
        towerEventArgs = new TowerCounterEventArgs()
        {
            MaxTowerCounter = maxNumberTower,
            TowerCounter = 1,
        };
        this.OnUpdateTowerScore += this.ChangeTowerCounterText;
        this.OnEndTowerSpawner += this.LastSalvoTowers;
        this.UpdateTowerCounterText();
    }

    private void ChangeTowerCounterText(object sender, TowerCounterEventArgs args) => this.towerCounterText.text = $"Towers: {args.TowerCounter}";

    private void LastSalvoTowers(object sender, EventArgs args)
    {
        var towers = FindObjectsOfType<MonoBehaviour>().OfType<ITower>();
        foreach (var tower in towers)
        {
            tower.Restart();
        }
    }

    public void IncreaseTowerCounter() => ++this.towerEventArgs.TowerCounter;
    public void DecreaseTowerCounter() => --this.towerEventArgs.TowerCounter;

    public void UpdateTowerCounterText() => this.OnUpdateTowerScore?.Invoke(this, this.towerEventArgs);
    public void EndGame() => this.OnEndTowerSpawner?.Invoke(this, EventArgs.Empty);
}

