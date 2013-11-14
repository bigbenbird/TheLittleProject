//#include<QTableWidgetItem>
//pp
#include<QtGui>
#include"cell.h"

Cell::Cell()
{
        setDirty();
}
QTableWidgetItem* Cell::clone()const
{
        return new Cell(*this);
}
void Cell::setDirty()
{
        cachedDirty=true;
}
void Cell::setData(int role,const QVariant& formula)
{
        QTableWidgetItem::setData(role,formula);
        if(role==Qt::EditRole)
                setDirty();
}
QVariant Cell::data(int role)const
{
        if(role==Qt::DisplayRole)
        {
                if(value().isValid())
                        return value().toString();
                else
                        return "####";

        }
        else if(role==Qt::TextAlignmentRole)
        {
                if(value().type()==QVariant::String)
                        return int(Qt::AlignLeft | Qt::AlignVCenter);
                else
                        return int(Qt::AlignRight | Qt::AlignVCenter);
        }
        else
                return QTableWidgetItem::data(role);
}
const QVariant Invalid;
QVariant Cell::value()const
{
        if(cachedDirty)
        {
                cachedDirty=false;
                QString string=formula();
                if(string.startsWith('\''))
                        cachedValue=string.mid(1);
                else
                        if(string.startsWith('='))
                        {
                                cachedValue=Invalid;
                                QString expr=string.mid(1);
                                expr.replace(" ","");
                                expr.append(QChar::Null);
                                int pos=0;
                                cachedValue=evalExpression(expr,pos);
                                if(expr[pos]!=QChar::Null)
                                        cachedValue=Invalid;
                        }
                        else
                        {
                                bool ok;
                                double stringDouble=string.toDouble(&ok);
                                if(ok)
                                        cachedValue=stringDouble;
                                else
                                        cachedValue=string;
                        }
        }
        return cachedValue;
}
