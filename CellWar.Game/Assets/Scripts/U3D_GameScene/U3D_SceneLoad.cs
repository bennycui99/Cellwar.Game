using System.Collections;
using System.Collections.Generic;
using CellWar.GameData;
using CellWar.Test.Mock;
using CellWar.Model.Substance;
using CellWar.Utils;
using CellWar.View;
using UnityEngine;
using UnityEngine.UI;
using CellWar.Utils.Object;
using CellWar.Model.Map;

namespace CellWar.View {
    /// <summary>
    /// 主游戏场景加载
    /// </summary>
    public class U3D_SceneLoad : MonoBehaviour {
        private void Awake() {
            MainGameCurrent.LoadMap();

            MainGameCurrent.StrainList = ObjectHelper.CloneList2( Save.Strains );

            /// 将所有玩家制作的strain的privatechemicals设置成地图给出的默认值
            /// 每个strain一份
            MainGameCurrent.StrainList.ForEach(strain => strain.PrivateChemicals = ObjectHelper.CloneList2(MainGameCurrent.StageMap.PlayerOwnedChemicals));

            UIHelper.InitUIList( "UI_StrainList", "UI_Strain", MainGameCurrent.StrainList,
                ( GameObject g, Strain obj ) => {
                    g.GetComponent<U3D_StrainPackageLogic>().Strain = obj;
                    g.name = obj.Name;
                } );
            // Map GameObject见 GenerateBlockContainer
        }
    }
}