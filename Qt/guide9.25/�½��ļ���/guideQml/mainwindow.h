#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QDeclarativeView>
#include"paint.h"
#include"sql.h"


class QWidget;

class interface:public QWidget
{
    Q_OBJECT
public:
    interface(QWidget *parent=0);
    ~interface();
    void show();
    Q_INVOKABLE void writeSettings();
public slots:
    void selectMap();
    void search(QString,QString);
private:
    void readSettings();
    QObject *object;
    QDeclarativeView view;
    QString mapName;

};

#endif
