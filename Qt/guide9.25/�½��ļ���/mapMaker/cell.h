//pp

#ifndef CELL_H
#define CELL_H
class Cell:public QTableWidgetItem
{

public:
        Cell();

        QTableWidgetItem* clone()const;
        void setDirty();
        QString formula()const;
        void setFormula(const QString&);
        QVariant data(int role)const;
        void setData(int role,const QVariant&);

private:
        QVariant value()const;
        QVariant evalExpression(const QString&,int&)const;
        QVariant evalTerm(const QString&,int&)const;
        QVariant evalFactor(const QString&,int&)const;
        mutable QVariant cachedValue;
        mutable bool cachedDirty;
};
#endif
