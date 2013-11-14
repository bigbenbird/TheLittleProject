#ifndef PAINT_H
#define PAINT_H

#include <QObject>
#include <QList>
#include <list>
#include "gridcell.h"

//class QPushButton;
class sql;
class QDeclarativeView;
using std::list;



class paint : public QObject
{
    Q_OBJECT

public:
    paint(QString,QString,QString,QWidget *parent = 0);
    ~paint();
    void showPaint();
    bool success;
    void setRowCount(int);
    void setColumnCount(int);
    void setItem(int,int,GridCell*);
    void show();

private slots:
    void draw();
private:
    void create(QString,QString);
    void createItemList(QString);
    void setColor(QString&);

    //list<QTableWidgetItem*> itemList;
    //list<QTableWidgetItem*>::iterator itemIterator;
//    list<list<QTableWidgetItem*>::iterator> itemListLast;
//    list<list<QTableWidgetItem*>::iterator>::iterator itemListLastIterator;
    QList<QObject*> itemList;
    QList<QObject*>::iterator itemIterator;
    list<QList<QObject*>::iterator>  itemListLast;
    list<QList<QObject*>::iterator>::iterator itemListLastIterator;
    list<int> columnNumList;
    list<int>::iterator columnNum;
    list<int> rowNumList;
    list<int>::iterator rowNum;
    QDeclarativeView *view;
//    QPushButton *nextButton;
//    QPushButton *returnButton;
    sql* database;
};

#endif // PAINT_H
