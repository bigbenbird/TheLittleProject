/****************************************************************************
** Meta object code from reading C++ file 'paint.h'
**
** Created: Wed Oct 12 10:46:02 2011
**      by: The Qt Meta Object Compiler version 62 (Qt 4.7.3)
**
** WARNING! All changes made in this file will be lost!
*****************************************************************************/

#include "../paint.h"
#if !defined(Q_MOC_OUTPUT_REVISION)
#error "The header file 'paint.h' doesn't include <QObject>."
#elif Q_MOC_OUTPUT_REVISION != 62
#error "This file was generated using the moc from 4.7.3. It"
#error "cannot be used with the include files from this version of Qt."
#error "(The moc has changed too much.)"
#endif

QT_BEGIN_MOC_NAMESPACE
static const uint qt_meta_data_paint[] = {

 // content:
       5,       // revision
       0,       // classname
       0,    0, // classinfo
       1,   14, // methods
       0,    0, // properties
       0,    0, // enums/sets
       0,    0, // constructors
       0,       // flags
       0,       // signalCount

 // slots: signature, parameters, type, tag, flags
       7,    6,    6,    6, 0x08,

       0        // eod
};

static const char qt_meta_stringdata_paint[] = {
    "paint\0\0draw()\0"
};

const QMetaObject paint::staticMetaObject = {
    { &QObject::staticMetaObject, qt_meta_stringdata_paint,
      qt_meta_data_paint, 0 }
};

#ifdef Q_NO_DATA_RELOCATION
const QMetaObject &paint::getStaticMetaObject() { return staticMetaObject; }
#endif //Q_NO_DATA_RELOCATION

const QMetaObject *paint::metaObject() const
{
    return QObject::d_ptr->metaObject ? QObject::d_ptr->metaObject : &staticMetaObject;
}

void *paint::qt_metacast(const char *_clname)
{
    if (!_clname) return 0;
    if (!strcmp(_clname, qt_meta_stringdata_paint))
        return static_cast<void*>(const_cast< paint*>(this));
    return QObject::qt_metacast(_clname);
}

int paint::qt_metacall(QMetaObject::Call _c, int _id, void **_a)
{
    _id = QObject::qt_metacall(_c, _id, _a);
    if (_id < 0)
        return _id;
    if (_c == QMetaObject::InvokeMetaMethod) {
        switch (_id) {
        case 0: draw(); break;
        default: ;
        }
        _id -= 1;
    }
    return _id;
}
QT_END_MOC_NAMESPACE
