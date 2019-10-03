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
        [SerializeField]
        Text m_IntroductionText;
        [SerializeField]
        RawImage m_IntroductionImage;
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
                m_Dropdown.options.Add(new Dropdown.OptionData() { text = Path.GetFileNameWithoutExtension(f) });
                //It's a file name with out extension, so when try to load it, be sure to add .json on the path string!

            }
            SetIntroductionImage(Application.dataPath + "/Resources/Textures/Artworks/Example.png");
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
        public void SetIntroductionText(string intro)
        {
            m_IntroductionText.text = intro;
        }
        public void SetIntroductionImage(string imgFilePath)
        {
            var tex = new Texture2D(1, 1);
            var bytes = File.ReadAllBytes(imgFilePath);
            //Warning: this will override the original tex size, the image we need will be constant size!
            tex.LoadImage(bytes);
            m_IntroductionImage.texture = tex;
        }
    }
}

