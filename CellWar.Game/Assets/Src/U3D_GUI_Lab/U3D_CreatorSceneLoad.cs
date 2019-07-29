using System;
using System.Collections;
using System.Collections.Generic;
using CellWar.Contoller;
using CellWar.GameData;
using CellWar.Utils;
using CellWar.View;
using UnityEngine;
using UnityEngine.UI;
using static CellWar.Model.Substance.Strain;
using CellWar.Utils.Object;
using System.IO;

namespace CellWar.View {
    public class U3D_CreatorSceneLoad : MonoBehaviour {

        static GameObject UITextMaxLength;

        // Start is called before the first frame update
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
            #endregion

            // 没有Strain实例表示创建新的Strain
            if( LabCurrent.Strain == null ) {
                LabCurrent.Strain = new CellWar.Model.Substance.Strain();
            }

            UITextMaxLength = GameObject.Find( "UI_MaxLength" );
            gameObject.SetActive(false);
        }

        public static void FreshLength() {
            StrainContoller contoller = new StrainContoller();
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

        public static void EmitAlert( string message ) {
            GameObject.Find( "Canvas" ).transform.Find( "UI_AlertPanel" ).gameObject.SetActive( true );
            UIHelper.ChangeText( GameObject.Find( "UI_AlertMsg" ), message );
        }

        /// <summary>
        /// 完成基因编辑重要函数
        /// </summary>
        public void FinishCreating() {
            var strainName = UIHelper.GetInputText( "UI_Strain_Name" );
            if( strainName == "" ) {
                U3D_CreatorSceneLoad.EmitAlert( "Please Enter Strain Name" );
                return;
            }
            if( LabCurrent.IsLengthOverflowed ) {
                U3D_CreatorSceneLoad.EmitAlert( "The Length is Overflowed. Please Trim the Gene you carry." );
                return;
            }
            // 储存基因
            LabCurrent.Strain.Name = strainName;
            Save.Strains.Add( LabCurrent.Strain );
        }
    }
}