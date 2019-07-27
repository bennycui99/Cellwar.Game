using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace HexAutotile
{
    public static class XMLExtensions
    {
        public static int TryElementValueInt(this XElement e, string name)
        {
            XElement e2 = e.Element(name);
            if (e2 != null)
            {
                int res = 0;
                if (int.TryParse(e2.Value, out res))
                {
                    Console.WriteLine("Error, cant parse " + e2.Value);
                }
                return res;
            }
            return 0;
        }

        public static string TryElementValue(this XElement e, string name)
        {
            XElement e2 = e.Element(name);
            if (e2 != null)
            {
                return e2.Value;
            }
            return "";
        }

        public static string TryAttributeValue(this XElement e, string name)
        {
            XAttribute xa = e.Attribute(name);
            if (xa != null)
            {
                return xa.Value;
            }
            return "";
        }

        public static int TryAttributeValueInt(this XElement e, string name)
        {
            XAttribute xa = e.Attribute(name);
            if (xa != null)
            {
                int res = 0;
                if (int.TryParse(xa.Value, out res))
                {
                    Console.WriteLine("Error, cant parse " + xa.Value);
                }
                return res;
            }
            return 0;
        }

        public static XElement CreateChildBody(this XElement me, string name, string value)

        {
            if (value == null)
            {
                throw new Exception();
            }

            XElement nx = new XElement(name);
            me.Add(nx);
            nx.Value = value;
            return nx;
        }

        public static XElement CreateChildBodyIntBiggerZero(this XElement me, string name, int val)
        {
            if (val <= 0)
            {
                return null;
            }

            XElement nx = new XElement(name);
            me.Add(nx);
            nx.Value = val + "";
            return nx;
        }

        public static XAttribute CreateAttributeValue(this XElement me, string name, string value)

        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            if (value == null)
            {
                throw new Exception();
            }

            if (value == "")
            {
                return null;
            }

            XAttribute nx = new XAttribute(name, value);
            me.Add(nx);
            return nx;
        }

        public static XElement CreateChild(this XElement me, string name)
        {
            XElement nx = new XElement(name);
            me.Add(nx);
            return nx;
        }
    }

}
