using UnityEngine;
using UnityEngine.UI;
using CellWar.GameData;
using CellWar.Model.Map;
using CellWar.Utils;
using System.IO;
using CellWar.Model.Substance;
using CellWar.Controller;

namespace CellWar.View
{
    public class U3D_EditorUIManager : MonoBehaviour
    {
        [SerializeField]
        Text m_ExportText;


        // Start is called before the first frame update
        void Awake()
        {
            UIHelper.InitUIList<Chemical>("UI_ChemicalList", "UI_Chemical", Local.AllChemicals,
                (GameObject g, Chemical obj) => {
                    g.GetComponent<U3D_EditorChemicalPackageLogic>().UIChemical = obj;
                    g.name = obj.Name;
                });

            MainGameCurrent.EditorNpcStrainList = Save.LoadStrainsWithFilePath("GameData/npc_strains.json");

            Utils.UIHelper.InitUIList("UI_StrainList", "UI_Strain", MainGameCurrent.EditorNpcStrainList,
                (GameObject g, Model.Substance.Strain obj) => {
                    g.GetComponent<U3D_StrainPackageLogic>().Strain = obj;
                    g.name = obj.Name;
                });
        }

        public void OnExportClick()
        {
            new MapController().SaveMapToLocal();
        }

    }

}
