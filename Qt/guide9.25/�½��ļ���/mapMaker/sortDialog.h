//pp
#ifndef SORTDIALOG_H
#define SORTDIALOG_H

#include <QDialog>

class QPushButton;
class QGroupBox;
class QComboBox;

class SortDialog : public QDialog
{
        Q_OBJECT

public:
        SortDialog(QWidget *parent = 0);
        void setColumnRange(QComboBox*,QChar first,QChar lase);
        void setColumnRange(QChar first,QChar lase);

        QComboBox *primaryColumnCombo;
        QComboBox *secondaryColumnCombo;
        QComboBox *tertiaryColumnCombo;

        QComboBox *primaryOrderCombo;
        QComboBox *secondaryOrderCombo;
        QComboBox *tertiaryOrderCombo;

private slots:
        void show_or_hide();

private:
        void createGroupBox(const QString&, QComboBox**, QComboBox**, QGroupBox*);

        QGroupBox *primaryGroupBox;
        QGroupBox *secondaryGroupBox;
        QGroupBox *tertiaryGroupBox;

        QPushButton *okButton;
        QPushButton *cancelButton;
        QPushButton *moreButton;

};

#endif // SORTDIALOG_H
