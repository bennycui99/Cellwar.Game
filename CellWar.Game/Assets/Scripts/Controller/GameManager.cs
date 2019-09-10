using UnityEngine;
using CellWar.GameData;

namespace CellWar.Controller
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }

        public bool IsGameStarted;
        public bool IsGameCompleted;

        const float MAX_UPDATE_COUNT = 1.0f;
        public float UpdateCount = MAX_UPDATE_COUNT;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            IsGameStarted = false;
            IsGameCompleted = false;
        }

        private void Update()
        {
            // 游戏还没开始就不更新
            if (!Instance.IsGameStarted) { return; }

            // 每隔一秒更新一次
            if (UpdateCount > 0)
            {
                UpdateCount -= Time.deltaTime;
                return;
            }
            else
            {
                UpdateCount = MAX_UPDATE_COUNT;
            }

            if (IsStageCompleted())
            {
                IsGameCompleted = true;
                IsGameStarted = false;
                //send information here
            }
        }

        bool IsStageCompleted()
        {
            foreach( var block in MainGameCurrent.StageMap.Blocks ) {
                if( block.PublicChemicals.Find( m => m.Name == "Cu" )?.Count > 100 ) {
                    return false;
                }
            }
            Debug.Log("Game Over");
            return true;
        }
    }

}
