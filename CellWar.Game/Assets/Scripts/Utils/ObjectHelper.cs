using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace CellWar.Utils.Object
{
    public static class ObjectHelper {
        public static T Clone<T>( T obj ) {
            T ret = default( T );
            if( obj != null ) {
                XmlSerializer cloner = new XmlSerializer( typeof( T ) );
                MemoryStream stream = new MemoryStream();
                cloner.Serialize( stream, obj );
                stream.Seek( 0, SeekOrigin.Begin );
                ret = ( T )cloner.Deserialize( stream );
            }
            return ret;
        }
        public static T Clone<T>( T obj, Type type ) {
            T ret = default( T );
            if( obj != null ) {
                XmlSerializer cloner = new XmlSerializer( type );
                MemoryStream stream = new MemoryStream();
                cloner.Serialize( stream, obj );
                stream.Seek( 0, SeekOrigin.Begin );
                ret = ( T )cloner.Deserialize( stream );
            }
            return ret;
        }
        /// <summary>
        /// Clones the specified list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List">The list.</param>
        /// <returns>List{``0}.</returns>
        public static List<T> CloneList<T>( object List ) {
            using( Stream objectStream = new MemoryStream() ) {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize( objectStream, List );
                objectStream.Seek( 0, SeekOrigin.Begin );
                return formatter.Deserialize( objectStream ) as List<T>;
            }
        }

        public static List<T> CloneList2<T>( List<T> list ) {
            List<T> newList = new List<T>();
            foreach( var ele in list ) {
                newList.Add( ObjectHelper.Clone( ele, ele.GetType() ) );
            }
            return newList;
        }
    }
}
