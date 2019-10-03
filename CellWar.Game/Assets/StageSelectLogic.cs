using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

namespace Cellwar.View
{
    public class StageSelectLogic : MonoBehaviour
    {
        [SerializeField]
        Dropdown m_Dropdown;

        private string[] m_Mapfiles; 
        void Awake()
        {
            m_Dropdown.options.Clear();
        }
        void Start()
        {
            m_Mapfiles = getMapFiles();
            foreach (string f in m_Mapfiles)
            {
                m_Dropdown.options.Add(new Dropdown.OptionData() {text = Path.GetFileNameWithoutExtension(f) });
                //It's a file name with out extension, so when try to load it, be sure to add .json on the path string!
            }
        }
        string[] getMapFiles()
        {
            string path = Application.dataPath + "/Resources/GameData";
            string[] filePaths = Directory.GetFiles(@path, "map*.json");
            foreach (string file in filePaths)
            {
                Debug.Log(file);
            }
            return filePaths;
        }
    }
}

