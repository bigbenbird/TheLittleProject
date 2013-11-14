#ifndef GRIDCELL_H
#define GRIDCELL_H

#include <QObject>

class GridCell : public QObject
{
    Q_OBJECT

    Q_PROPERTY(QString roomNumber READ text WRITE setText NOTIFY textChanged)
    Q_PROPERTY(QString textColor READ textColor WRITE setTextColor NOTIFY textColorChanged)
    Q_PROPERTY(QString imageState READ state WRITE setState NOTIFY stateChanged)
public:
    GridCell(QObject *parent = 0);
    QString text();
    void setText(const QString&);
    QString textColor();
    void setTextColor(const QString&);
    QString state();
    void setState(const QString&);
private:
    QString m_text;
    QString m_textColor;
    QString m_state;
signals:
    void textChanged();
    void textColorChanged();
    void stateChanged();

};

#endif // GRIDCELL_H
