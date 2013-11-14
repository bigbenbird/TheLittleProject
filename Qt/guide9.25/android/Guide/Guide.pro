#-----------------------------------------------------------
#
# Project created by NecessitasQtCreator 2011-10-13T10:08:11
#
#-----------------------------------------------------------

QT       += core gui \
            sql \
            declarative
RESOURCES += \
    guide.qrc

TARGET = Guide
TEMPLATE = app
ICON = ../../../\guide9.25\android\Guide\applicationIcon.png


SOURCES += \
    ../../../\guide9.25\android\Guide\sql.cpp \
    ../../../\guide9.25\android\Guide\paint.cpp \
    ../../../\guide9.25\android\Guide\mainwindow.cpp \
    ../../../\guide9.25\android\Guide\main.cpp \
    ../../../\guide9.25\android\Guide\gridcell.cpp

HEADERS  += \
    ../../../\guide9.25\android\Guide\sql.h \
    ../../../\guide9.25\android\Guide\paint.h \
    ../../../\guide9.25\android\Guide\mainwindow.h \
    ../../../\guide9.25\android\Guide\gridcell.h

CONFIG += mobility
MOBILITY = 

symbian {
    TARGET.UID3 = 0xee07b138
    # TARGET.CAPABILITY += 
    TARGET.EPOCSTACKSIZE = 0x14000
    TARGET.EPOCHEAPSIZE = 0x020000 0x800000
}

OTHER_FILES += \
    android/AndroidManifest.xml \
    android/res/drawable-hdpi/icon.png \
    android/res/drawable-ldpi/icon.png \
    android/res/drawable-mdpi/icon.png \
    android/res/values/libs.xml \
    android/res/values/strings.xml \
    android/src/eu/licentia/necessitas/industrius/QtActivity.java \
    android/src/eu/licentia/necessitas/industrius/QtApplication.java \
    android/src/eu/licentia/necessitas/industrius/QtLayout.java \
    android/src/eu/licentia/necessitas/industrius/QtSurface.java \
    android/src/eu/licentia/necessitas/ministro/IMinistro.aidl \
    android/src/eu/licentia/necessitas/ministro/IMinistroCallback.aidl \
    android/src/eu/licentia/necessitas/mobile/QtAndroidContacts.java \
    android/src/eu/licentia/necessitas/mobile/QtCamera.java \
    android/src/eu/licentia/necessitas/mobile/QtFeedback.java \
    android/src/eu/licentia/necessitas/mobile/QtLocation.java \
    android/src/eu/licentia/necessitas/mobile/QtMediaPlayer.java \
    android/src/eu/licentia/necessitas/mobile/QtSensors.java \
    android/src/eu/licentia/necessitas/mobile/QtSystemInfo.java \
    ../../../\guide9.25\android\Guide\show.qml \
    ../../../\guide9.25\android\Guide\searchView.qml \
    ../../../\guide9.25\android\Guide\main.qml \
    ../../../\guide9.25\android\Guide\exitView.qml \
    ../../../\guide9.25\android\Guide\button.qml \
    ../../../\guide9.25\android\Guide\aboutView.qml \
    ../../../\guide9.25\android\Guide\aboutQt.qml \
    ../../../\guide9.25\android\Guide\qmldir \
    ../../../\guide9.25\android\Guide\test.db
