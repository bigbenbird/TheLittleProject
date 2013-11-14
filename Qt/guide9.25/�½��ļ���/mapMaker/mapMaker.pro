#-------------------------------------------------
#
# Project created by QtCreator 2011-08-19T15:22:30
#
#-------------------------------------------------

QT       += core gui\
            sql

TARGET = mapMaker
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    spreadsheet.cpp \
    gotoCell.cpp \
    findDialog.cpp

HEADERS  += mainwindow.h \
    spreadsheet.h \
    gotocell.h \
    findDialog.h

RESOURCES += \
    image.qrc
