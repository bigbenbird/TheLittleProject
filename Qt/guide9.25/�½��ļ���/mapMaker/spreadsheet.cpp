//pp
#include<QtDebug>
#include<QtGui>
#include<QtSql>
#include<vector>
//#include <QSQLiteDriver>
//#include <QtDebug>
#include"spreadsheet.h"

using std::vector;

Spreadsheet::Spreadsheet(QWidget *partent)
        :QTableWidget(partent)
{
    autoRecalc=true;
    bridgeBool=false;
    setSelectionMode(ContiguousSelection);

    connect(this,SIGNAL(itemChanged(QTableWidgetItem*)),
            this,SLOT(somethingChanged()));
    clear();
}
void Spreadsheet::clear()
{
        setRowCount(0);
        setColumnCount(0);

        setRowCount(RowCount);
        setColumnCount(ColumnCount);

        for(int i=0;i<ColumnCount;++i)
        {
                QTableWidgetItem *item=new QTableWidgetItem;
                item->setText(QString(QChar('A'+i)));
                setHorizontalHeaderItem(i,item);
        }
        setCurrentCell(0,0);
}

void Spreadsheet::selectCurrentColumn()
{
        selectColumn(currentColumn());
}
void Spreadsheet::selectCurrentRow()
{
        selectRow(currentRow());
}
QTableWidgetSelectionRange Spreadsheet::selectedRange()
{
        QList<QTableWidgetSelectionRange> ranges=selectedRanges();
        if(ranges.isEmpty())
                return QTableWidgetSelectionRange();
        return ranges.first();
}
void Spreadsheet::cut()
{
        copy();
        del();
}
void Spreadsheet::copy()
{
        QTableWidgetSelectionRange range=selectedRange();
        QString string;
        for(int i=0;i<range.rowCount();++i)
        {
                if(i>0)
                    string+="\n";
                for(int j=0;j<range.columnCount();++j)
                {
                    if(j>0)
                        string+="\t";
                    if(item(range.topRow()+i,range.leftColumn()+j))
                        string+=item(range.topRow()+i,range.leftColumn()+j)
                                ->data(Qt::EditRole).toString();
                }
        }
        QApplication::clipboard()->setText(string);
}
void Spreadsheet::del()
{
        QList<QTableWidgetItem*> items=selectedItems();
        if(!items.isEmpty())
        {
                foreach(QTableWidgetItem* item,items)
                        if(item)
                            delete item;
                somethingChanged();
        }
}
void Spreadsheet::paste()
{
        QTableWidgetSelectionRange range=selectedRange();
        QString string=QApplication::clipboard()->text();
        QStringList rows=string.split('\n');
        int rowsCount=rows.count();
        int columnCount=rows.first().count('\t')+1;

        if(range.rowCount()*range.columnCount()!=1
                && (range.rowCount()!=rowsCount
                        ||range.columnCount()!=columnCount))
        {
                QMessageBox::information(this,tr("Spreadsheet"),
                                tr("The information cannot be pasted because the copy"
                                "and pasted ares aren't the same size."));
                return;
        }

        for(int i=0;i<rowsCount;++i)
        {
                QStringList columns=rows[i].split('\t');

                for(int j=0;j<columnCount;++j)
                {
                        int row=range.topRow()+i;
                        int column=range.leftColumn()+j;

                        if(row<RowCount && column<ColumnCount)
                        {
                            if(!item(row,column))
                            {
                                QTableWidgetItem *item=new QTableWidgetItem;
                                setItem(row,column,item);

                            }
                            item(row,column)->setData(Qt::EditRole,columns[j]);
                        }
                }
        }

        somethingChanged();
}

void Spreadsheet::somethingChanged()
{
    update();
    emit modified();
}
QString Spreadsheet::currentLocation()
{
        return QChar('A'+currentColumn())+QString::number(currentRow()+1);
}
bool Spreadsheet::readFile(const QString& fileName)
{
        QFile file(fileName);
        if(!file.open(QIODevice::ReadOnly))
        {
                QMessageBox::warning(this,tr("Spreadsheet"),
                                                        tr("Cannot read file %1:\n%2.")
                                                        .arg(file.fileName())
                                                        .arg(file.errorString()));
                return false;
        }
        QDataStream in(&file);
        in.setVersion(QDataStream::Qt_4_3);

        quint32 magic;
        in>>magic;
        if(magic!=MagicNumber)
        {
                QMessageBox::warning(this,tr("Spreadsheet"),
                                                        tr("The file is not a Spreadsheet file."));
                return false;
        }
        clear();

        quint16 row;
        quint16 column;
        QString string;

        QApplication::setOverrideCursor(Qt::WaitCursor);
        while(!in.atEnd())
        {
                in>>row>>column>>string;
                if(!item(row,column))
                {
                    QTableWidgetItem *newItem=new QTableWidgetItem;
                    setItem(row,column,newItem);
                }
                item(row,column)->setData(Qt::EditRole,string);
        }
        QApplication::restoreOverrideCursor();
        return true;
}
bool Spreadsheet::writeFile(const QString& fileName)
{

        QFile file(fileName);
        if(!file.open(QIODevice::WriteOnly))
        {
                QMessageBox::warning(this,tr("Spreadsheet"),
                                                  tr("Cannot write fil:%1:%2.")
                                                  .arg(fileName).
                                                  arg(file.errorString()));
                return false;
        }

        QDataStream out(&file);
        out.setVersion(QDataStream::Qt_4_3);

        out<<quint32(MagicNumber);
        QApplication::setOverrideCursor(Qt::WaitCursor);
        for(int i=0;i<RowCount;++i)
            for(int j=0;j<ColumnCount;++j)
            {
                QString string;
                if(!item(i,j))
                    string="";
                else
                    string=item(i,j)->data(Qt::EditRole).toString();
                if(!string.isEmpty())
                                out<<quint16(i)<<quint16(j)<<string;
            }
        QApplication::restoreOverrideCursor();
        return true;
}
void Spreadsheet::findNext(const QString& str,Qt::CaseSensitivity cs)
{
        for(int row=currentRow();row<=RowCount;++row)
                for(int column=(row==currentRow()?currentColumn():0)
                        ;column<=ColumnCount;++column)
                        if(text(row,column).contains(str,cs))
                        {
                                clearSelection();
                                setCurrentCell(row,column);
                                activateWindow();
                                return;
                        }

}
void Spreadsheet::findPrevious(const QString& str,Qt::CaseSensitivity cs)
{
        for(int row=currentRow();row>=0;--row)
                for(int column=(row==currentRow()?currentColumn()-1:ColumnCount)
                        ;column>=0;--column)
                        if(text(row,column).contains(str,cs))
                        {
                                clearSelection();
                                setCurrentCell(row,column);
                                activateWindow();
                                return;
                        }
}

QString Spreadsheet::text(int raw,int column)
{
        QTableWidgetItem * c=item(raw,column);
        if(c)
                return c->text();
        else
                return "";

}
void Spreadsheet::makeMap(QString fileName)
{
    QSqlDatabase db=QSqlDatabase::addDatabase("QSQLITE");
    db.setDatabaseName(fileName);
    if(!db.open())
    {
        QMessageBox::critical(this,tr("Database ERROR"),
                              db.lastError().text());
        return;
    }
    QSqlQuery query;
    query.exec("CREATE TABLE A "
               "("
               "x integer , "
               "y integer , "
               "z integer , "
               "RoomName varchar(10)"
               ");");
    query.exec("CREATE TABLE B"
               "("
               "x1 integer ,"
               "x2 integer ,"
               "y1 integer ,"
               "y2 integer ,"
               "z1 integer ,"
               "z2 integer"
               ");");
    bool gap=true;
    int y=1,z=0;

    QApplication::setOverrideCursor(Qt::WaitCursor);
    QString string;
    for(int i=0;i<RowCount;++i)
    {
        if(gap && item(i,0))
        {
            gap=false;
            y=1;
            z++;
        }
        if(!item(i,0))
            gap=true;
        for(int j=0;j<ColumnCount;++j)
        {
            if(item(i,j))
            {
                string=item(i,j)->data(Qt::EditRole).toString();
              /*
                query.prepare("INSERT INTO A"
                              "VALUES"
                              "(:x,:y,:z,:roomName);");
                query.bindValue(":x",2);//j+1
                query.bindValue(":y",2);//y
                query.bindValue(":z",2);//string
                query.bindValue(":roomName",2);//z
                query.exec();
                */
                query.prepare("INSERT INTO A "
                              "VALUES"
                              "(:x,:y,:z,:roomName);");
                query.bindValue(":x",j+1);
                query.bindValue(":y",y);
                query.bindValue(":z",z);
                query.bindValue(":roomName",string);
                query.exec();
                /*
                query.prepare("INSERT INTO A "
                              "VALUES"
                              "(:x,:y,:z,:roomName);");
                query.bindValue(":x",1);
                query.bindValue(":y",1);
                query.bindValue(":z",1);
                query.bindValue(":roomName",1);
                query.exec();

                qDebug()<<j+1<<y<<z<<string;
                */
            }
        }
        y++;
    }
    while(bridges.begin()!=bridges.end())
    {
        int x1,x2,y1,y2,z1,z2;
        QString room1,room2;
        room1=bridges.begin()->room1;
        room2=bridges.begin()->room2;

        query.prepare(" SELECT x,y,z  FROM A "
                      " WHERE RoomName=:room1;");
        query.bindValue(":room1",room1);
        qDebug()<<room1;
        query.exec();
        query.next();
        x1=query.value(0).toInt();
        y1=query.value(1).toInt();
        z1=query.value(2).toInt();
        qDebug()<<x1<<y1<<z1;
        query.prepare(" SELECT x,y,z  FROM A "
                      " WHERE RoomName=:room2;");
        query.bindValue(":room2",room2);
        qDebug()<<room2;
        query.exec();
        query.next();
        x2=query.value(0).toInt();
        y2=query.value(1).toInt();
        z2=query.value(2).toInt();
        qDebug()<<x2<<y2<<z2;

        query.prepare(" INSERT INTO B "
                      " VALUES"
                      "(:x1, :x2 ,:y1, :y2, :z1, :z2);");
        query.bindValue(":x1",x1);
        query.bindValue(":x2",x2);
        query.bindValue(":y1",y1);
        query.bindValue(":y2",y2);
        query.bindValue(":z1",z1);
        query.bindValue(":z2",z2);
        query.exec();
        bridges.erase(bridges.begin());

    }
    QApplication::restoreOverrideCursor();
}
void Spreadsheet::bridge()
{
    if(!bridgeBool)
    {
        room x;
        if(item(selectedRange().topRow(),
                selectedRange().leftColumn()))
            x.room1=item(selectedRange().topRow(),
                         selectedRange().leftColumn())
                    ->data(Qt::EditRole).toString();
        bridges.push_back(x);
        bridgeBool=true;
        qDebug()<<1;
    }
    else
    {
        if(item(selectedRange().topRow(),
             selectedRange().leftColumn()))
         bridges.back().room2=item(selectedRange().topRow(),
                                   selectedRange().leftColumn())
                              ->data(Qt::EditRole).toString();
        bridgeBool=false;
        qDebug()<<2;
    }

}
