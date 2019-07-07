using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// Utils
/// </summary>
namespace CellWar.Utils {

    /// <summary>
    /// load json from file to objects.
    /// </summary>
    public static class JsonHelper {
        /// <summary>
        /// convert json file to object
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="path">like "json/foo"</param>
        /// <returns>object</returns>
        public static T Json2Object_JU<T>( string path ) {
            return JsonUtility.FromJson<T>( getStringFromFile( path ) );
        }
        /// <summary>
        /// convert json file to object
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="path">like "json/foo"</param>
        /// <returns>object</returns>
        public static T Json2Object_NT<T>( string path ) {
            return JsonConvert.DeserializeObject<T>( getStringFromFile( path ) );
        }

        public static Dictionary<string, string> Json2DictionaryFromPath( string path ) {
            return Json2Dictionary( getStringFromFile( path ) );
        }

        public static Dictionary<string, string> Json2Dictionary( string context ) {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>( context );
        }

        /// <summary>
        /// load file and get its text
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string getStringFromFile( string path ) { 
            return ( Resources.Load( path ) as TextAsset ).text;
        }
    }

}