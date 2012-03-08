using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Xml;
using RGiesecke.DllExport;

namespace UdkXmlParser
{
    internal static class UnmanagedExports
    {
        public static XmlDocument _xmlDocument;
        public static Dictionary<String, XmlNode> _xmlNodeDictionary;
        public static string _curNodeName;
        public static string _errorMsg;

        public static String GetAssemblyPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        [DllExport("initXML", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String initXML([MarshalAs(UnmanagedType.LPWStr)] String documentNodeName)
        {
            _xmlNodeDictionary = new Dictionary<string, XmlNode>();
            _xmlDocument = new XmlDocument();
            _xmlNodeDictionary.Add(documentNodeName, _xmlDocument);
            _curNodeName = documentNodeName;
            return _curNodeName;
        }

        [DllExport("getLocalFilePath", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static string getLocalFilePath([MarshalAs(UnmanagedType.LPWStr)] String fileName)
        {
            return Path.Combine(GetAssemblyPath(), fileName);
        }

        [DllExport("loadXML", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        static bool loadXML([MarshalAs(UnmanagedType.LPWStr)] String filePath)
        {
            if ( _xmlDocument != null )
            {
                try
                {
                    _xmlDocument.Load( filePath );
                }
                catch ( Exception e )
                {
                    _errorMsg = e.Message;
                    return false;
                }
            }
            else
            {
                _errorMsg = "Must call initXML before loadXML";
                return false;
            }

            return true;
        }

        [DllExport("loadXMLString", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        static bool loadXMLString([MarshalAs(UnmanagedType.LPWStr)] String xmlString)
        {
            if ( _xmlDocument != null )
            {
                try
                {
                    _xmlDocument.LoadXml(xmlString);
                }
                catch ( Exception e )
                {
                    _errorMsg = e.Message;
                    return false;
                }
            }
            else
            {
                _errorMsg = "Must call initXML before loadXMLString";
                return false;
            }

            return true;
        }

        [DllExport("firstChild", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String firstChild([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName, [MarshalAs(UnmanagedType.LPWStr)] String resultNodeName)
        {
            if ( resultNodeName != "" )
            {
                _curNodeName = resultNodeName;
            }

            if ( targetNodeName == "" )
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    _xmlNodeDictionary[_curNodeName] = _xmlNodeDictionary[targetNodeName].FirstChild;
                }
                else
                {
                    _errorMsg = "Invalid target node " + targetNodeName + " for firstChild";
                }
            }
            else
            {
                _errorMsg = "Must call initXML before firstChild";
            }

            return _curNodeName;
        }

        [DllExport("lastChild", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String lastChild([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName, [MarshalAs(UnmanagedType.LPWStr)] String resultNodeName)
        {
            if ( resultNodeName != "" )
            {
                _curNodeName = resultNodeName;
            }

            if ( targetNodeName == "" )
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    _xmlNodeDictionary[_curNodeName] = _xmlNodeDictionary[targetNodeName].LastChild;
                }
                else
                {
                    _errorMsg = "Invalid target node " + targetNodeName + " for lastChild";
                }
            }
            else
            {
                _errorMsg = "Must call initXML before lastChild";
            }

            return _curNodeName;
        }

        [DllExport("hasChildren", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        static bool hasChildren([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName)
        {
            if ( targetNodeName == "" )
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    return _xmlNodeDictionary[targetNodeName].HasChildNodes;
                }
                else
                {
                    _errorMsg = "Invalid target node " + targetNodeName + " for hasChildren";
                    return false;
                }
            }
            else
            {
                _errorMsg = "Must call intiXML before hasChildren";
                return false;
            }
        }

        [DllExport("childCount", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        static int childCount([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName)
        {
            if ( targetNodeName == "" )
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    return _xmlNodeDictionary[targetNodeName].ChildNodes.Count;
                }
                else
                {
                    _errorMsg = "Invalid target node " + targetNodeName + "for numberOfChildren";
                    return 0;
                }
            }
            else
            {
                _errorMsg = "Must call initXML before childCount";
                return 0;
            }
        }

        [DllExport("getChildAtIdx", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String getChildAtIdx([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName, int nodeIdx, [MarshalAs(UnmanagedType.LPWStr)] String resultNodeName)
        {
            if ( resultNodeName != "" )
            {
                _curNodeName = resultNodeName;
            }

            if ( targetNodeName == "" )
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    if ( _xmlNodeDictionary[targetNodeName].HasChildNodes && nodeIdx < _xmlNodeDictionary[targetNodeName].ChildNodes.Count && nodeIdx >= 0 )
                    {
                        _xmlNodeDictionary[_curNodeName] = _xmlNodeDictionary[targetNodeName].ChildNodes[nodeIdx];
                    }
                    else
                    {
                        _errorMsg = "Invalid node index " + nodeIdx + " for getChildAtIdx using targetNode " + targetNodeName;
                    }
                }
                else
                {
                    _errorMsg = "Invalid target node " + targetNodeName + "for getChildAtIdx";
                }
            }
            else
            {
                _errorMsg = "Must call initXML before getChildAtIdx";
            }

            return _curNodeName;
        }

        [DllExport("parentNode", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String parentNode([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName, [MarshalAs(UnmanagedType.LPWStr)] String resultNodeName)
        {
            if ( resultNodeName != "" )
            {
                _curNodeName = resultNodeName;
            }

            if ( targetNodeName == "" )
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    _xmlNodeDictionary[_curNodeName] = _xmlNodeDictionary[targetNodeName].ParentNode;
                }
                else
                {
                    _errorMsg = "Invalid target node " + targetNodeName + "for parentNode";
                }
            }
            else
            {
                _errorMsg = "Must call initXML before parentNode";
            }

            return _curNodeName;

        }

        [DllExport("nextSibling", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String nextSibling([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName, [MarshalAs(UnmanagedType.LPWStr)] String resultNodeName)
        {
            if ( resultNodeName != "" )
            {
                _curNodeName = resultNodeName;
            }

            if ( targetNodeName == "" )
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    _xmlNodeDictionary[_curNodeName] = _xmlNodeDictionary[targetNodeName].NextSibling;
                }
                else
                {
                    _errorMsg = "Invalid target node " + targetNodeName + "for nextSibling";
                }
            }
            else
            {
                _errorMsg = "Must call initXML before nextSibling";
            }
            return _curNodeName;

        }

        [DllExport("previousSibling", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String previousSibling([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName, [MarshalAs(UnmanagedType.LPWStr)] String resultNodeName)
        {
            if ( resultNodeName != "" )
            {
                _curNodeName = resultNodeName;
            }

            if ( targetNodeName == "" )
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    _xmlNodeDictionary[_curNodeName] = _xmlNodeDictionary[targetNodeName].PreviousSibling;
                }
                else
                {
                    _errorMsg = "Invalud target node " + targetNodeName + " for previousSibling";
                }
            }
            else
            {
                _errorMsg = "Must call initXML before previousSibling";
            }

            return _curNodeName;
        }

        [DllExport("attributeCount", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        static int attributeCount([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName)
        {
            if ( targetNodeName == "" )
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    return _xmlNodeDictionary[targetNodeName].Attributes.Count;
                }
                else
                {
                    _errorMsg = "Invalid target node " + targetNodeName + " for attributeCount";
                    return 0;
                }
            }
            else
            {
                _errorMsg = "Must call initXML before attributeCount";
                return 0;
            }
        }


        [DllExport("getAttributeByIdx", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String getAttributeByIdx([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName, int attributeIdx)
        {
            if ( targetNodeName == "" )
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    if ( attributeIdx >= 0 && attributeIdx < _xmlNodeDictionary[targetNodeName].Attributes.Count )
                    {
                        return _xmlNodeDictionary[targetNodeName].Attributes[attributeIdx].Value;
                    }
                    else
                    {
                        _errorMsg = "Invalid attribute index " + attributeIdx + " for " + targetNodeName;
                        return "";
                    }
                }
                else
                {
                    _errorMsg = "Invalid target node " + targetNodeName + " for getAttributeByIdx";
                    return "";
                }
            }
            else
            {
                _errorMsg = "Must call initXML before getAttributeByIdx";
                return "";
            }

        }

        [DllExport("getAttributeByName", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String getAttributeByName([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName, [MarshalAs(UnmanagedType.LPWStr)] String attributeName)
        {
            if (targetNodeName == "")
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    try
                    {
                        XmlElement targetElement = _xmlNodeDictionary[targetNodeName] as XmlElement;

                        if ( targetElement.HasAttribute(attributeName) )
                        {
                            return _xmlNodeDictionary[targetNodeName].Attributes[attributeName].Value;
                        }
                        else
                        {
                            _errorMsg = "Invalid attribute name " + attributeName + " for " + targetNodeName;
                            return "";
                        }
                    }
                    catch ( InvalidCastException )
                    {
                        _errorMsg = "Node " + targetNodeName + " is not an element";
                        return "";
                    }
                }
                else
                {
                    _errorMsg = "Invalid target node " + targetNodeName + " for getAttributeByIdx";
                    return "";
                }
            }
            else
            {
                _errorMsg = "Must call initXML before getAttributeByName";
                return "";
            }
        }

        [DllExport("innerText", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String innerText([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName)
        {
            if ( targetNodeName == "" )
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    return _xmlNodeDictionary[targetNodeName].InnerText;
                }
                else
                {
                    _errorMsg = "Invalid target node " + targetNodeName + " for innerText";
                    return "";
                }
            }
            else
            {
                _errorMsg = "Must call intiXML before innerText";
                return "";
            }
        }

        [DllExport("innerXML", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String innerXML([MarshalAs(UnmanagedType.LPWStr)] String targetNodeName)
        {
            if ( targetNodeName == "" )
            {
                targetNodeName = _curNodeName;
            }

            if ( _xmlNodeDictionary != null )
            {
                if ( _xmlNodeDictionary.ContainsKey(targetNodeName) )
                {
                    return _xmlNodeDictionary[targetNodeName].InnerXml;
                }
                else
                {
                    _errorMsg = "Invalid target node " + targetNodeName + " for innerXML";
                    return "";
                }
            }
            else
            {
                _errorMsg = "Must call initXML before innerXML";
                return "";
            }
        }

        [DllExport("getError", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPWStr)]
        static String getError()
        {
            return _errorMsg;
        }

        [DllExport("clearError", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        static int clearError()
        {
            _errorMsg = "";
            return 0;
        }

        [DllExport("hasError", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        static bool hasError()
        {
            return _errorMsg != "";
        }
    }
}
