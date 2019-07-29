using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CellWar.Utils {
    public static class UIHelper {
        public delegate void FeedListElementHandle<T>( GameObject gameObject, T obj );
        /// <summary>
        /// 初始化一个UI列表
        /// </summary>
        /// <typeparam name="T">List的数据类型</typeparam>
        /// <param name="listName">Viewport下的content的名字</param>
        /// <param name="elementName">Viewport下列示元素的名字</param>
        /// <param name="dataList">List T的列表</param>
        /// <param name="func">进行数据元素和ui元素的某些操作</param>
        public static void InitUIList<T>( string listName, string elementName, List<T> dataList, FeedListElementHandle<T> func ) {
            var UIList = GameObject.Find( listName ).gameObject;
            var UIElement = GameObject.Find( elementName ).gameObject;
            foreach( var datum in dataList ) {
                var newUIElement = GameObject.Instantiate( UIElement );
                func( newUIElement, datum );
                newUIElement.transform.parent = UIList.transform;
            }
            UIElement.SetActive( false );
        }
        /// <summary>
        /// 改变UI Text的text值
        /// </summary>
        /// <param name="textObject"></param>
        /// <param name="text"></param>
        public static void ChangeText( GameObject textObject, string text ) {
            textObject.GetComponent<Text>().text = text;
        }

        /// <summary>
        /// 开关toggle
        /// </summary>
        /// <param name="toggleName"></param>
        /// <param name="isOn"></param>
        public static void SwitchToggle( string toggleName, bool isOn ) {
            GameObject.Find( toggleName ).GetComponent<Toggle>().isOn = isOn;
        }

        public static void SwitchOffAllToggle( string parentName ) {
            foreach( Transform toggle in GameObject.Find( parentName ).transform ) {
                toggle.GetComponent<Toggle>().isOn = false;
            }
        }

        public static string GetInputText( string inputName ) {
            return GameObject.Find( inputName ).GetComponent<InputField>().text;
        }
    }
}
