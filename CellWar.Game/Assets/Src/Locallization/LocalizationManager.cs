using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;

using CellWar.Utils;

namespace CellWar.Misc.Localization {
    public class LocalizationManager : MonoBehaviour {

        #region FUCK_SINGLETON

        public static LocalizationManager m_Instance = null;

        void CreateSingletonInstance() {
            if( m_Instance == null ) {
                m_Instance = this;
            } else if( m_Instance != this ) {
                Destroy( gameObject );
            }
            //DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region MEMBERS
        [SerializeField]
        Language m_CurrentMainLanguage = Language.English;
        
        /// <summary>
        /// TextAsset.text is the context of file.
        /// </summary>
        Dictionary<string, TextAsset> m_LocalizationFiles = new Dictionary<string, TextAsset>();
        Dictionary<string, string> m_LocalizationText = new Dictionary<string, string>();
        #endregion

        void Awake() {
            CreateSingletonInstance();

            m_LocalizationFiles = GetLocalizationFiles();
            SetupLocalization();
        }

        /// <summary>
        /// Search for each Language defined in the Language Enum
        /// </summary>
        /// <returns>
        /// Localization files pairs.
        /// <TextAsset.name, TextAsset> wtf?
        /// </returns>
        public Dictionary<string, TextAsset> GetLocalizationFiles() {
            Dictionary<string, TextAsset> localizationFiles = new Dictionary<string, TextAsset>();
            foreach( Language lan in Language.GetValues( typeof( Language ) ) ) {
                string textAssetPath = "Localization/" + lan.ToString();
                TextAsset textAsset = ( TextAsset )Resources.Load( textAssetPath );
                if( textAsset ) {
                    localizationFiles[textAsset.name] = textAsset;
                    Debug.Log( "Text Asset: " + textAsset.name );
                } else {
                    Debug.LogError( "TextAssetPath not found: " + textAssetPath );
                }
            }
            return localizationFiles;
        }


        public void SetupLocalization() {
            TextAsset textAsset;
            // Search for the specified language file
            if( m_LocalizationFiles.ContainsKey( m_CurrentMainLanguage.ToString() ) ) {
                Debug.Log( "Selected language: " + m_CurrentMainLanguage );
                textAsset = m_LocalizationFiles[m_CurrentMainLanguage.ToString()];
            }
            // If we can't find the specific language default to English
            else {
                Debug.LogError( "Couldn't find localization file for: " + m_CurrentMainLanguage );
                textAsset = m_LocalizationFiles[Language.English.ToString()];
            }

            m_LocalizationText = JsonHelper.Json2Dictionary( textAsset.text );
        }

        public string GetLocalisedString( string key ) {
            string localisedString = "";
            if( !m_LocalizationText.TryGetValue( key, out localisedString ) ) {
                localisedString = "LOC KEY: " + key;
            }

            return localisedString;
        }
    }
}