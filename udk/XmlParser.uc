class XmlParser extends Object
       DLLBind(UdkXmlParser);

dllimport final function string initXML(string documentNodeName);
dllimport final function bool loadXML(string fileName);
dllimport final function bool loadXMLString(string xmlString);
dllimport final function string firstChild(string targetNodeName, string resultNodeName);
dllimport final function string lastChild(string targetNodeName, string resultNodeName);
dllimport final function bool hasChildren(string targetNodeName);
dllimport final function int childCount(string targetNodeName);
dllimport final function string getChildAtIdx(string targetNodeName, int nodeIdx, string resultNodeName);
dllimport final function string parentNode(string targetNodeName, string resultNodeName);
dllimport final function string nextSibling(string targetNodeName, string resultNodeName);
dllimport final function string previousSibling(string targetNodeName, string resultNodeName);
dllimport final function int attributeCount(string targetNodeName);
dllimport final function string getAttributeByIdx(string targetNodeName, int attributeIdx);
dllimport final function string getAttributeByName(string targetNodeName, string attributeName);
dllimport final function string innerText(string targetNodeName);
dllimport final function string innerXML(string targetNodeName);
dllimport final function string getError();
dllimport final function int clearError();
dllimport final function bool hasError();

defaultproperties
{
    
}