#ifndef PAINT_H
#define PAINT_H

#include <QTableWidget>
#include <list>

class QTableWidgetItem;
class QPushButton;
class sql;
using std::list;

class paint : public QTableWidget
{
    Q_OBJECT
public:
    paint(QString,QString,QString,QWidget *parent = 0);
    ~paint();
    void showPaint();
    bool success;

private slots:
    void draw();
private:
    void create(QString);
    void createItemList(QString string);
    void setColor();

    list<QTableWidgetItem*> itemList;
    list<QTableWidgetItem*>::iterator itemIterator;
    list<list<QTableWidgetItem*>::iterator> itemListLast;
    list<list<QTableWidgetItem*>::iterator>::iterator itemListLastIterator;
    list<int> columnNumList;
    list<int>::iterator columnNum;
    list<int> rowNumList;
    list<int>::iterator rowNum;
    QPushButton *nextButton;
    QPushButton *returnButton;
    sql* database;
};

#endif // PAINT_H
