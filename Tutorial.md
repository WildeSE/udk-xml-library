# Usage Example #

```
exec function TestXML()
{
   local PlayerController PC;
   local XmlParser xmlTest;
   local string testNode;
   local string dogNode;
   local string rootNode;
   xmlTest = new class'UTGame.XmlParser';
   rootNode = xmlTest.initXML("root");
   xmlTest.loadXMLString("<test><dog age='23'>bannana</dog><cat>apple</cat></test>");
   testNode = xmlTest.firstChild(rootNode, "testNode");
   dogNode = xmlTest.firstChild(testNode, "dogNode");
   
   foreach WorldInfo.AllControllers(class'PlayerController', PC)
   {
      PC.ClientMessage("ROOT: "$rootNode);
      PC.ClientMessage("NODE: "$testNode);
      PC.ClientMessage("innerText: "$xmlTest.innerText(dogNode));
      PC.ClientMessage("name attribute: "$xmlTest.getAttributeByName(dogNode, "age"));
   }
}
```

# Working with files #
```
exec function TestXML()
{
   local PlayerController PC;
   local XmlParser xmlTest;
   local string xmlFilePath;
   local string dialogsNode;
   local string dialogNode;
   local string documentNode;
   local string textNode;
   local int childIdx;
   xmlTest = new class'UTGame.XmlParser';
   documentNode = xmlTest.initXML("document");
   xmlFilePath = xmlTest.getLocalFilePath("test.xml");
   xmlTest.loadXML(xmlFilePath);
   dialogsNode = xmlTest.firstChild(documentNode, "dialogsNode");
   
   foreach WorldInfo.AllControllers(class'PlayerController', PC)
   {
      for(childIdx=0; childIdx < xmlTest.childCount(dialogsNode); childIdx++)
      {
        dialogNode = xmlTest.getChildAtIdx("dialogsNode",childIdx,"dialogNode");
        textNode = xmlTest.firstChild(dialogNode, "textNode");
        PC.ClientMessage("Dialog");
        PC.ClientMessage("Text: "$xmlTest.innerText(textNode));
      }
   }
}
```

Contents of test.xml

```
<dialogs>
    <dialog>
        <text>Hello, how are you today?</text>
        <name>Donald</name>
    </dialog>
    <dialog>
        <text>I am fine.</text>
        <name>Alien</name>
    </dialog>
</dialogs>
```

![http://i16.photobucket.com/albums/b48/shadowisadog/testxml.jpg](http://i16.photobucket.com/albums/b48/shadowisadog/testxml.jpg)