#include <QtGui/QApplication>
#include "mainwindow.h"

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    interface w;
    w.show();

//    QDeclarativeView view;
//    view.setSource(QUrl::fromLocalFile("./qml/guideQml/main.qml"));
//    view.show();

    return a.exec();
}
