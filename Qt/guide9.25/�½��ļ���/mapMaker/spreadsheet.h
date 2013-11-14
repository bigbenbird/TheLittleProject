//pp
#ifndef SPREADSHEET_H
#define SPREADSHEET_H

#include<QTableWidget>
#include<vector>
using std::vector;

struct room
{
    QString room1;
    QString room2;
};
class Spreadsheet:public QTableWidget
{
        Q_OBJECT

public slots:
        void cut();
        void del();
        void copy();
        void paste();
//	void setShowGrip(bool);
        void selectCurrentColumn();
        void selectCurrentRow();
        void findNext(const QString&,Qt::CaseSensitivity);
        void findPrevious(const QString&,Qt::CaseSensitivity);
        void makeMap(QString fileName);
        void bridge();
//	void setshowGrip(bool);

private slots:
        void somethingChanged();

public:
        Spreadsheet(QWidget* partent=0);
        QString currentLocation();
        bool readFile(const QString&);
        bool writeFile(const QString&);
        QTableWidgetSelectionRange selectedRange();
        QString text(int,int);
        void clear();
        bool autoRecalculate(){return autoRecalc;}
signals:
    void modified();

private:
        enum { MagicNumber = 0x7F51C883, RowCount = 999, ColumnCount = 26 };
        bool bridgeBool;
        bool autoRecalc;
        vector<room> bridges;

};


#endif




