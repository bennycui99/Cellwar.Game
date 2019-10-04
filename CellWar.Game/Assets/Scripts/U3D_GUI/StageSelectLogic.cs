using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;
using CellWar.GameData;
using CellWar.Model.Json;
using CellWar.Utils;

namespace CellWar.View {
    public class StageSelectLogic : MonoBehaviour {
        [SerializeField]
        Dropdown m_Dropdown;
        [SerializeField]
        Text m_IntroductionText;
        [SerializeField]
        RawImage m_IntroductionImage;

        /// <summary>
        /// 将路径与mapmodel一一对应
        /// 只做存档，其实fullpath已没有用处
        /// </summary>
        class MapPreview {
            public string FullPath { get; set; }
            public MapJsonModel JsonModel { get; set; }
        }

        List<MapPreview> mapPreviewModels = new List<MapPreview>();

        public string CurrentMapFilePath;
        void Awake() {
            m_Dropdown.options.Clear();
        }

        void Start() {
            foreach( var path in getMapFilePaths() ) {
                var mapJsonModel = JsonHelper.Json2Object_NT<MapJsonModel>( path, JsonHelper.PathType.FullPath );
                mapPreviewModels.Add( new MapPreview {
                    FullPath = path,
                    JsonModel = mapJsonModel
                } );
                m_Dropdown.options.Add( new Dropdown.OptionData() { text = mapJsonModel.Name } );
            }

            // 可能但不现实
            //if( mapFilePaths.Length == 0 ) {
            //    Debug.LogError( "No map files found!" );
            //}
            DoValueChange();
        }

        /// <summary>
        /// 根据select获取当前选择的map数据
        /// </summary>
        /// <returns></returns>
        MapPreview getCurrentSelectMapPreview() {
            return mapPreviewModels[m_Dropdown.value];
        }

        /// <summary>
        /// 更新显示数据
        /// </summary>
        void DoValueChange() {
            var currentSelected = getCurrentSelectMapPreview();

            SetIntroductionImage( currentSelected.JsonModel.ImageName );
            SetIntroductionText( currentSelected.JsonModel.Description );
            MainGameCurrent.LoadMap( currentSelected.JsonModel );
        }

        /// <summary>
        /// 获取所有的map路径
        /// </summary>
        /// <returns></returns>
        string[] getMapFilePaths() {
            string path = Application.dataPath + "/Resources/Maps";
            string[] filePaths = Directory.GetFiles(@path, "map*.json");
            foreach( string file in filePaths ) {
                Debug.Log( file );
            }
            return filePaths;
        }
        #region METHODS

        public void SetIntroductionText( string intro ) {
            m_IntroductionText.text = intro;
        }
        public void SetIntroductionImage( string imgFileFullName ) {
            string path = Textures.GetArtworkPath( imgFileFullName );
            if( !File.Exists( path ) ) {
                path = Textures.DefaultArtworkPath;
            }
            var tex = new Texture2D(1, 1);
            var bytes = File.ReadAllBytes(path);
            //Warning: this will override the original tex size, the image we need will be constant size!
            tex.LoadImage( bytes );
            m_IntroductionImage.texture = tex;
        }
        #endregion
        public void OnValueChange() {
            DoValueChange();
        }
    }
}

