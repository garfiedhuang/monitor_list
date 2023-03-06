; 该脚本使用 HM VNISEdit 脚本编辑器向导产生

; 安装程序初始定义常量
!define PRODUCT_NAME "Monitor List"
!define PRODUCT_VERSION "1.0.0.0"
!define PRODUCT_PUBLISHER "GuangZhou Garfield Huang Co., Ltd"
!define PRODUCT_WEB_SITE ""
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\Monitor.List.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_ICON "D:\Work\Demo\MonitorList\monitor_list\monitor.list\Assets\Images\icon.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!define MUI_FINISHPAGE_RUN "$INSTDIR\Monitor.List.exe"
!insertmacro MUI_PAGE_FINISH

; 安装卸载过程页面
!insertmacro MUI_UNPAGE_INSTFILES

; 安装界面包含的语言设置
!insertmacro MUI_LANGUAGE "SimpChinese"

; 安装预释放文件
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI 现代界面定义结束 ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Setup.exe"
InstallDir "$PROGRAMFILES\Monitor List"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUnInstDetails show
BrandingText " "

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File "..\monitor.list\bin\Release\Monitor.List.pdb"
  CreateDirectory "$SMPROGRAMS\Monitor List"
  CreateShortCut "$SMPROGRAMS\Monitor List\Monitor List.lnk" "$INSTDIR\Monitor.List.exe"
  CreateShortCut "$DESKTOP\Monitor List.lnk" "$INSTDIR\Monitor.List.exe"
  File "..\monitor.list\bin\Release\Monitor.List.exe.config"
  File "..\monitor.list\bin\Release\Monitor.List.exe"
  File "..\monitor.list\bin\Release\WpfMultiStyle.dll"
  File "..\monitor.list\bin\Release\System.Windows.Interactivity.dll"
  File "..\monitor.list\bin\Release\System.ValueTuple.xml"
  File "..\monitor.list\bin\Release\System.ValueTuple.dll"
  File "..\monitor.list\bin\Release\NLog.xml"
  File "..\monitor.list\bin\Release\NLog.dll"
  File "..\monitor.list\bin\Release\NLog.config"
  File "..\monitor.list\bin\Release\Newtonsoft.Json.xml"
  File "..\monitor.list\bin\Release\Newtonsoft.Json.dll"
  File "..\monitor.list\bin\Release\HandyControl.xml"
  File "..\monitor.list\bin\Release\HandyControl.dll"
  File "..\monitor.list\bin\Release\GalaSoft.MvvmLight.xml"
  File "..\monitor.list\bin\Release\GalaSoft.MvvmLight.Platform.xml"
  File "..\monitor.list\bin\Release\GalaSoft.MvvmLight.Platform.pdb"
  File "..\monitor.list\bin\Release\GalaSoft.MvvmLight.Platform.dll"
  File "..\monitor.list\bin\Release\GalaSoft.MvvmLight.pdb"
  File "..\monitor.list\bin\Release\GalaSoft.MvvmLight.Extras.xml"
  File "..\monitor.list\bin\Release\GalaSoft.MvvmLight.Extras.pdb"
  File "..\monitor.list\bin\Release\GalaSoft.MvvmLight.Extras.dll"
  File "..\monitor.list\bin\Release\GalaSoft.MvvmLight.dll"
  File "..\monitor.list\bin\Release\CommonServiceLocator.dll"
  File /r "..\monitor.list\bin\Release\*.*"
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\Monitor List\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\Monitor List\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\Monitor.List.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\Monitor.List.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

/******************************
 *  以下是安装程序的卸载部分  *
 ******************************/

Section Uninstall
  Delete "$INSTDIR\${PRODUCT_NAME}.url"
  Delete "$INSTDIR\uninst.exe"
  Delete "$INSTDIR\CommonServiceLocator.dll"
  Delete "$INSTDIR\GalaSoft.MvvmLight.dll"
  Delete "$INSTDIR\GalaSoft.MvvmLight.Extras.dll"
  Delete "$INSTDIR\GalaSoft.MvvmLight.Extras.pdb"
  Delete "$INSTDIR\GalaSoft.MvvmLight.Extras.xml"
  Delete "$INSTDIR\GalaSoft.MvvmLight.pdb"
  Delete "$INSTDIR\GalaSoft.MvvmLight.Platform.dll"
  Delete "$INSTDIR\GalaSoft.MvvmLight.Platform.pdb"
  Delete "$INSTDIR\GalaSoft.MvvmLight.Platform.xml"
  Delete "$INSTDIR\GalaSoft.MvvmLight.xml"
  Delete "$INSTDIR\HandyControl.dll"
  Delete "$INSTDIR\HandyControl.xml"
  Delete "$INSTDIR\Newtonsoft.Json.dll"
  Delete "$INSTDIR\Newtonsoft.Json.xml"
  Delete "$INSTDIR\NLog.config"
  Delete "$INSTDIR\NLog.dll"
  Delete "$INSTDIR\NLog.xml"
  Delete "$INSTDIR\System.ValueTuple.dll"
  Delete "$INSTDIR\System.ValueTuple.xml"
  Delete "$INSTDIR\System.Windows.Interactivity.dll"
  Delete "$INSTDIR\WpfMultiStyle.dll"
  Delete "$INSTDIR\Monitor.List.exe"
  Delete "$INSTDIR\Monitor.List.exe.config"
  Delete "$INSTDIR\Monitor.List.pdb"

  Delete "$SMPROGRAMS\Monitor List\Uninstall.lnk"
  Delete "$SMPROGRAMS\Monitor List\Website.lnk"
  Delete "$DESKTOP\Monitor List.lnk"
  Delete "$SMPROGRAMS\Monitor List\Monitor List.lnk"

  RMDir "$SMPROGRAMS\Monitor List"
  
  RMDir /r "$INSTDIR\logs"
  RMDir /r "$INSTDIR\tr"
  RMDir /r "$INSTDIR\ru"
  RMDir /r "$INSTDIR\pt-BR"
  RMDir /r "$INSTDIR\pl"
  RMDir /r "$INSTDIR\ko-KR"
  RMDir /r "$INSTDIR\fr"
  RMDir /r "$INSTDIR\fa"
  RMDir /r "$INSTDIR\en"
  RMDir /r "$INSTDIR\Data"
  RMDir /r "$INSTDIR\ca-ES"

  RMDir "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  SetAutoClose true
SectionEnd

#-- 根据 NSIS 脚本编辑规则，所有 Function 区段必须放置在 Section 区段之后编写，以避免安装程序出现未可预知的问题。--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "您确实要完全移除 $(^Name) ，及其所有的组件？" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) 已成功地从您的计算机移除。"
FunctionEnd
