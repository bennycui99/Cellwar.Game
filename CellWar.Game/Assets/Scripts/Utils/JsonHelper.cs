using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

/// <summary>
/// Utils
/// </summary>
namespace CellWar.Utils
{

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
            return JsonUtility.FromJson<T>( getFullPath( path ) );
        }
        /// <summary>
        /// convert json file to object
        /// </summary>
        /// <typeparam name="T">object type</typeparam>
        /// <param name="path">like "json/foo"</param>
        /// <returns>object</returns>
        public static T Json2Object_NT<T>( string path ) {
            return JsonConvert.DeserializeObject<T>( getFullPath( path ) );
        }

        public static Dictionary<string, string> Json2DictionaryFromPath( string path ) {
            return Json2Dictionary( getFullPath( path ) );
        }

        public static Dictionary<string, string> Json2Dictionary( string context ) {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>( context );
        }

        public static string Object2Json<T>( T obj ) {
            return JsonConvert.SerializeObject( obj );
        }

        /// <summary>
        /// load file and get its text
        /// </summary>
        /// <param name="jsonFilePath"></param>
        /// <returns></returns>
        private static string getFullPath( string jsonFilePath ) {
            string fullPath = Path.Combine( Application.dataPath, jsonFilePath );
            if( !File.Exists( fullPath ) ) {
                throw new FileNotFoundException( "File: [" + fullPath + "] Not Found." );
            }

            StreamReader sr = new StreamReader( fullPath );
            if( sr == null ) {
                throw new IOException( "File: [" + fullPath + "] Failed to Open." );
            }
            return sr.ReadToEnd();
        }
    }

}