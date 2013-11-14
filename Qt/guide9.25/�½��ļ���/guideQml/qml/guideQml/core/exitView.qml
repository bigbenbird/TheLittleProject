import QtQuick 1.0

Rectangle
{
    id:exitView
    opacity: 0.8
    width: parent.width
    height: parent.height
    // y:0
    color: "powderblue"
    property color buttonBorderColor: "#7A8182"
    property color buttonFillColor: "#61BDCACD"
    gradient: Gradient{
        GradientStop { position: 0.0; color: "#6A7570" }
        GradientStop { position: 1.0; color: Qt.darker("#6A7570") }
    }
    property real perH:0.1
    Rectangle{
        id:textArea
        opacity:2
        width:parent.width/2
        height:parent.height*perH*3
        anchors.centerIn: parent
        color:buttonBorderColor
        x:0
        y:0
        Text{
            //把text放在父级组件的中央
            anchors.centerIn: parent
            id: name
            text: "Do you make sure to quit"
            font.pointSize:20
        }
        gradient: Gradient{
            GradientStop { position: 0.0; color: "#6A7570" }
            GradientStop { position: 1.0; color: Qt.darker("#6A7570") }
        }
    }
    Rectangle{
        id:actionContainer
        color:"transparent"
       // anchors.centerIn: parent
        y:textArea.height
        width:parent.width
        height: exitView.height-textArea.height
        Column{
            //anchors.fill: actionContainer
            //width: actionContainer.width; height: actionContainer.height / 5
            spacing: 3*width/5
            Button{
                id:exitButton
                buttonColor: buttonFillColor
                borderColor: buttonBorderColor
                buttonHeight: actionContainer.height/5; buttonWidth: actionContainer.width
                //y:actionContainer.height-buttonHeight
               // anchors.left: parent.left
               // anchors.horizontalCenter:parent.horizontalCenter
                //anchors.verticalCenter:parent.verticalCenter
               // anchors.top: actionContainer.top
                anchors.topMargin: buttonHeight

                label:"Yes"
                labelSize: 18
                textColor: "black"
                onButtonClick:{
                    myObject.writeSettings()
                    quit()
                }
                gradient: Gradient{
                    GradientStop { position: 0.0; color: Qt.lighter(buttonFillColor,1.25) }
                    GradientStop { position: 0.67; color: Qt.darker(buttonFillColor,1.3) }
                }
            }
            Button{
                id:noButton
                buttonHeight: actionContainer.height/5; buttonWidth: actionContainer.width
               // y:actionContainer.height-buttonHeight
                anchors.top: exitButton.bottom
                anchors.topMargin: buttonHeight
                label:"No"
                labelSize: 18
                onButtonClick:exitView.x=exitView.width
                buttonColor: buttonFillColor
                borderColor: buttonBorderColor
                textColor: "black"
                gradient: Gradient{
                    GradientStop { position: 0.0; color: Qt.lighter(buttonFillColor,1.25) }
                    GradientStop { position: 0.67; color: Qt.darker(buttonFillColor,1.3) }
                }
            }
        }
    }
}

