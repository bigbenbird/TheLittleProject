#include <QDebug>
#include "gridcell.h"

GridCell::GridCell(QObject *parent) :
    QObject(parent)
{
    m_text="red";
    m_textColor="black";
    m_state="hide";
}
QString GridCell::text()
{
    return m_text;
}
void GridCell::setText(const QString &text)
{
    if(text!=m_text)
    {
        m_text=text;
        emit textChanged();
    }
}
QString GridCell::textColor()
{
    return m_textColor;
}
void GridCell::setTextColor(const QString &color)
{
    //qDebug()<<"setTextColor";
   // qDebug()<<m_text;
    //qDebug()<<"m_text can be used";
    //m_textColor="red";
    //qDebug()<<"can use";
    if(color!=m_textColor)
    {
        qDebug()<<"setTextColor"<<color;
        m_textColor=color;
        emit textColorChanged();
    }
   // qDebug()<<"finish setTextColor";
}
QString GridCell::state()
{
    return m_state;
}
void GridCell::setState(const QString &state)
{
    if(state!=m_state)
    {
        m_state=state;
        emit stateChanged();
    }

}
