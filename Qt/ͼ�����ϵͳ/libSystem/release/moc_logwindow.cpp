/****************************************************************************
** Meta object code from reading C++ file 'logwindow.h'
**
** Created: Sun Dec 18 20:56:55 2011
**      by: The Qt Meta Object Compiler version 62 (Qt 4.7.3)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "C:/Users/LiJianpeng/Documents/libSystem/logwindow.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'logwindow.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 62
#error "This file was generated using the moc from 4.7.3. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_logWindow[] = {

 // content:
       5,       // revision
       0,       // classname
       0,    0, // classinfo
       3,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       1,       // signalCount

 // signals: signature, parameters, type, tag, flags
      13,   11,   10,   10, 0x05,

 // slots: signature, parameters, type, tag, flags
      36,   10,   10,   10, 0x08,
      55,   10,   10,   10, 0x0a,

       0        // eod
};

static const char qt_meta_stringdata_logWindow[] = {
    "logWindow\0\0,\0logOn(QString,QString)\0"
    "logButtonClicked()\0enableLogButton(QString)\0"
};

const QMetaObject logWindow::staticMetaObject = {
    { &QWidget::staticMetaObject, qt_meta_stringdata_logWindow,
      qt_meta_data_logWindow, 0 }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &logWindow::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *logWindow::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *logWindow::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_logWindow))
        return static_cast<void*>(const_cast< logWindow*>(this));
    return QWidget::qt_metacast(_clname);
}

int logWindow::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QWidget::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: logOn((*reinterpret_cast< const QString(*)>(_a[1])),(*reinterpret_cast< const QString(*)>(_a[2]))); break;
        case 1: logButtonClicked(); break;
        case 2: enableLogButton((*reinterpret_cast< const QString(*)>(_a[1]))); break;
        default: ;
        }
        _id -= 3;
    }
    return _id;
}

// SIGNAL 0
void logWindow::logOn(const QString & _t1, const QString & _t2)
{
    void *_a[] = { 0, const_cast<void*>(reinterpret_cast<const void*>(&_t1)), const_cast<void*>(reinterpret_cast<const void*>(&_t2)) };
    QMetaObject::activate(this, &staticMetaObject, 0, _a);
}
QT_END_MOC_NAMESPACE
