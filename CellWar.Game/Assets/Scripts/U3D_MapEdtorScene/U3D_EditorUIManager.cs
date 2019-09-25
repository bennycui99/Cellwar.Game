using UnityEngine;
using UnityEngine.UI;
using CellWar.GameData;
using CellWar.Model.Map;
using CellWar.Utils;
using System.IO;
using CellWar.Model.Substance;

namespace CellWar.View
{
    public class U3D_EditorUIManager : MonoBehaviour
    {
        public Map StageMap;

        [SerializeField]
        Text m_ExportText;


        // Start is called before the first frame update
        void Awake()
        {
            StageMap = MainGameCurrent.StageMap;

            UIHelper.InitUIList<Chemical>("UI_ChemicalList", "UI_Chemical", Local.AllChemicals,
                (GameObject g, Chemical obj) => {
                    g.GetComponent<U3D_EditorChemicalPackageLogic>().UIChemical = obj;
                    g.name = obj.Name;
                });
        }

        public void OnExportClick()
        {
            Map map = new Map();

            map.Blocks.AddRange(StageMap.Blocks.FindAll(block => block.IsActive));

            File.WriteAllText(Save.GetGameSavePath("map_generate.json"), JsonHelper.Object2Json(map));
        }

    }

}
