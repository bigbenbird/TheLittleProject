//pp
#include<QtGui>
#include "sortDialog.h"

SortDialog::SortDialog(QWidget *parent)
        : QDialog(parent)
{
        primaryGroupBox=new QGroupBox;
        createGroupBox(tr("&Primary Key"),
                &primaryColumnCombo,&primaryOrderCombo,
                primaryGroupBox);

        secondaryGroupBox=new QGroupBox;
        createGroupBox(tr("&Secondary Key"),
                &secondaryColumnCombo,&secondaryOrderCombo,
                secondaryGroupBox);

        tertiaryGroupBox=new QGroupBox;
        createGroupBox(tr("&Tertiary Key"),
                &tertiaryColumnCombo,&tertiaryOrderCombo,
                tertiaryGroupBox);

//	setColumnRange(tertiaryColumnCombo,'A','Z');
        secondaryColumnCombo->addItem(tr("None"));
        tertiaryColumnCombo->addItem(tr("None"));
        primaryColumnCombo->setMinimumSize(
                secondaryColumnCombo->sizeHint());

        secondaryGroupBox->hide();
        tertiaryGroupBox->hide();

        okButton=new QPushButton(tr("&OK"));
        cancelButton=new QPushButton(tr("&Cancel"));
        moreButton=new QPushButton(tr("&More"));

        connect(okButton,SIGNAL(clicked()),
                this,SLOT(accept()));
        connect(cancelButton,SIGNAL(clicked()),
                this,SLOT(reject()));
        connect(moreButton,SIGNAL(clicked()),
                this,SLOT(show_or_hide()));

        QVBoxLayout *rightLayout=new QVBoxLayout;
        rightLayout->addWidget(okButton);
        rightLayout->addWidget(cancelButton);
        rightLayout->addStretch();
        rightLayout->addWidget(moreButton);

        QHBoxLayout *topLayout=new QHBoxLayout;
        topLayout->addWidget(primaryGroupBox);
        topLayout->addLayout(rightLayout);

        QVBoxLayout *mainLayout=new QVBoxLayout;
        mainLayout->addLayout(topLayout);
        mainLayout->addStretch();
        mainLayout->addWidget(secondaryGroupBox);
        mainLayout->addWidget(tertiaryGroupBox);

        setLayout(mainLayout);

        layout()->setSizeConstraint(QLayout::SetFixedSize);
        setWindowTitle(tr("Sort"));
}

void SortDialog::createGroupBox(const QString& name,QComboBox **column,QComboBox **order,QGroupBox* groupBox)
{
        (*column)=new QComboBox;
        //column->addItem(tr("None"));
        QLabel *columnLabel=new QLabel(tr("Column:"));
        columnLabel->setBuddy(*column);

        *order=new QComboBox;
        (*order)->addItem(tr("Ascending"));
        (*order)->addItem(tr("Descending"));
        QLabel *orderLabel=new QLabel(tr("Order:"));
        orderLabel->setBuddy(*order);

        QHBoxLayout *top=new QHBoxLayout;
        top->addWidget(columnLabel);
        top->addWidget(*column);
        top->addStretch();

        QHBoxLayout *down=new QHBoxLayout;
        down->addWidget(orderLabel);
        down->addWidget(*order);
        down->addStretch();

        QVBoxLayout *mainLayout=new QVBoxLayout;
        mainLayout->addLayout(top);
        mainLayout->addLayout(down);

        groupBox->setLayout(mainLayout);

        groupBox->setTitle(name);
}
void SortDialog::show_or_hide()
{
        if(secondaryGroupBox->isHidden())
        {
                secondaryGroupBox->show();
                tertiaryGroupBox->show();
        }
        else
        {
                secondaryGroupBox->hide();
                tertiaryGroupBox->hide();
        }
}
void SortDialog::setColumnRange(QComboBox *comboBox,QChar first,QChar last)
{
        for(QChar ch=first;ch<=last;ch=ch.unicode()+1)
                comboBox->addItem(QString(ch));
}
void SortDialog::setColumnRange(QChar first, QChar last)
{
        setColumnRange(primaryColumnCombo,first,last);
        setColumnRange(secondaryColumnCombo,first,last);
        setColumnRange(tertiaryColumnCombo,first,last);
}



