#include <QtGui>
#include <string>
#include <QDebug>
#include <list>
#include "sql.h"
#include "paint.h"
#include "mainwindow.h"
using std::string;
using std::list;
paint::paint(QString map,QString startRoom,QString endRoom,QWidget *parent) :
    QTableWidget(parent)
{
    setShowGrid(false);
    verticalHeader()->setVisible(false);
    horizontalHeader()->setVisible(false);
    setEditTriggers ( QAbstractItemView::NoEditTriggers );

    setItemPrototype(new QTableWidgetItem);
    returnButton=0;
    nextButton=0;
    rowNum=rowNumList.end();
    columnNum=columnNumList.end();

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
    create(string);
    itemListLastIterator=itemListLast.end();
    draw();
}
void paint::create(QString str)
{
    qDebug()<<"create";
    QStringList stringList=str.split("opposite\t");
    int number=stringList.count();
    for(int i=0;i<number;++i)
    {
        createItemList(stringList[i]);
        setColor();
    }
}
void paint::createItemList(QString stringOld)
{
    qDebug("createItemList");
    QString string=stringOld.split("Message")[0];
    QStringList rows=string.split('\n');
    list<QTableWidgetItem*> itemList2;
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
            QTableWidgetItem *item=new QTableWidgetItem;
            item->setText(columns[j]);
            itemList2.push_back(item);
        }
        qDebug()<<"finish";
    }
    QTableWidgetItem *item=new QTableWidgetItem;
    item->setText(stringOld.split("Message")[1]);
    itemList2.push_back(item);

    itemIterator=itemList2.begin();
    itemListLast.push_back(--itemList2.end());
    itemList.splice(itemList.end(),itemList2);
    return;
}
void paint::setColor()
{
    qDebug()<<"setColor";
    QString str=itemList.back()->text();
    string str2;
    str2=str.toStdString();
   // qDebug()<<str;
    int x=int(str2[0]);
    int y=int(str2[1]);
    int z=int(str2[2]);
    qDebug()<<x<<y<<z;

    list<QTableWidgetItem*>::iterator item=itemIterator ;
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
  //  if(rowNum==1) rowNum=2;
    while(i<=(rowNum-1)*(columnNum))
    {
        qDebug()<<i;
        if(i<columnNum+1 &&(i-x)*(i-z)<=0)
            (*item)->setTextColor(Qt::red);
        if((i-z)%columnNum==0)
            (*item)->setTextColor(Qt::red);
        if(i>columnNum*(rowNum-2) && (i%columnNum-y)*(i%columnNum-z)<=0)
            (*item)->setTextColor(Qt::red);
        if(i>columnNum*(rowNum-2) && y%columnNum==0 && i%columnNum==0)
            (*item)->setTextColor(Qt::red);
        item++;
        i++;
    }
}
void paint::draw()
{
    qDebug()<<"draw";
    if(returnButton)
        delete returnButton;
    returnButton=0;
    if(nextButton)
        delete nextButton;
    nextButton=0;
    setRowCount(0);
    setColumnCount(0);
    if(rowNum==rowNumList.end())
        rowNum=rowNumList.begin();
    else
        rowNum++;
    if(columnNum==columnNumList.end())
        columnNum=columnNumList.begin();
    else
        columnNum++;
    setRowCount(*rowNum);
    setColumnCount(*columnNum);
    qDebug()<<*rowNum<<*columnNum-1;

    list<QTableWidgetItem*>::iterator item;
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
    while(item!=(*itemListLastIterator))
    {
        setItem(i/(*columnNum),i%(*columnNum),(*item));
        i++;
        item++;
    }
    setItem(i/(*columnNum),i%(*columnNum),(*item));
    i++;


    if(itemListLastIterator!=--itemListLast.end())
    {
        nextButton=new QPushButton(tr("nextMap"),this);
        this->setCellWidget(i/(*columnNum),i%(*columnNum),nextButton);
        connect(nextButton,SIGNAL(clicked()),
                this,SLOT(draw()));
    }
    qDebug()<<"startreturnButton1";
    qDebug()<<returnButton;
    qDebug()<<"startreturnButton1";
    qDebug()<<"creatButton";
    returnButton=new QPushButton(tr("return"),this);
    qDebug()<<"i:"<<i;
    this->setCellWidget(i/(*columnNum),0,returnButton);
    qDebug()<<"setCellWidget";
    connect(returnButton,SIGNAL(clicked()),
            parent(),SLOT(returnMain()));
    qDebug()<<"connect";
    this->update();
}
paint::~paint()
{
    itemList.clear();
    itemListLast.clear();
}
