#include <QtGui>
#include <string>
#include <QDebug>
#include <list>
#include <QDeclarativeView>
#include <QDeclarativeContext>
#include "sql.h"
#include "paint.h"
#include "mainwindow.h"
#include "gridcell.h"
#include <QtCore>
using std::string;
using std::list;
paint::paint(QString map,QString startRoom,QString endRoom,QWidget *parent) :
    QObject(parent)
{
//    setShowGrid(false);
//    verticalHeader()->setVisible(false);
//    horizontalHeader()->setVisible(false);
//    setEditTriggers ( QAbstractItemView::NoEditTriggers );

//    setItemPrototype(new QTableWidgetItem);
//    returnButton=0;
//    nextButton=0;
    rowNum=rowNumList.end();
    columnNum=columnNumList.end();
    view= qobject_cast<interface*>(this->parent())->qmlView();

    database=new sql(map);
    QString string;
    string=database->findPath(startRoom,endRoom);
    qDebug()<<string;
    if(string.isEmpty())
    {
        success=false;
        return;
    }
    else
        success=true;
 //   itemListLastIterator;
    create(string,startRoom);
    itemListLastIterator=itemListLast.end();
    draw();

}
void paint::create(QString str,QString start)
{
    qDebug()<<"create";
    QStringList stringList=str.split("opposite\t");
    int number=stringList.count();
    for(int i=0;i<number;++i)
    {
        createItemList(stringList[i]);
        setColor(start);
    }
}
void paint::createItemList(QString stringOld)
{
    qDebug("createItemList");
    QString string=stringOld.split("Message")[0];
    QStringList rows=string.split('\n');
    QList<QObject*> itemList2;
    int rowNum=rows.count();
    rowNumList.push_back(rowNum);
    qDebug()<<rowNum;
    int columnNum=rows.first().count('\t')+1;
    columnNumList.push_back(columnNum);
   // qDebug()<<rows.first();
    qDebug()<<columnNum;
    for(int i=0;i<rowNum-1;++i)
    {
        QStringList columns=rows[i].split('\t');
        qDebug()<<columns;
        for(int j=0;j<columnNum;++j)
        {
       //     qDebug()<<columns[j];
            GridCell *item=new GridCell;
            item->setText(columns[j]);
            itemList2.append(item);
        }
        qDebug()<<"finish";
    }
    GridCell *item=new GridCell;
    item->setText(stringOld.split("Message")[1]);
    itemList2.append(item);

    itemIterator=itemList2.begin();
    itemListLast.push_back(--itemList2.end());
    itemList.append(itemList2);
    return;
}
void paint::setColor(QString& start)
{
    qDebug()<<"setColor";
    QString str=qobject_cast<GridCell*>(itemList.back())->text();
    string str2;
    str2=str.toStdString();
   // qDebug()<<str;
    int x=int(str2[0]);
    int y=int(str2[1]);
    int z=int(str2[2]);
    qDebug()<<x<<y<<z;

    QList<QObject*>::iterator item=itemIterator ;
 //   list<QLabel*> labelList2(label,labelList.end());
 //   int n=labelList2.size();
    int min=x>y?y:x;
    min=min>z?z:min;
    x-=min-1;
    y-=min-1;
    z-=min-1;
    int i=1;
    int columnNum=columnNumList.back();
    int rowNum=rowNumList.back();
    qDebug()<<"x y z"<<x<<y<<z;
    qDebug()<<"columnNum"<<columnNum;
    qDebug()<<"rowNum"<<rowNum;
    QString state1,state2,state3;
    state3=qobject_cast<GridCell*>(*(item+x-1))->text()==start?"down":"up";
    if(state3=="up")
    {
        state1=x>z?"right":"left";
        state2=y>z?"left":"right";
    }
    else
    {
        state1=x>z?"left":"right";
        state2=y>z?"right":"left";
    }
  //  if(rowNum==1) rowNum=2;
    while(i<=(rowNum-1)*(columnNum) && (*item))
    {
        qDebug()<<i;
        if(i<columnNum+1 &&(i-x)*(i-z)<=0)
        {
            qobject_cast<GridCell*>(*item)->setState(state1);
            qobject_cast<GridCell*>(*item)->setTextColor(tr("red"));
        }
        if((i-z)%columnNum==0)
        {
            qobject_cast<GridCell*>(*item)->setState(state3);
            qobject_cast<GridCell*>(*item)->setTextColor(tr("red"));
        }
        if(i>columnNum*(rowNum-2) && (i%columnNum-y)*(i%columnNum-z)<=0)
        {
            qobject_cast<GridCell*>(*item)->setState(state2);
            qobject_cast<GridCell*>(*item)->setTextColor(tr("red"));
        }
        if(i>columnNum*(rowNum-2) && y%columnNum==0 && i%columnNum==0)
        {
            qobject_cast<GridCell*>(*item)->setState(state2);
            qobject_cast<GridCell*>(*item)->setTextColor(tr("red"));
        }
        item++;
        i++;
    }
    qDebug()<<"finish setColor";
}
void paint::draw()
{
    qDebug()<<"draw";
//    if(returnButton)
//        delete returnButton;
//    returnButton=0;
//    if(nextButton)
//        delete nextButton;
//    nextButton=0;
    //setRowCount(0);
    //setColumnCount(0);

    if(rowNum==rowNumList.end())
        rowNum=rowNumList.begin();
    else
        rowNum++;
    if(columnNum==columnNumList.end())
        columnNum=columnNumList.begin();
    else
        columnNum++;
    setRowCount(*rowNum-1);
    setColumnCount(*columnNum);
    qDebug()<<"hello3";
    qDebug()<<*rowNum<<*columnNum-1;

    QList<QObject*>::iterator item;
    if(itemListLastIterator==itemListLast.end())
    {
        itemListLastIterator=itemListLast.begin();
        item=itemList.begin();
    }
    else
    {
        item=(*itemListLastIterator);
        item++;
        itemListLastIterator++;
    }

    int i=0;
    while(item!=(*itemListLastIterator) && item!=itemList.end())
    {
        qDebug()<<i;
        setItem(i/(*columnNum),i%(*columnNum),qobject_cast<GridCell*>(*item));
        i++;
        item++;
    }
    //setItem(i/(*columnNum),i%(*columnNum),qobject_cast<GridCell*>(*item));
    i++;
    qDebug("end");

//    if(itemListLastIterator!=--itemListLast.end())
//    {
//        nextButton=new QPushButton(tr("nextMap"));
//        //this->setCellWidget(i/(*columnNum),i%(*columnNum),nextButton);
//        connect(nextButton,SIGNAL(clicked()),
//                this,SLOT(draw()));
//    }
//    qDebug()<<"startreturnButton1";
//    qDebug()<<returnButton;
//    qDebug()<<"startreturnButton1";
//    qDebug()<<"creatButton";
//    returnButton=new QPushButton(tr("return"));//,this);
//    qDebug()<<"i:"<<i;
//    //this->setCellWidget(i/(*columnNum),0,returnButton);
//    qDebug()<<"setCellWidget";
//    connect(returnButton,SIGNAL(clicked()),
//            parent(),SLOT(returnMain()));
//    qDebug()<<"connect";
    //this->update();
}
paint::~paint()
{
    itemList.clear();
    itemListLast.clear();
}
void paint::show()
{
    qDebug()<<"show";
   // view.rootContext()->setContextProperty("GridModel", QVariant::fromValue(itemList));
    view->rootContext()->setContextProperty("gridModel",QVariant::fromValue(itemList));
   // v.setSource(QUrl::fromLocalFile("./qml/guideQml/core/show.qml"));
    //view->show();
    qDebug()<<"endshow";
}

void paint::setRowCount(int row)
{
    view->rootContext()->setContextProperty("gridRow",row);
    qDebug()<<"setRowCount"<<row;
}
void paint::setColumnCount(int column)
{
    view->rootContext()->setContextProperty("gridColumn",column);
    qDebug()<<"setColumnCount"<<column;
}
void paint::setItem(int,int,GridCell*)
{
}
