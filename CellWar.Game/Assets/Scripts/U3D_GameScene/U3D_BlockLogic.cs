using UnityEngine;
using CellWar.Model.Map;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using CellWar.GameData;
using CellWar.Model.Substance;

namespace CellWar.View {
    public class U3D_BlockLogic : MonoBehaviour {
        const float MAX_UPDATE_COUNT = 1.0f;
        float m_UpdateCount = MAX_UPDATE_COUNT;

        public Block HexBlockModel;

        [SerializeField]
        GameObject m_BlockMouseDetectObject;

        U3D_BlockMouseDetect m_BlockMouseDetectLogic;

        /// <summary>
        /// 初始化这个Block
        /// </summary>
        private void Start()
        {
            HexBlockModel.ParentUnityObjectName = gameObject.name;

            m_BlockMouseDetectLogic = m_BlockMouseDetectObject.GetComponent<U3D_BlockMouseDetect>();
            m_BlockMouseDetectLogic.ParentBlockLogic = this;
            m_BlockMouseDetectLogic.HexBlockModel = HexBlockModel;
        }

        private void Update()
        {
            // 每隔一秒更新一次
            if (m_UpdateCount > 0)
            {
                m_UpdateCount -= Time.deltaTime;
                return;
            }
            else
            {
                m_UpdateCount = MAX_UPDATE_COUNT;
            }

            // 遍历所有种类细菌
            for(var i = 0; i < HexBlockModel.Strains.Count; ++i)
            {
                Strain currentStrain = HexBlockModel.Strains[i];
                // 遍历该菌的所有基因
                foreach (var reg in HexBlockModel.Strains[i].PlayerSelectedGenes)
                {
                    // 检查基因条件
                    if (MainGameCurrent.RegCtor.IsTriggered(HexBlockModel.PublicChemicals, reg))
                    {
                        for (int j = 0; j < reg.DominatedGenes.Count; j++)
                        {
                            var cod = reg.DominatedGenes[j];
                            // 细菌活动
                            MainGameCurrent.CodCtor.Effect(ref currentStrain, ref HexBlockModel, ref cod);
                        }
                    }
                }
                //更新该菌种
                HexBlockModel.Strains[i] = currentStrain;
            }

            m_BlockMouseDetectLogic.BlockColorUpdate();
        }

    }
}
