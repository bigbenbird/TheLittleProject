
//pp
#include <QtGui>
#include "gotocell.h"

GoToCell::GoToCell(QWidget *parent)
        : QDialog(parent)
{
        label=new QLabel(tr("&Cell Location"));
        lineEdit=new QLineEdit();
        label->setBuddy(lineEdit);

        QRegExp regExp("[A-Za-z][1-9][0-9]{0,2}");
        lineEdit->setValidator(new QRegExpValidator(regExp,this));

        okButton=new QPushButton(tr("OK"));
        okButton->setDefault(true);
        okButton->setEnabled(false);

        cancelButton=new QPushButton(tr("Cancel"));

        connect(lineEdit,SIGNAL(textChanged(const QString&)),
                this,SLOT(enableOkButton()));
        connect(okButton,SIGNAL(clicked()),
                this,SLOT(accept()));
        connect(cancelButton,SIGNAL(clicked()),
                this,SLOT(reject()));

        QHBoxLayout *topLayout=new QHBoxLayout;
        topLayout->addWidget(label);
        topLayout->addStretch();
        topLayout->addWidget(lineEdit);

        QHBoxLayout *downLayout=new QHBoxLayout;
        downLayout->addStretch();
        downLayout->addWidget(okButton);
        downLayout->addWidget(cancelButton);

        QVBoxLayout *mainLayout=new QVBoxLayout;
        mainLayout->addLayout(topLayout);
        mainLayout->addLayout(downLayout);

        setLayout(mainLayout);

        setWindowTitle(tr("Go To Cell"));
        setFixedHeight(sizeHint().height());
}

void GoToCell::enableOkButton()
{
        okButton->setEnabled(lineEdit->hasAcceptableInput());
}
