using UnityEngine;
using CellWar.GameData;

namespace CellWar.Controller
{
    public class GameManager : MonoBehaviour
    {
        public bool IsGameStarted;
        public bool IsGameCompleted;

        const float defaultUpdateCount = 1.0f;

        public float MaxUpdateCount { get; set; } = defaultUpdateCount;

        private float CurrentUpdateCount = defaultUpdateCount;

        bool IsStageCompleted()
        {
            ////////////////////////////////////////////////////////////
            int total = 0;
            foreach( var block in MainGameCurrent.StageMap.Blocks ) {
                var quantity = block.PublicChemicals.Find(m => m.Name == "Cu");
                if ( quantity != null)
                {
                    total += quantity.Count;
                }
            }
            if (total <= 100)
            {
                Debug.Log("Game Over");
                return true;
            }
            return false;
            //////////////////////////////////////////////////////////// 
        }

        #region SINGLETON

        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }

        #endregion

        #region U3D

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

        /// <summary>
        /// 游戏最重要函数
        /// </summary>
        private void Update()
        {
            // 游戏还没开始就不更新
            if (!Instance.IsGameStarted) { return; }

            // 每隔一秒更新一次
            if (CurrentUpdateCount > 0)
            {
                CurrentUpdateCount -= Time.deltaTime;
                return;
            }
            else
            {
                CurrentUpdateCount = MaxUpdateCount;
            }

            // 更新所有格子
            foreach (var block in MainGameCurrent.StageMap.Blocks)
            {
                block.BlockLogic.BlockBacteriaUpdate();
            }
            
            // 判断游戏胜利
            if (IsStageCompleted())
            {
                IsGameCompleted = true;
                IsGameStarted = false;
                //send information here
            }
            
        }
        #endregion
    }

}
