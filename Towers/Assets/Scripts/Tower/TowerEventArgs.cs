using System;

namespace Tower
{
    public class TowerCounterEventArgs : EventArgs
    {
        public int MaxTowerCounter { private get => maxTowerCounter; set => maxTowerCounter = value; }
        public int TowerCounter
        {
            get => towerCounter;
            set
            {
                if (towerCounter == value) return;
                if (towerCounter < maxTowerCounter)
                {
                    towerCounter = value;
                }
                else
                {
                    if (!isEnd)
                    {
                        SceneManager.Instance.EndGame();
                        isEnd = true;
                    }
                }
                SceneManager.Instance.UpdateTowerCounterText();
            }
        }

        private int towerCounter;
        private int maxTowerCounter;
        private bool isEnd = false;
    }
}