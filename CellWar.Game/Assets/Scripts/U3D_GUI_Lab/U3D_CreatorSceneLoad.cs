using System.Linq;
using System.Collections;
using System.Collections.Generic;
using CellWar.Controller;
using CellWar.GameData;
using CellWar.Utils;
using CellWar.View;
using UnityEngine;
using UnityEngine.UI;
using static CellWar.Model.Substance.Strain;
using CellWar.Utils.Object;
using System.IO;
using CellWar.Model.Substance;

namespace CellWar.View {
    public class U3D_CreatorSceneLoad : MonoBehaviour {

        static GameObject UITextMaxLength;
        static GameObject UIStrainElement;

        public static void RefreshStrainList() {
            UIHelper.RefreshUIList( "UI_StrainList", UIStrainElement, Save.Strains, ( GameObject g, Strain obj ) => {
                g.GetComponent<U3D_StrainElement>().Strain = obj;
                g.name = obj.Name;
            } );
        }

        void Awake() {
            #region INIT_UIs
            UIHelper.InitUIList( "UI_RaceList", "UI_Ele_Race", Local.AllRaces,
                ( GameObject g, Race obj ) => {
                    g.GetComponent<U3D_RaceElement>().Race = obj;
                    g.name = obj.Name;
                } );

            UIHelper.InitUIList( "UI_CodList", "UI_Ele_Cod", Local.AllCodingGenes,
                ( GameObject g, CodingGene obj ) => {
                    g.GetComponent<U3D_CodElement>().CodingGene = ObjectHelper.Clone( obj );
                    g.name = obj.Name;
                } );

            UIHelper.InitUIList( "UI_RegList", "UI_Ele_Reg", Local.AllRegulartoryGenes,
                ( GameObject g, RegulatoryGene obj ) => {
                    g.GetComponent<U3D_RegElement>().RegulatoryGene = ObjectHelper.Clone( obj, obj.GetType() );
                    g.name = obj.Name;
                } );


            UIHelper.InitUIList( "UI_StrainList", "UI_Ele_Strain", Save.Strains,
                ( GameObject g, Strain obj ) => {
                    g.GetComponent<U3D_StrainElement>().Strain = obj;
                    g.name = obj.Name;
                }, ref UIStrainElement );
            #endregion

            // 没有Strain实例表示创建新的Strain
            if( LabCurrent.Strain == null ) {
                LabCurrent.Strain = new CellWar.Model.Substance.Strain();
            }


            UITextMaxLength = GameObject.Find( "UI_MaxLength" );
            gameObject.SetActive( false );
        }

        public static void FreshLength() {
            StrainController contoller = new StrainController();
            if( LabCurrent.Strain == null ) {
                return;
            }

            int maxLength = LabCurrent.Strain.BasicRace.MaxLength;
            int currentLength = contoller.GetTotalLength( LabCurrent.Strain?.PlayerSelectedGenes );

            string alert = maxLength < currentLength ? "Overflowed!!!!!" : "";

            LabCurrent.IsLengthOverflowed = alert == "" ? false : true;

            UITextMaxLength.GetComponent<Text>().text =
                "Max Length: " + maxLength.ToString() + "/" + currentLength.ToString() + "  " + alert;
        }

        public static void ChangeMaxLengthText( string text ) {
            UITextMaxLength.GetComponent<Text>().text = text;
        }

        /// <summary>
        /// 完成基因编辑重要函数
        /// </summary>
        public void FinishCreating() {
            var strainName = UIHelper.GetInputText( "UI_Strain_Name" );
            if( strainName == "" ) {
                U3D_AlertPanel.EmitAlert( "Please Enter Strain Name" );
                return;
            }
            if( LabCurrent.IsLengthOverflowed ) {
                U3D_AlertPanel.EmitAlert( "The Length is Overflowed. Please Trim the Gene you carry." );
                return;
            }

            if( Save.Strains.Exists( m => m.Name == strainName ) ) { // 修改已存在的细菌
                U3D_AlertPanel.EmitAlert( string.Format(
                    "Strain[{0}]'s Gene Data will be Covered by the modified one.\n Are you sure to modify it? The change can not be recovered.",
                    strainName ),
                    () => { // ok 
                        // 储存基因
                        var strain = Save.Strains.Find( m => m.Name == LabCurrent.Strain.Name );
                        Save.Strains.Remove( strain );
                        Save.Strains.Add( LabCurrent.Strain );
                        Save.SaveAllStrains();
                        RefreshStrainList();
                    },
                    () => { } );
            } else { // 添加新创建的基因
                LabCurrent.Strain.Name = strainName;
                Save.Strains.Add( LabCurrent.Strain );
                Save.SaveAllStrains();
                RefreshStrainList();
            }
        }

        public void ClearSelection() {
            U3D_AlertPanel.EmitAlert(
                "Clear All Selected Genes?",
                () => { // ok 
                    LabCurrent.Strain = new CellWar.Model.Substance.Strain {
                        BasicRace = Local.AllRaces[0]
                    };

                    LabCurrent.RegulatoryGene = null;
                    // 勾选所有的reg
                    UIHelper.SwitchOffAllToggle( "UI_RegList" );
                    UIHelper.SwitchOffAllToggle( "UI_StrainList" );

                    // 刷新长度提示
                    U3D_CreatorSceneLoad.FreshLength();
                    // 替换input名字
                    UIHelper.SetInputText( "UI_Strain_Name", "" );
                },
                () => { } );

        }
        public void ClearSelectionWithNoAlert() {
            LabCurrent.Strain = new CellWar.Model.Substance.Strain {
                BasicRace = Local.AllRaces[0]
            };

            LabCurrent.RegulatoryGene = null;
            // 勾选所有的reg
            UIHelper.SwitchOffAllToggle( "UI_RegList" );
            UIHelper.SwitchOffAllToggle( "UI_StrainList" );
            // 刷新长度提示
            U3D_CreatorSceneLoad.FreshLength();
            // 替换input名字
            UIHelper.SetInputText( "UI_Strain_Name", "" );
        }
    }
}