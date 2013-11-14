//pp

#ifndef FINDDIALOG_H
#define FINDDIALOG_H

#include<QDialog>

class QCheckBox;
class QPushButton;
class QLabel;
class QLineEdit;

class FindDialog:public QDialog
{
        Q_OBJECT

public:
        FindDialog(QWidget *parent=0);

signals:
        void findNext(const QString &str,Qt::CaseSensitivity cs);
        void findPrevious(const QString &str,Qt::CaseSensitivity cs);

private slots:
        void findClicked();
        void enableFindButton(const QString &str);

private:
        QPushButton *findButton;
        QPushButton *closeButton;
        QLineEdit *lineEdit;
        QLabel *label;
        QCheckBox *caseCheckBox;
        QCheckBox *backwardCheckBox;

};
#endif
