#include<QSqlDatabase>
#include<QSqlQuery>
#include<QtGui>
#include<QSqlError>
#include<cmath>
#include<QDebug>


#include"sql.h"


void swap(int*);

sql::sql()
{
}
bool sql::connectDatabase(const QString& fileName)
{
    db=QSqlDatabase::addDatabase("QSQLITE");
    QString string=QDir::toNativeSeparators(fileName);
    qDebug()<<"fileName:"<<fileName<<string<< QDir::currentPath();
    QFile file(fileName);
    if(!file.open(QIODevice::ReadOnly))
        qDebug()<<"the path is wrong";
    else
        qDebug()<<QDir().absoluteFilePath(fileName);
    db.setDatabaseName(string);
    if(!db.open())
    {
        qDebug()<<db.lastError().text();
        qDebug()<<QDir().absoluteFilePath(fileName);
        QMessageBox::critical(0,QObject::tr("Database Error"),
                              db.lastError().text());
        qDebug()<<"Error";
        return false;
    }
    return true;
}
QString sql::findPath(const QString& start,const QString& end)
{
    qDebug()<<"findPath";
    QSqlQuery query;

    query.prepare("SELECT x,y,z FROM A "
                  "WHERE RoomName=:start OR RoomName=:end;");
    query.bindValue(":start",start);
    query.bindValue(":end",end);
    query.exec();
    int x[2],y[2],z[2];
    int i=-1;
    while(query.next())
    {
        i++;
        x[i]=query.value(0).toInt();
        y[i]=query.value(1).toInt();
        z[i]=query.value(2).toInt();
        if(i==2)
        {
            QMessageBox::critical(0,QObject::tr("Database Error"),
                                  QObject::tr("the map has two same number room."));
            return QString();
        }
        qDebug()<<"room"<<x[i]<<y[i]<<z[i];
    }
    if(i==0)
    {
        QMessageBox::critical(0,QObject::tr("Database Error"),
                              db.lastError().text());
        QMessageBox::critical(0,QObject::tr("Database Error"),
                              QObject::tr("there is no path between these room"));
        return QString();
    }
    if(z[0]==z[1])
    {
        int xs;
        xs=sameFind(x,y,z);
        if(xs==0) return QString();
        return way(x,y,z,xs);
    }
    //int *result;
    return differentFind(x,y,z);
//    QStringList stringList;
//    stringList=way(x,y,z,);
}//noneDone
int sql::sameFind(int* x,int *y,int *z)
{
    if(y[0]==y[1])
        return (x[0]+x[1])/2;
    QSqlQuery query;
    query.prepare("SELECT x FROM A "
                 "WHERE y=:y1 AND (x-:x1)*(x-:x2)<0 AND z=:z1 AND RoomName='stair';");
    query.bindValue(":y1",y[0]);
    query.bindValue(":x1",x[0]);
    query.bindValue(":x2",x[1]);
    query.bindValue(":z1",z[0]);
    query.exec();
    if(query.next())
    {
   //     qDebug()<<query.value(0).toInt();
        return query.value(0).toInt();
    }
    query.prepare("SELECT x FROM A "
                  "WHERE y=:y1 AND z=:z1 AND RoomName='stair';");
    query.bindValue(":y1",y[0]);
    query.bindValue(":z1",z[0]);
    query.exec();
    int min=1000;                                              //a large enough number
    int m,r;
    while(query.next())
    {
        m=query.value(0).toInt();
    //    qDebug()<<m;
        if((abs(m-x[0])+abs(m-x[1]))< min)
        {
            r=m;
            min=abs((m-x[0])+(m-x[1]));
        }
    }
    if(min==1000)
    {
        QMessageBox::critical(0,QObject::tr("Database Error"),
                              QObject::tr("there is no path between these room"));
        return 0;

    }
    return r;
}
namespace Value
{
    int px[2],py[2],pz[2],qx[2],qy[2],qz[2];
    int dx[2],dy[2],dz[2];
    int m1,m2;
}
QString sql::differentFind(int *x,int *y,int *z)
{

    using namespace Value;
    qDebug()<<"differentFind";
    QSqlQuery query;

    query.prepare("SELECT x1,x2,y1,y2,z1,z2 FROM B "
                  "WHERE ((z1=:z1 AND z2=:z2) OR (z1=:z2 AND z2=:z1)) "
                  "AND (x1-:x11)*(x1-:x21)<0 AND (x2-:x12)*(x2-:x22)<0  "
                  "AND (y1-:y11)*(y1-:y22)<0 AND (y2-:y22)*(y2-:y11)<0 ;");
    query.bindValue(":z11",z[0]);
    query.bindValue(":z12",z[0]);
    query.bindValue(":z21",z[1]);
    query.bindValue(":z22",z[1]);
    query.bindValue(":x11",x[0]);
    query.bindValue(":x12",x[0]);
    query.bindValue(":x21",x[1]);
    query.bindValue(":x22",x[1]);
    query.bindValue(":y11",y[0]);
    query.bindValue(":y12",y[0]);
    query.bindValue(":y21",y[1]);
    query.bindValue(":y22",y[1]);
    query.exec();

    if(query.next())
    {
        qDebug()<<"differentFind 1:";
        getValue(query,x,y,z);
        QString string;
        string+=way(px,py,pz,m1);
        string+="opposite\t";
        string+=way(qx,qy,qz,m2);
        return string;
    }
    qDebug()<<"the z"<<z[0]<<z[1];
    qDebug()<<"differentFind 2:";
    query.prepare("SELECT x1,x2,y1,y2,z1,z2 FROM B WHERE ((z1=:a AND z2=:b ) OR (z1=:c AND z2=:d )) ;");
   // qDebug()<<query(":a").toInt()<<query.boundValue(":b").toInt();
    query.bindValue(":a",z[0]);
   // qDebug()<<query.boundValue(":a").toInt()<<query.boundValue(":b").toInt();
    query.bindValue(":b",z[1]);
   // qDebug()<<query.boundValue(":a").toInt()<<query.boundValue(":b").toInt();   
    query.bindValue(":c",z[1]);
    query.bindValue(":d",z[2]);
    query.exec();
   // qDebug()<<query.executedQuery();
   // qDebug()<<"the Value"<<query.boundValue(":a").toInt()<<query.boundValue(":b").toInt();
    int m1min,m2min,min=1000;
   // qDebug()<<query.executedQuery();
    while(query.next())
    {
      //  qDebug()<<"differentFind 3:";
        getValue(query,x,y,z);
     //   qDebug()<<"1:"<<m1<<px[0]<<qx[0]<<px[1]<<qx[1];
        if(abs(m1-px[0])+abs(m1-px[1])+abs(m2-qx[0])+abs(m2-qx[1])<min)
        {
            m1min=m1;
            m2min=m2;
            min=abs(m1-px[0])+abs(m1-px[1])+abs(m2-qx[0])+abs(m2-qx[1]);
        }
    }
    qDebug()<<"min "<<m1min<<m2min;
    if(min==1000)
        return QString();
    QString str=way(px,py,pz,m1);
    str+="opposite\t";
    str+=way(qx,qy,qz,m2);
    qDebug()<<"the map string"<<str;
    return str;
}
void swap(int* arr)
{
    int x;
    x=arr[0];
    arr[0]=arr[1];
    arr[1]=x;
}
void sql::getValue(QSqlQuery& query,int *x,int *y,int *z)
{
    using namespace Value;
    dx[0]=query.value(0).toInt();
    dx[1]=query.value(1).toInt();
    dy[0]=query.value(2).toInt();
    dy[1]=query.value(3).toInt();
    dz[0]=query.value(4).toInt();
    dz[1]=query.value(5).toInt();

    if(dz[0]!=z[0])
    {
        swap(x);
        swap(y);
        swap(z);
    }

    px[0]=x[0];
    px[1]=dx[0];
    py[0]=y[0];
    py[1]=dy[0];
    pz[0]=z[0];
    pz[1]=z[0];
    qx[0]=x[1];
    qx[1]=dx[1];
    qy[0]=y[1];
    qy[1]=dy[1];
    qz[0]=z[1];
    qz[1]=z[1];
    m1=sameFind(px,py,pz);
    m2=sameFind(qx,qy,qz);
}
QString sql::way(int* x,int* y,int* z,int xy)
{
    int xMin,xMax,yMin,yMax;

    xMin=x[0]>x[1]?x[1]:x[0];
    xMax=x[0]>x[1]?x[0]:x[1];
    xMin=xMin>xy?xy:xMin;
    xMax=xMax>xy?xMax:xy;

    yMin=y[0]>y[1]?y[1]:y[0];
    yMax=y[0]>y[1]?y[0]:y[1];

    QSqlQuery query;
    query.prepare("SELECT RoomName FROM A "
                  " WHERE x>=:xMin AND x<=:xMax "
                  " AND y>=:yMin AND y<=:yMax "
                  " AND z=:z;");
    query.bindValue(":xMin",xMin);
    query.bindValue(":xMax",xMax);
    query.bindValue(":yMin",yMin);
    query.bindValue(":yMax",yMax);
    query.bindValue(":z",z[0]);
    query.exec();

    QString str;

    int i=0;
    while(query.next())
    {
        str+=query.value(0).toString();
  //      if(yMin==yMax && )
  //          str+='\n';
        if(!((++i)%(xMax-xMin+1)))
            str+='\n';
        else
            str+='\t';
    }
    qDebug()<<"i"<<i;
    if(str.isEmpty())
        return QString();
    QString s;
 //   qDebug()<<x[0]<<x[1]<<xy;
    s+="Message";
    if(yMin==y[0])
    {
        s+=char(x[0]);
        s+=char(x[1]);
    }
    else
    {
        s+=char(x[1]);
        s+=char(x[0]);
    }
    s+=char(xy);
    str+=s;
    str+='\n';
    qDebug()<<s;
    qDebug()<<int(s.toStdString()[0])<<int(s.toStdString()[1])<<int(s.toStdString()[2]);
    return str;
}
