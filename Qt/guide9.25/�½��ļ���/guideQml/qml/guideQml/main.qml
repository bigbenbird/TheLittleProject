import QtQuick 1.0
//import "core"

//Rectangle
//{
//    id:main
//    signal selectMap()
//    signal search(string start,string end)
//    signal quit()
//    width: 360
//    height: 640
//    Image
//    {
//        id:image
//        width:360
//        height:640
//        source:"../../guidePic/main.jpg"
//        //opacity: 0.5

//    }
//    //  color: "white"
//    //设置图标列表
//    ListModel
//    {
//        id:imageModel
//        ListElement{name:"search";icon:"../../guidePic/search.png";}
//        ListElement{name:"exit";icon:"../../guidePic/exit.png";}
//        ListElement{name:"AboutQt";icon:"../../guidePic/qt.png";}
//        ListElement{name:"About";icon:"../../guidePic/about.png";}
//        ListElement{name:"Select map";icon:"../../guidePic/selectMap.png";}
//    }
//    //设置每个图标的显示模板
//    Component
//    {
//        id:imageComponent
//        Item
//        {
//            width:80
//            height:80
//            scale: PathView.iconScale
//            Image
//            {
//                id: myIcon
//                width:80
//                height:80
//                y:0
//                anchors.horizontalCenter: parent.horizontalCenter
//                smooth: true
//                source: icon
//              //  opacity: 0.5
//            }
//      //      function fun(){Qt.quit();}
//            Text
//            {
//                anchors { top: myIcon.bottom; horizontalCenter: parent.horizontalCenter }
//                text: name
//                smooth: true
//                font.pointSize:15
//            }
//            MouseArea
//            {
//                anchors.fill: parent
//                onClicked:
//                    if(pathView.currentIndex == index)
//                        if(name=="exit"){
//                               exitView.x=0
//                               exitView.y=0
//                           }
//                        else if(name=="search"){
//                                    searchView.y=0;
//                                    searchView.x=0;
//                        }
//                        else if(name=="Select map")
//                            main.selectMap()


//            }
//        }
//    }
//    //设置加亮区域模板
//    Component
//    {
//        id:appHighlight
//        Rectangle { width: 80; height: 80; color: "lightsteelblue";opacity: 0.5}

//    }
//    //设置图标排列方式

//    PathView
//    {
//        id:pathView
//        x: 0
//        y: 8
//        anchors.rightMargin: 0
//        anchors.bottomMargin: -8
//        anchors.leftMargin: 0
//        anchors.topMargin: 8
//        anchors.fill: parent
//        preferredHighlightBegin: 0.5
//        preferredHighlightEnd: 0.5
//        focus: true
//        highlight: appHighlight
//        model: imageModel
//        delegate:imageComponent
//        Keys.onUpPressed: if (!moving) { incrementCurrentIndex(); console.log(moving) }
//        Keys.onDownPressed: if (!moving) decrementCurrentIndex()
//        path: Path
//        {
//            startX: main.width
//            startY: 0
//            PathAttribute { name: "iconScale"; value: 0.5 }
//            PathQuad { x: 50; y: main.height/2; controlX:(main.width-x)*0.1; controlY: y*0.25}
//            PathAttribute { name: "iconScale"; value: 1.0 }
//            PathQuad { x: main.width; y:main.height; controlX:(main.height-50)*0.1; controlY: y*0.75 }
//            PathAttribute { name: "iconScale"; value: 0.5 }
//        }

//    }
//    AboutQt
//    {
//        id:aboutqt
//        width:parent.width
//        height:parent.height
//        x:parent.width
//    }
//    About
//    {
//        id:about
//        width: parent.width
//        height: parent.height
//        x:parent.width
//    }
//    ExitView
//    {
//        id:exitView
//        width:360
//        height:640
//        x:parent.width
//    }
//    SearchView
//    {
//        id:searchView
//        width:parent.width
//        height:parent.height
//        x:parent.width
//    }
//}

Rectangle
{
    width: 500;
    height: 500

    // 图片条目数据组织
    VisualItemModel {
        id: itemModel


        Image {
            width: view.width/4; height: view.height/4
            source:"../../guidePic/search.png"
           //  anchors.horizontalCenter: parent.horizontalCenter
        }
        Image {
            width: view.width/4; height: view.height/4
            source:"../../guidePic/exit.png"
        }
        Image {
            width: view.width/4; height: view.height/4
            source:"../../guidePic/qt.png"
        }
        Image {
            width: view.width/4; height: view.height/4
            source:"../../guidePic/main.png"
        }
    }

    ListView {
        id: view
        anchors { fill: parent }
        model: itemModel
        preferredHighlightBegin: 0; preferredHighlightEnd: 0
        highlightRangeMode: ListView.StrictlyEnforceRange
        orientation: ListView.Horizontal
        //snapMode: ListView.NoSnap;
        snapMode: ListView.Nosnap
        //SnapOneItem  SnapToItem  NoSnap

        flickDeceleration: 2000
    }
}




















//    SelectMapView
//    {
//        id:selectMapView
//        width:main.width
//        height:main.height
//        x:parent.width
//    }

//Rectangle
//{
//    id:main
//    width: 360
//    height: 640
//    ListModel{
//        id:imageModel
//        ListElement{name:"search";icon:"../../guidePic/search.png";}
//        ListElement{name:"exit";icon:"../../guidePic/exit.png";}
//        ListElement{name:"AboutQt";icon:"../../guidePic/qt.png";}
//        ListElement{name:"About";icon:"../../guidePic/about.png";}
//        ListElement{name:"Select map";icon:"../../guidePic/selectMap.png";}
//    }
//    Component
//    {

//        id:imageComponent
//        Item
//        {
//            width:80
//            height:80
//            scale: PathView.iconScale
//            Image
//            {
//                id: myIcon
//                width:80
//                height:80
//                y:0
//                anchors.horizontalCenter: parent.horizontalCenter
//                smooth: true
//                source: icon
//                //  opacity: 0.5
//            }
//            //      function fun(){Qt.quit();}
//            Text
//            {
//                anchors { top: myIcon.bottom; horizontalCenter: parent.horizontalCenter }
//                text: name
//                smooth: true
//                font.pointSize:15
//            }
//            MouseArea
//            {
//                anchors.fill: parent
//                onClicked:
//                    if(pathView.currentIndex == index)
//                        if(name=="exit"){
//                            exitView.x=0
//                            exitView.y=0
//                        }
//                        else if(name=="search"){
//                            searchView.y=0;
//                            searchView.x=0;
//                        }

//            }
//        }
//    }
//    GridView{
//        id:gridView
//        anchors.fill: parent
//        model: imageModel
//        delegate: imageComponent


//    }


//}
