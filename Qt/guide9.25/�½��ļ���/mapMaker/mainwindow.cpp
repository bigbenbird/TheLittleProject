//pp
#include<QtGui>

#include "mainwindow.h"
#include "finddialog.h"
#include "spreadsheet.h"
#include "gotocell.h"


MainWindow::MainWindow()
//MainWindow::MainWindow()
{
        spreadsheet=new Spreadsheet;
        setCentralWidget(spreadsheet);

        createActions();
        createMenus();
        createContextMenu();
        createToolBar();
        createStatusBar();

        readSettings();

        findDialog=0;

        //setAtttibute(Qt::WA_DeleteOnClose);
        setWindowIcon(QIcon(":image/images/icon.png"));
       // newAction->setIcon(QIcon(":/File/images/new.png"));

}

void MainWindow::createActions()
{
        createAction(&newAction,tr("&New"),
                     tr("Create a new spreadsheet file"),
                     QKeySequence::New,":image/images/new.png");
        connect(newAction,SIGNAL(triggered()),
                this,SLOT(newFile()));

        createAction(&openAction,tr("&Open"),
                     tr("Open a existing spreadsheet file"),
                     QKeySequence::Open,":image/images/open.png");
        connect(openAction,SIGNAL(triggered()),
                this,SLOT(openFile()));

        createAction(&saveAction,tr("&Save"),
                     tr("Save the spreadsheet to disk"),
                     QKeySequence::Save,":/image/images/save.png");
        connect(saveAction,SIGNAL(triggered()),
                this,SLOT(save()));

        createAction(&saveAsAction,tr("Save &As"),
                     tr("Save the spreadsheet under a new name"));
        connect(saveAsAction,SIGNAL(triggered()),
                this,SLOT(saveAs()));

        for(int i=0;i<MaxRecentFiles;i++)
        {
                recentFileActions[i]=new QAction(this);
                recentFileActions[i]->setVisible(false);
                connect(recentFileActions[i],SIGNAL(triggered()),
                                this,SLOT(openRecentFile()));
        }

        createAction(&exitAction,tr("E&xit"),
                     tr("Exit the application"));
        connect(exitAction,SIGNAL(triggered()),
                this,SLOT(close()));

        createAction(&cutAction,tr("Cu&t"),
                     tr("Cut the current selection's contents to the clipboard"),
                     QKeySequence::Cut,":/image/images/cut.png");
        connect(cutAction,SIGNAL(triggered()),
                spreadsheet,SLOT(cut()));

        createAction(&copyAction,tr("&Copy"),
                     tr("Copy the current selection's contents to the clipboard"),
                     QKeySequence::Copy,":/image/images/copy.png");
        connect(copyAction,SIGNAL(triggered()),
                spreadsheet,SLOT(copy()));

        createAction(&pasteAction,tr("&Paste"),
                     tr("Paste the clipboard's contents into the current selection"),
                     QKeySequence::Paste,":/image/images/paste.png");
        connect(pasteAction,SIGNAL(triggered()),
                spreadsheet,SLOT(paste()));

        createAction(&deleteAction,tr("&Delete"),
                     tr("Delete the current selection's contents"),
                     QKeySequence::Delete);
        connect(deleteAction,SIGNAL(triggered()),
                spreadsheet,SLOT(del()));

        createAction(&selectRowAction,tr("&Row"),
                     tr("Select all the cells in the current row"));
        connect(selectRowAction,SIGNAL(triggered()),
                spreadsheet,SLOT(selectCurrentRow()));

        createAction(&selectColumnAction,tr("&Column"),
                     tr("Select all the cells int the current column"));
        connect(selectColumnAction,SIGNAL(triggered()),
                spreadsheet,SLOT(selectCurrentColumn()));

        createAction(&selectAllAction,tr("&All"),
                     tr("Select all the cells int the spreadsheet"));
        connect(selectAllAction,SIGNAL(triggered()),
                spreadsheet,SLOT(selectAll()));

        createAction(&findAction,tr("&Find...."),
                     tr("Find a matching cell"),
                     QKeySequence::Find,":/image/images/find.png");
        connect(findAction,SIGNAL(triggered()),
                this,SLOT(find()));

        createAction(&goToCellAction,tr("&Go To Cell...."),
                     tr("Go to the specified cell"),
                     QKeySequence::UnknownKey,":/image/images/gotocell.png");
        connect(goToCellAction,SIGNAL(triggered()),
                this,SLOT(goToCell()));


        createAction(&showGridAction,tr("&Show Grip"),
                     tr("Show or hide the spreadsheet's grip"));
        connect(showGridAction,SIGNAL(toggled(bool)),
                spreadsheet,SLOT(setShowGrid(bool)));

        createAction(&autoRecalcAction,tr("&Auto-Recalculate"),
                     tr("Switch auto-recalculation on or off"));
        connect(autoRecalcAction,SIGNAL(toggled(bool)),
                spreadsheet,SLOT(setAutoRecalculate(bool)));

        createAction(&aboutAction,tr("&About"),
                     tr("Show the application's About box"));
        connect(aboutAction,SIGNAL(triggered(bool)),
                this,SLOT(about()));

        createAction(&aboutQtAction,tr("About &Qt"),
                     tr("Show the Qt library's About box"));
        connect(aboutQtAction,SIGNAL(triggered(bool)),
                qApp,SLOT(aboutQt()));

        createAction(&makeMapAction,tr("Make Map"),
                     tr("Make the database of this map"),
                     QKeySequence::UnknownKey,
                     tr(":/image/images/make.png"));
        connect(makeMapAction,SIGNAL(triggered(bool)),
                this,SLOT(makeMap()));

        createAction(&createBridgeAction,tr("Connect"),
                     tr("connect two map"),
                     QKeySequence::UnknownKey,
                     tr(":/image/images/connect.png"));
        connect(createBridgeAction,SIGNAL(triggered(bool)),
                spreadsheet,SLOT(bridge()));

}

void MainWindow::createAction(QAction **action,const QString& name,
                              const QString& statusTip,
                              const QKeySequence::StandardKey& shortcut,
                              const QString& icon)
{
        (*action)=new QAction(name,this);
        //if(icon==0)
        (*action)->setIcon(QIcon(icon));
//	if(shortcut==QKeySequence::UnknownKey)
                (*action)->setShortcut(shortcut);
        (*action)->setStatusTip(statusTip);
}

void MainWindow::createMenus()
{
        fileMenu=menuBar()->addMenu(tr("&File"));
        fileMenu->addAction(newAction);
        fileMenu->addAction(openAction);
        fileMenu->addAction(saveAction);
        fileMenu->addAction(saveAsAction);
        fileMenu->addSeparator();
        for(int i=0;i<MaxRecentFiles;i++)
                fileMenu->addAction(recentFileActions[i]);
        separatorAction=fileMenu->addSeparator();
//	fileMenu->addAction(closeAction);
        fileMenu->addAction(exitAction);

        editMenu=menuBar()->addMenu(tr("&Edit"));
        editMenu->addAction(cutAction);
        editMenu->addAction(copyAction);
        editMenu->addAction(pasteAction);
        editMenu->addAction(deleteAction);

        selectSubMenu=editMenu->addMenu(tr("&Select"));
        selectSubMenu->addAction(selectRowAction);
        selectSubMenu->addAction(selectColumnAction);
        selectSubMenu->addAction(selectAllAction);

        editMenu->addSeparator();
        editMenu->addAction(findAction);
        editMenu->addAction(goToCellAction);

        optionMenu=menuBar()->addMenu(tr("&Options"));
        optionMenu->addAction(showGridAction);
        optionMenu->addAction(autoRecalcAction);

        makeMenu=menuBar()->addMenu(tr("&Make"));
        makeMenu->addAction(createBridgeAction);
        makeMenu->addAction(makeMapAction);

        helpMenu=menuBar()->addMenu(tr("&Help"));
        helpMenu->addAction(aboutAction);
        helpMenu->addAction(aboutQtAction);

}
void MainWindow::createToolBar()
{
        fileToolBar=addToolBar(tr("&File"));
        fileToolBar->addAction(newAction);
        fileToolBar->addAction(openAction);
        fileToolBar->addAction(saveAction);

        editToolBar=addToolBar(tr("&Edit"));
        editToolBar->addAction(cutAction);
        editToolBar->addAction(copyAction);
        editToolBar->addAction(pasteAction);
        editToolBar->addSeparator();
        editToolBar->addAction(findAction);
        editToolBar->addAction(goToCellAction);
        editToolBar->addSeparator();
        editToolBar->addAction(makeMapAction);
        editToolBar->addAction(createBridgeAction);

}
void MainWindow::createContextMenu()
{
        spreadsheet->addAction(cutAction);
        spreadsheet->addAction(copyAction);
        spreadsheet->addAction(pasteAction);
        spreadsheet->setContextMenuPolicy(Qt::ActionsContextMenu);
}
void MainWindow::createStatusBar()
{
        locationLabel=new QLabel(tr(" W999 "));
        locationLabel->setAlignment(Qt::AlignHCenter);              //设置对齐方式:水平方向上 居中对齐
        locationLabel->setMinimumSize(locationLabel->sizeHint());

        formulaLabel=new QLabel();
        formulaLabel->setIndent(3);

        statusBar()->addWidget(locationLabel);
        statusBar()->addWidget(formulaLabel,1);

        connect(spreadsheet,SIGNAL(currentCellChanged(int,int,int,int)),
                this,SLOT(updataStatusBar()));
        connect(spreadsheet,SIGNAL(modified()),
                this,SLOT(spreadsheetModified()));

        updataStatusBar();

}
void MainWindow::updataStatusBar()
{
        formulaLabel->setText(spreadsheet->currentLocation());
}
void MainWindow::spreadsheetModified()
{
        setWindowModified(true);
        updataStatusBar();
}
void  MainWindow::newFile()
{
        if (okToContinue()) {
        spreadsheet->clear();
        setCurrentFile("");
    }
}
void MainWindow::openFile()
{
        if(okToContinue())
        {
                QString fileName=QFileDialog::getOpenFileName(
                        this,tr("Open a spreasheet"),".",
                        tr("Spreasheet files (*.sp)"));
                if(!fileName.isEmpty())
                        loadFile(fileName);
        }

}
bool MainWindow::okToContinue()
{
        if(isWindowModified())
        {
                int r=QMessageBox::warning(this,tr("spreadsheet"),
                        tr("The spreadsheet has been modified\n"
                        "Do you want to save your change?"),
                        QMessageBox::Yes|QMessageBox::No|
                        QMessageBox::Cancel);
                if(r==QMessageBox::Yes)
                        return save();
                else if(r==QMessageBox::Cancel)
                        return false;
        }
        return true;
}
bool MainWindow::loadFile(const QString& fileName)
{
        if(!spreadsheet->readFile(fileName))
        {
                statusBar()->showMessage(tr("Loading canceled"),2000);
                return false;
        }
        setCurrentFile(fileName);
        statusBar()->showMessage(tr("File loaded"),2000);
        return true;
}
bool MainWindow::save()
{
        if(curFile.isEmpty())
                return saveAs();
        else
                return saveFile(curFile);
}
bool MainWindow::saveAs()
{
        QString fileName=
                QFileDialog::getSaveFileName(this,
                tr("Save Spreadsheet"),".",
                tr("Spreadsheet File (*.sp)"));
        if(fileName.isEmpty())
                return false;
        return saveFile(fileName);
}
bool MainWindow::saveFile(const QString& fileName)
{
        if(!spreadsheet->writeFile(fileName))
        {
                statusBar()->showMessage(tr("Saving canceled"),2000);
                return false;
        }
        setCurrentFile(fileName);
        statusBar()->showMessage(tr("File saved"),2000);
        return true;
}
void MainWindow::closeEvent(QCloseEvent *event)
{
        if(okToContinue())
        {
                writeSettings();
                event->accept();
        }
        else event->ignore();
}
void MainWindow::setCurrentFile(const QString& fileName)
{
        curFile=fileName;
        setWindowModified(false);

        QString showName=tr("Untitled");
        if(!curFile.isEmpty())
        {
                showName=strippedName(curFile);
                recentFileList.removeAll(curFile);
                recentFileList.prepend(curFile);
                updataRecentFileActions();
        }
        setWindowTitle(tr("%1[*] - &2").arg(showName).
                arg(tr("Spreadsheet")));
}
QString MainWindow::strippedName(const QString& fullFileName)
{
        return QFileInfo(fullFileName).fileName();
}
void MainWindow::updataRecentFileActions()
{
        QMutableStringListIterator i(recentFileList);
        while(i.hasNext())
                if(!QFile::exists(i.next()))
                        i.remove();
        for(int j=0;j<MaxRecentFiles;j++)
                if(j<recentFileList.count())
                {
                        QString text=tr("&%1 %2").arg(j+1)
                                .arg(strippedName(recentFileList[j]));
                        recentFileActions[j]->setText(text);
                        recentFileActions[j]->setData(recentFileList[j]);
                        recentFileActions[j]->setVisible(true);
                }
                else
                        recentFileActions[j]->setVisible(false);
        separatorAction->setVisible(!recentFileList.isEmpty());
}
void MainWindow::openRecentFile()
{
        if(okToContinue())
        {
                QAction *action=qobject_cast<QAction*>(sender());
                if(action)
                        loadFile(action->data().toString());
        }
}

void MainWindow::find()
{
        if(!findDialog)
        {
                findDialog=new FindDialog;
                connect(findDialog,SIGNAL(findNext(const QString&,Qt::CaseSensitivity)),
                        spreadsheet,SLOT(findNext(const QString&,Qt::CaseSensitivity)));
                connect(findDialog,SIGNAL(findPrevious(const QString&,Qt::CaseSensitivity)),
                        spreadsheet,SLOT(findPrevious(const QString&,Qt::CaseSensitivity)));
        }
        findDialog->show();
        findDialog->raise();
        findDialog->activateWindow();
}
void MainWindow::goToCell()
{
        GoToCell dialog(this);
        if(dialog.exec())
        {
                QString str=dialog.lineEdit->text().toUpper();
                spreadsheet->setCurrentCell(str.mid(1).toInt()-1,
                                                                        str[0].unicode()-'A');
        }
}

void MainWindow::about()
{
          QMessageBox::about(this, tr("About Spreadsheet"),
            tr("<h2>Spreadsheet 1.1</h2>"
               "<p>Copyright &copy; 2008 Software Inc."
               "<p>Spreadsheet is a small application that "
               "demonstrates QAction, QMainWindow, QMenuBar, "
               "QStatusBar, QTableWidget, QToolBar, and many other "
               "Qt classes."));
}
void MainWindow::writeSettings()
{
        QSettings settings ("Software Inc","Spreadsheet");
        settings.setValue("geometry",saveGeometry());
        settings.setValue("recentFileList",recentFileList);
        settings.setValue("showGrid",showGridAction->isChecked());
        settings.setValue("autoRecalculate",autoRecalcAction->isChecked());
}
void MainWindow::readSettings()
{
        QSettings settings ("Software Inc","Spreadsheet");

        restoreGeometry(settings.value("geometry").toByteArray());

        recentFileList=settings.value("recentFileList").toStringList();
        updataRecentFileActions();

        bool grid=settings.value("showGrid").toBool();
        showGridAction->setChecked(grid);

        bool recalculate=settings.value("autoRecalculate").toBool();
        autoRecalcAction->setChecked(recalculate);

}
void MainWindow::makeMap()
{
    QString fileName=
            QFileDialog::getSaveFileName(this,
                                         tr("Make Map"),".",
                                         tr("Map File (*.db)"));
    if(fileName.isEmpty())
            return;
    spreadsheet->makeMap(fileName);
}
