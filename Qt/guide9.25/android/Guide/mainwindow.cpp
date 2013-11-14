#include <QDeclarativeView>
#include <QDeclarativeContext>
#include <QtGui>
#include <QtDebug>
#include "sql.h"
#include "mainwindow.h"
#include "paint.h"


interface::interface(QWidget *parent):QWidget(parent)
{
    readSettings();
    QFont AndroidFont("system/fonts/DroidSans.ttf", 12,QFont::Bold,false);
      //set font of application
    QApplication::setFont(AndroidFont);

    int screenWidth = QApplication::desktop()->width();
    int screenHeight = QApplication::desktop()->height();
    view.rootContext()->setContextProperty("myObject", this);
  //  view.rootContext()->setContextProperty("searchSource",QDir::toNativeSeparators("../../guidePic/search.png"));
    view.rootContext()->setContextProperty("screenWidth",screenWidth);
    view.rootContext()->setContextProperty("screenHeight",screenHeight);

    view.setSource(QUrl("qrc:/main.qml"));
    object=view.rootObject();



    QObject::connect(object,SIGNAL(selectMap()),
                     this,SLOT(selectMap()));
    QObject::connect(object,SIGNAL(search(QString,QString)),
                     this,SLOT(search(QString,QString)));
    QObject::connect(object, SIGNAL(quit()),
                     QCoreApplication::instance(), SLOT(quit()));

}
void interface::show()
{
    view.showFullScreen();
}
void interface::selectMap()
{
    mapName=QFileDialog::getOpenFileName(
                this,tr("Open Map"),
                ".",tr("Map File (*.db)"));
}
void interface::search(QString start,QString end)
{
    if(start=="")
    {

        QMessageBox::critical(this,tr("roomGuide"),
                              tr("please input a start room's number"),
                              tr("Yes,I know"));
        return;
    }
    if(end=="")
    {
        QMessageBox::critical(this,tr("roomGuide"),
                              tr("please input a end room's number"),
                              tr("Yes,I know"));
        return;
    }
    if(mapName==0)
    {
        /*
        QMessageBox::critical(this,tr("roomSearch"),
                              tr("No map to search.\n plaese select a map\n"),
                              tr("Yes,I konw"));
        return;
        */
        mapName=QDir::toNativeSeparators("qrc:/test.db");
    }
    paint *resultPaint=new paint(mapName,QString(start),QString(end),this);
    if(resultPaint->success==true)
    {
        resultPaint->show();
        this->update();
    }
}
void interface::writeSettings()
{
    QSettings settings("Software Inc","roomSearch");
    settings.setValue("MapName",mapName);

}
void interface::readSettings()
{
    QSettings settings("Software Inc","roomSearch");
    mapName=settings.value("MapName").toString();

}
interface::~interface()
{
    delete  object;
}

