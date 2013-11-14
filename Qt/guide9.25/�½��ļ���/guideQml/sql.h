#ifndef SQL_H
#define SQL_H
#include<QSqlDatabase>
//class QSqlDatabase;

class sql
{
public:
    sql(const QString);
   // QString show(int*,int*,int*,int);
    QString findPath(const QString&,const QString&);
    ~sql(){db.close();}
private:
    bool connectDatabase(const QString&);
    QString differentFind(int *x,int *y,int *z);
    QString way(int* ,int*,int*,int);//{QStringList a;return a;}
    void getValue(QSqlQuery&,int*,int*,int*);

    QSqlDatabase db;
    QStringList roomList;
    int sameFind(int* ,int*,int*);

};
#endif // SQL_H
