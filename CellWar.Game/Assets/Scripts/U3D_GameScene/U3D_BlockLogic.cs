using UnityEngine;
using CellWar.Model.Map;
using CellWar.GameData;
using CellWar.Model.Substance;
using CellWar.Controller;

namespace CellWar.View {
    public class U3D_BlockLogic : MonoBehaviour {

        public Block HexBlockModel;

        [SerializeField]
        GameObject m_BlockMouseDetectObject;

        public U3D_BlockMouseDetect BlockMouseDetectLogic { get; set; }

        /// <summary>
        /// 初始化这个Block
        /// </summary>
        private void Start() {
            BlockMouseDetectLogic = m_BlockMouseDetectObject.GetComponent<U3D_BlockMouseDetect>();
            BlockMouseDetectLogic.HexBlockModel = HexBlockModel;
        }

        private void Update() {
            BlockMouseDetectLogic.BlockColorUpdate();
        }
    }
}
