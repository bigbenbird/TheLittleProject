import QtQuick 1.0

Rectangle {
    // width: parent.width; height: parent.height
     id:map

     z:1

     color:"powderblue"
    // anchors.fill:parent
     gradient: Gradient{
         GradientStop { position: 0.0; color: "lightblue" }
         GradientStop { position: 1; color: Qt.darker("blue") }
     }
//     BorderImage {
//         id: background
//         anchors.fill: parent
//         source: "../../../guidePic/goujiaoyin3.jpg"
//         //rotation:90
//     }

     property string imageState :"right";  
  //   property int gridColumn:1;
   //  property int gridRow:1;
     property int cellWidth:60
     property int cellHeight:60
    // property string text:"NULL"
    // property QtObject gridModel
     property int mapWidth
     property int mapHeight

     Button{
        id:returnButton
        rotation:90
        x:parent.width/10*7.5
        y:parent.height/10
        buttonWidth: parent.width*0.3
        buttonHeight: parent.height/10
        label: "Return!!"
        labelSize: 10
        buttonColor:"#61BDCACD"
        gradient: Gradient{
            GradientStop { position: 0.0; color: "#6A7570" }
            GradientStop { position: 1.0; color: Qt.darker("#6A7570") }
        }
        onButtonClick:parent.x=parent.width
     }
     width: mapWidth;height: mapHeight
     Component {
         id: contactDelegate
         Item {
             width: cellWidth; height: cellHeight


             Rectangle
             {
                 id:itemRectangle
                 smooth: true
                 width: cellWidth
                 height:cellHeight
                 border.color: "black"
                 //border.width: 5
                 color: "transparent"
                 Image {
                     id: arrow
                     width: cellWidth
                     height: cellHeight
                     opacity:1
                     source: "qrc:/guidePic/arrow2.png"

                     state: imageState
                     states: [
                         State {
                             name: "left"
                             PropertyChanges {target:arrow;rotation:180}
                         },
                         State{
                             name: "right"
                             PropertyChanges{target:arrow;rotation:0}
                         },
                         State{
                             name:"up"
                             PropertyChanges{target: arrow;rotation:270}
                         },
                         State{
                             name:"down"
                             PropertyChanges{target: arrow;rotation:90}
                         },
                         State{
                             name:"hide"
                             PropertyChanges{target: arrow;visible: false}
                         }
                     ]
                 }
                Rectangle {
                     id: stair
                     //width:cellWidth
                     //height:cellHeight
                     color: "transparent"
                     Column
                     {
                         anchors.fill: parent
                         Rectangle{width: parent.width;height: parent.height/3;color: "transparent";border.color: "black"}
                         Rectangle{width: parent.width;height: parent.height/3;color: "transparent";border.color: "black"}
                         Rectangle{width: parent.width;height: parent.height/3;color: "transparent";border.color: "black"}
                     }
                     state:roomNumber=="stair"?"show":"hide"
                     anchors.fill: parent
                     //width: cellWidth
                     //height: cellHeight
                     states: [
                         State {
                             name: "hide"
                             PropertyChanges {target: stair;visible:false}
                         },
                         State{
                             name: "show"
                             PropertyChanges { target: stair;visible:true}
                         }
                     ]
                 }
                 Text {
                     width: cellWidth
                     height: cellHeight
                     style:Text.Sunken
                     styleColor: "lightblue"
                     text: roomNumber
                     color: textColor
                     elide:Text.ElideMiddle
                     //smooth :true
                    // anchors.fill: parent
                     font.pointSize:roomNumber=="stair"?10:12
                     font.bold : textColor=="red"?true:false
                     horizontalAlignment: Text.AlignHCenter
                     verticalAlignment:Text.AlignVCenter
                     //anchors.horizontalCenter: parent.horizontalCenter
                     //anchors.verticalCenter: parent.verticalCenter
                 }

             }
         }
     }


//     Rectangle{
//         id:flickableRectangle
//         //opacity:0

//         color:"transparent"

//         y:map.height-grid.width*1.1
//         anchors.horizontalCenter: parent.horizontalCenter
//        // anchors.verticalCenter: parent.verticalCenter
//         width:parent.width
//         height: parent.height-returnButton.height
         Flickable{
             id:flickable
             //anchors.fill: parent
             //anchors.horizontalCenter: parent.horizontalCenter
             //anchors.verticalCenter: parent.verticalCenter
             //
             //width:parent.height
             //height: parent.width
             contentWidth:grid.width
             contentHeight: grid.height
             clip:false
             pressDelay:0
             interactive:true
             flickableDirection:Flickable.HorizontalAndVerticalFlick

             Grid {
                 id: grid
                 spacing:10
                 rotation:90
                 rows: gridRow
                 columns:gridColumn
                 x:-returnButton.height
                 y:map.height/2-grid.height/2
                 //anchors.horizontalCenter: parent.horizontalCenter
                 //anchors.verticalCenter: parent.verticalCenter
                 Repeater{
                     model: gridModel
                     delegate: contactDelegate
                 }
             }
         }
//     }
}
