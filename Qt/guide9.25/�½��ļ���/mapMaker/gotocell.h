//pp
#ifndef GOTOCELL_H
#define GOTOCELL_H

#include <QDialog>
class QLabel;
class QLineEdit;
class QPushButton;


class GoToCell : public QDialog
{
        Q_OBJECT

public:
        GoToCell(QWidget *parent = 0);
        QLineEdit *lineEdit;
//	~goToCell();
private slots:

        void enableOkButton();

private:
        QPushButton *okButton;
        QPushButton *cancelButton;
        QLabel *label;
};

#endif // GOTOCELL_H
