import QtQuick 1.0
Rectangle
{
    id:main
    signal selectMap()
    signal search(string start,string end)
    signal quit()

    width: screenWidth
    height: screenHeight
    Image
    {
        id:image
        width:screenWidth
        height:screenHeight
        source:"qrc:/guidePic/main.jpg"
        //opacity: 0.5
    }
    //  color: "white"
    //??????
    ListModel
    {
        id:imageModel
        ListElement{name:"search";icon:"qrc:/guidePic/search.png";}
        ListElement{name:"exit";icon:"qrc:/guidePic/exit.png";}
        ListElement{name:"AboutQt";icon:"qrc:/guidePic/qt.png";}
        ListElement{name:"About";icon:"qrc:/guidePic/about.png";}
        ListElement{name:"Select map";icon:"qrc:/guidePic/selectMap.png";}
    }
    //???????????
    Component
    {
        id:imageComponent

        Item
        {
            width:80
            height:80
            scale: PathView.iconScale
            Image
            {
                id: myIcon
                width:80
                height:80
                y:0
                anchors.horizontalCenter: parent.horizontalCenter
                smooth: true
                source: icon
              //  opacity: 0.5
            }
      //      function fun(){Qt.quit();}
            Text
            {
                anchors { top: myIcon.bottom; horizontalCenter: parent.horizontalCenter }
                text: name
                smooth: true
                font.pointSize:10
            }
            MouseArea
            {
                anchors.fill: parent
                onClicked:
                    if(pathView.currentIndex == index)
                        if(name=="exit"){
                               exitView.x=0
                               exitView.y=0
                           }
                        else if(name=="search"){
                                    searchView.y=0;
                                    searchView.x=0;
                        }
                        else if(name=="Select map")
                            main.selectMap()


            }
        }
    }
    //????????
    Component
    {
        id:appHighlight
        Rectangle { width: 80; height: 80; color: "lightsteelblue";opacity: 0.5}

    }
    //????????

    PathView
    {
        id:pathView
        x: 0
        y: 8
        anchors.rightMargin: 0
        anchors.bottomMargin: -8
        anchors.leftMargin: 0
        anchors.topMargin: 8
        anchors.fill: parent
        preferredHighlightBegin: 0.5
        preferredHighlightEnd: 0.5
        focus: true
        highlight: appHighlight
        model: imageModel
        delegate:imageComponent
        Keys.onUpPressed: if (!moving) { incrementCurrentIndex(); console.log(moving) }
        Keys.onDownPressed: if (!moving) { decrementCurrentIndex();console.log(moving)}
        path: Path
        {
            startX: main.width
            startY: 0
            PathAttribute { name: "iconScale"; value: 0.5 }
            PathQuad { x: 50; y: main.height/2; controlX:(main.width-x)*0.1; controlY: y*0.25}
            PathAttribute { name: "iconScale"; value: 1.0 }
            PathQuad { x: main.width; y:main.height; controlX:(main.height-50)*0.1; controlY: y*0.75 }
            PathAttribute { name: "iconScale"; value: 0.5 }
        }
    }
    AboutQt
    {
        id:aboutqt
        width:parent.width
        height:parent.height
        x:parent.width
    }
    About
    {
        id:about
        width: parent.width
        height: parent.height
        x:parent.width
    }
    ExitView
    {
        id:exitView
        width:parent.width
        height:parent.height
        x:parent.width
    }
    SearchView
    {
        id:searchView
        width:parent.width
        height:parent.height
        x:parent.width
     //   column:gridColumn
     //   row:gridRow
    }
}
