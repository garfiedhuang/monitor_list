; 该脚本使用 HM VNISEdit 脚本编辑器向导产生

; 安装程序初始定义常量
!define PRODUCT_NAME "Xp Resin Print"
!define PRODUCT_VERSION "1.0.0.0"
!define PRODUCT_PUBLISHER "GuangZhou Xiaopeng Automobile Co., Ltd"
!define PRODUCT_WEB_SITE "http://mes.xiaopeng.local"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\Xp.Resin.Print.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI 现代界面定义 (1.67 版本以上兼容) ------
!include "MUI.nsh"

; MUI 预定义常量
!define MUI_ABORTWARNING
!define MUI_ICON "D:\Work\Demo\RMES\zebra_print\xp.resin.print\Assets\Images\icon.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; 欢迎页面
!insertmacro MUI_PAGE_WELCOME
; 安装目录选择页面
!insertmacro MUI_PAGE_DIRECTORY
; 安装过程页面
!insertmacro MUI_PAGE_INSTFILES
; 安装完成页面
!define MUI_FINISHPAGE_RUN "$INSTDIR\Xp.Resin.Print.exe"
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
InstallDir "$PROGRAMFILES\Xp Resin Print"
InstallDirRegKey HKLM "${PRODUCT_UNINST_KEY}" "UninstallString"
ShowInstDetails show
ShowUnInstDetails show
BrandingText " "

Section "MainSection" SEC01
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File "..\xp.resin.print\bin\Release\Xp.Resin.Print.pdb"
  CreateDirectory "$SMPROGRAMS\Xp Resin Print"
  CreateShortCut "$SMPROGRAMS\Xp Resin Print\Xp Resin Print.lnk" "$INSTDIR\Xp.Resin.Print.exe"
  CreateShortCut "$DESKTOP\Xp Resin Print.lnk" "$INSTDIR\Xp.Resin.Print.exe"
  File "..\xp.resin.print\bin\Release\Xp.Resin.Print.exe.config"
  File "..\xp.resin.print\bin\Release\Xp.Resin.Print.exe"
  File "..\xp.resin.print\bin\Release\WpfMultiStyle.dll"
  File "..\xp.resin.print\bin\Release\System.Windows.Interactivity.dll"
  File "..\xp.resin.print\bin\Release\System.ValueTuple.xml"
  File "..\xp.resin.print\bin\Release\System.ValueTuple.dll"
  File "..\xp.resin.print\bin\Release\NLog.xml"
  File "..\xp.resin.print\bin\Release\NLog.dll"
  File "..\xp.resin.print\bin\Release\NLog.config"
  File "..\xp.resin.print\bin\Release\Newtonsoft.Json.xml"
  File "..\xp.resin.print\bin\Release\Newtonsoft.Json.dll"
  File "..\xp.resin.print\bin\Release\HandyControl.xml"
  File "..\xp.resin.print\bin\Release\HandyControl.dll"
  File "..\xp.resin.print\bin\Release\GalaSoft.MvvmLight.xml"
  File "..\xp.resin.print\bin\Release\GalaSoft.MvvmLight.Platform.xml"
  File "..\xp.resin.print\bin\Release\GalaSoft.MvvmLight.Platform.pdb"
  File "..\xp.resin.print\bin\Release\GalaSoft.MvvmLight.Platform.dll"
  File "..\xp.resin.print\bin\Release\GalaSoft.MvvmLight.pdb"
  File "..\xp.resin.print\bin\Release\GalaSoft.MvvmLight.Extras.xml"
  File "..\xp.resin.print\bin\Release\GalaSoft.MvvmLight.Extras.pdb"
  File "..\xp.resin.print\bin\Release\GalaSoft.MvvmLight.Extras.dll"
  File "..\xp.resin.print\bin\Release\GalaSoft.MvvmLight.dll"
  File "..\xp.resin.print\bin\Release\EQuality.Tools.dll"
  File "..\xp.resin.print\bin\Release\CommonServiceLocator.dll"
  File "..\xp.resin.print\bin\Release\Fnthex32.dll"
  File /r "..\xp.resin.print\bin\Release\*.*"
SectionEnd

Section -AdditionalIcons
  WriteIniStr "$INSTDIR\${PRODUCT_NAME}.url" "InternetShortcut" "URL" "${PRODUCT_WEB_SITE}"
  CreateShortCut "$SMPROGRAMS\Xp Resin Print\Website.lnk" "$INSTDIR\${PRODUCT_NAME}.url"
  CreateShortCut "$SMPROGRAMS\Xp Resin Print\Uninstall.lnk" "$INSTDIR\uninst.exe"
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\Xp.Resin.Print.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\Xp.Resin.Print.exe"
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
  Delete "$INSTDIR\Fnthex32.dll"
  Delete "$INSTDIR\EQuality.Tools.dll"
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
  Delete "$INSTDIR\Xp.Resin.Print.exe"
  Delete "$INSTDIR\Xp.Resin.Print.exe.config"
  Delete "$INSTDIR\Xp.Resin.Print.pdb"

  Delete "$SMPROGRAMS\Xp Resin Print\Uninstall.lnk"
  Delete "$SMPROGRAMS\Xp Resin Print\Website.lnk"
  Delete "$DESKTOP\Xp Resin Print.lnk"
  Delete "$SMPROGRAMS\Xp Resin Print\Xp Resin Print.lnk"

  RMDir "$SMPROGRAMS\Xp Resin Print"
  
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
