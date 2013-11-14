import QtQuick 1.0
//hello
Rectangle
{
    id:searchView

   // property int column:10;
  //  property int row:1;

    width: parent.width
    height: parent.height
    color: "powderblue"
    gradient: Gradient{
        GradientStop { position: 0.0; color: "lightblue" }
        GradientStop { position: 1.0; color: Qt.darker("blue") }
    }
    Rectangle{
        id:startRectangle
        state: "STARTRECTANGLE_CLOSED"
        gradient: Gradient{
            GradientStop { position: 0.0; color: "lightblue" }
            GradientStop { position: 1.0; color: Qt.darker("blue") }
        }
        width: parent.width
        height: parent.height*0.4
        y:-height
        Text{
            id: startLabel
            anchors.horizontalCenter:parent.horizontalCenter
            // width: parent.width
            //height: parent.height/20
            x:0
            y:0
            text: "Start Room"
            font.pointSize:20
        }
        Text
        {
            anchors.horizontalCenter: parent.horizontalCenter
            anchors.top: startLabel.bottom
            font.pointSize:8
            id:startDetail
            text: "(enter the number of the room \n which you are in)"
        }
        Rectangle{
            anchors.verticalCenter: parent.verticalCenter
            id:startEditRectangle
            anchors.top: startDetail.bottom
            anchors.topMargin: 20
            anchors.horizontalCenter: parent.horizontalCenter
            width: parent.width*0.8
            smooth: true
            height:parent.height/2
            border { width: 2; color: "lightsteelblue" }
            color: "blue"
            TextEdit
            {
                width:parent.width;height: parent.height
                anchors.fill: parent
                id:startEdit
                font.pointSize:15
                color: "black"
                selectionColor:"black"
                wrapMode: TextEdit.Wrap
                //            MouseArea{
                //                id:startMouseArea
                //                anchors.fill: parent
                //                hoverEnabled: true
                //                onEntered: {

                //                    startEditRectangle.border.color = "lightsteelblue"
                //                }
                //                onExited:{
                //                    anchors.fill=parent
                //                    startEditRectangle.border.color = "transparent"
                //                }
                //                onClicked:startMouseArea.x=searchView.width
                //            }
            }
            gradient: Gradient{
                GradientStop { position: 0.0; color: "lightblue" }
                GradientStop { position: 1.0; color: Qt.darker("blue") }
            }
        }

        states:[
            State {
                name: "STARTRECTANGLE_CLOSE"
                PropertyChanges {target: startRectangle;y:-startRectangle.height}
                PropertyChanges {target: startDrawer;y:0}
                PropertyChanges {target: startDrawer;rotation: 180}
            },
            State {
                name: "STARTRECTANGLE_OPEN"
                PropertyChanges {target: startRectangle;y:0}
                PropertyChanges{target: startDrawer;y:startRectangle.height}
                PropertyChanges {target: startDrawer;rotation:180}
            }
        ]
        transitions: [
            Transition {
                to:"*"
                NumberAnimation { target: startRectangle; property: "y"; duration: 100;easing.type:Easing.OutExpo }
                NumberAnimation { target: startDrawer; property: "y";  duration: 100;easing.type: Easing.OutExpo }
            }
        ]
    }
    Rectangle {
        id: startDrawer
        height: 30; width: parent.width
        border { color : "#6A6D6A"; width: 1 }
        z: 1
        gradient: Gradient {
                GradientStop { position: 0.0; color: "#8C8F8C" }
                GradientStop { position: 0.17; color: "#6A6D6A" }
                GradientStop { position: 0.77; color: "#3F3F3F" }
                GradientStop { position: 1.0; color: "#6A6D6A" }
            }
        Image {
            id: startArrowIcon
            source: "qrc:/guidePic/arrow.png"
            anchors.horizontalCenter: parent.horizontalCenter
            Behavior{ NumberAnimation { property: "rotation"; easing.type: Easing.OutExpo } }
        }

        MouseArea {
            id: startDrawerMouseArea
            anchors.fill: parent
            hoverEnabled: true
            onEntered: parent.border.color = Qt.lighter("#6A6D6A")
            onExited:  parent.border.color = "#6A6D6A"
            onClicked: {
                if (startRectangle.state == "STARTRECTANGLE_CLOSED") {
                    startRectangle.state = "STARTRECTANGLE_OPEN"
                }
                else if (startRectangle.state == "STARTRECTANGLE_OPEN"){
                    startRectangle.state = "STARTRECTANGLE_CLOSED"
                }
            }
        }
    }
    Rectangle {
        id: endDrawer
        rotation:180
        height: 30; width: parent.width
        border { color : "#6A6D6A"; width: 1 }
        y:parent.height-height
        z: 1
        gradient: Gradient {
                GradientStop { position: 0.0; color: "#8C8F8C" }
                GradientStop { position: 0.17; color: "#6A6D6A" }
                GradientStop { position: 0.77; color: "#3F3F3F" }
                GradientStop { position: 1.0; color: "#6A6D6A" }
            }
        Image {
            id: endArrowIcon
            source: "qrc:/guidePic/arrow.png"
            anchors.horizontalCenter: parent.horizontalCenter
            Behavior{ NumberAnimation { property: "rotation"; easing.type: Easing.OutExpo } }
        }

        MouseArea {
            id: endDrawerMouseArea
            anchors.fill: parent
            hoverEnabled: true
            onEntered: parent.border.color = Qt.lighter("#6A6D6A")
            onExited:  parent.border.color = "#6A6D6A"
            onClicked: {
                if (endRectangle.state == "ENDRECTANGLE_CLOSED") {
                   endRectangle.state = "ENDRECTANGLE_OPEN"
                }
                else if (endRectangle.state == "ENDRECTANGLE_OPEN"){
                    endRectangle.state = "ENDRECTANGLE_CLOSED"
                }
            }
        }
    }
    Rectangle{
        id: endRectangle
        state:"ENDRECTANGLE_CLOSED"
        gradient: Gradient{
            GradientStop { position: 0.0; color: "lightblue" }
            GradientStop { position: 1.0; color: Qt.darker("blue")}
        }
        width: parent.width
        height: parent.height*0.4
        y:parent.height
        Text
        {
            id:endText
            anchors.horizontalCenter: parent.horizontalCenter
            text: "End Room"
            font.pointSize:20
        }
        Text
        {
            id:endDetail
            anchors.horizontalCenter: parent.horizontalCenter
            anchors.top: endText.bottom
            font.pointSize: 8
            text:"(enter the number of the room \n which you want to go)"
        }
        Rectangle{
            id:endEditRectangle
            anchors.verticalCenter: parent.verticalCenter
            anchors.horizontalCenter: parent.horizontalCenter
            anchors.top: endDetail.bottom
            anchors.topMargin: parent.height/5
            width: parent.width*0.8
            smooth: true
            height: startEditRectangle.height
            border { width: 2; color: "lightsteelblue" }
            color: "blue"
            TextEdit
            {
                id:endEdit
                width:parent.width;height: parent.height
                //anchors.fill: parent
                font.pointSize:15
                color: "black"
                selectionColor:"black"
                wrapMode: TextEdit.Wrap
            }
            gradient: Gradient{
                GradientStop { position: 0.0; color: "lightblue" }
                GradientStop { position: 1.0; color: Qt.darker("blue") }
            }
        }
        states:[
            State {
                name: "ENDRECTANGLE_CLOSE"
                PropertyChanges {target: endRectangle;y:searchView.height}
                PropertyChanges {target: endDrawer;y:searchView.height-endDrawer.height}
                PropertyChanges {target: endDrawer;rotation: 180}
            },
            State {
                name: "ENDRECTANGLE_OPEN"
                PropertyChanges {target: endRectangle;y:searchView.height-height}
                PropertyChanges{target: endDrawer;y:searchView.height-endRectangle.height-endDrawer.height}
                PropertyChanges {target: endDrawer;rotation:0}
            }
        ]
        transitions: [
            Transition {
                to:"*"
                NumberAnimation { target: endRectangle; property: "y"; duration: 100;easing.type:Easing.OutExpo }
                NumberAnimation { target: endDrawer; property: "y";  duration: 100;easing.type: Easing.OutExpo }
            }
        ]
    }
    Rectangle{
        id:buttonRectangle
        width: parent.width
        height: parent.height/5
        anchors.verticalCenter: parent.verticalCenter
        anchors.horizontalCenter: parent.horizontalCenter
        gradient: Gradient{
            GradientStop { position: 0.0; color: "lightblue" }
            GradientStop { position: 1.0; color: Qt.darker("blue") }
        }
        Image {
            id: searchButtonImage
            source: "qrc:/guidePic/main.png"
            width:parent.width*0.15
            height: width
            anchors.verticalCenter: parent.verticalCenter
           // anchors.left: endText.left
        }
        Button
        {
            id:searchButton
            anchors.verticalCenter: parent.verticalCenter
            anchors.topMargin: 20
            anchors.left: searchButtonImage.right
            anchors.leftMargin: 10
            buttonWidth:parent.width-returnButton.buttonWidth-searchButtonImage.width-25
            height: returnButton.height
            label: "Search Now!!"
            labelSize: 10
            buttonColor:"#61BDCACD"
            onButtonClick:{
                search(startEdit.text,endEdit.text)
                map.x=0;
                map.y=0
            }
        }
        Button
        {
            id:returnButton
            anchors.left: searchButton.right
            anchors.leftMargin: 10
            anchors.top: searchButton.top
            buttonWidth:buttonHeight
            buttonHeight: parent.height*0.6
            label: "Return!!"
            labelSize: 8
            buttonColor:"#61BDCACD"
            onButtonClick:searchView.x=searchView.width
        }
    }
    Map
    {
        id:map
        x:parent.height
        mapWidth:parent.width
        mapHeight:parent.height
        //gridColumn:column
        //gridRow:row
    }
}
