//pp
#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>

class Spreadsheet;
class QLabel;
class FindDialog;
class QAction;

class MainWindow : public QMainWindow
{
        Q_OBJECT

protected:
        void closeEvent(QCloseEvent *event);

public:
        MainWindow();
        bool okToContinue();
        bool loadFile(const QString&);
        bool saveFile(const QString&);
        QString strippedName(const QString& fullFileName);
//	void updataRecentFileActions();

private slots:
        void makeMap();
        void updataStatusBar();
        void spreadsheetModified();
        void newFile();
        void openFile();
        bool save();
        bool saveAs();
        void openRecentFile();
        void find();
        void goToCell();
        void about();
//	void closeFile();
//	void setCurrentFile(QString&);
        void updataRecentFileActions();

private:
        void writeSettings();
        void readSettings();
        void createAction(QAction**,const QString&,const QString&,
                          const QKeySequence::StandardKey& shortcut=QKeySequence::UnknownKey,
                          const QString& icon=0);
//	void createConnect(const QAction **action,const QWidget*);
        void createActions();
        void createMenus();
        void createContextMenu();
        void createToolBar();
        void createStatusBar();
        void setCurrentFile(const QString& fileName);

        Spreadsheet *spreadsheet;

        FindDialog *findDialog;
        QString curFile;
        QStringList recentFileList;

        QAction *separatorAction;

        static const int MaxRecentFiles=5;
        QAction *recentFileActions[MaxRecentFiles];

        QLabel *locationLabel;
        QLabel *formulaLabel;

        QToolBar *fileToolBar;
        QToolBar *editToolBar;

        QMenu *fileMenu;
        QMenu *editMenu;
        QMenu *makeMenu;
        QMenu *optionMenu;
        QMenu *helpMenu;
        QMenu *selectSubMenu;

        QAction *newAction;
        QAction *openAction;
        QAction *saveAction;
        QAction *saveAsAction;
        //QAction *closeAction;
        QAction *exitAction;
        QAction *cutAction;
        QAction *copyAction;
        QAction *pasteAction;
        QAction *deleteAction;
        QAction *selectColumnAction;
        QAction *selectRowAction;
        QAction *selectAllAction;
        QAction *findAction;
        QAction *goToCellAction;
        QAction *showGridAction;
        //QAction *autoRecalculateAction;
        QAction *autoRecalcAction;
        QAction *aboutAction;
        QAction *aboutQtAction;
        QAction *makeMapAction;
        QAction *createBridgeAction;

};

#endif // MAINWINDOW_H
