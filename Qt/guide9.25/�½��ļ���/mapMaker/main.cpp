#include <QtGui/QApplication>
#include <QTextcodec>
#include "mainwindow.h"

int main(int argc, char *argv[])
{
    QApplication a(argc, argv);
    QTextCodec::setCodecForTr(QTextCodec::codecForName("GB2312"));
    MainWindow w;
    w.show();

    return a.exec();
}
