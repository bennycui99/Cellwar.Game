using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CellWar.Utils.Object {
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
    }
}
