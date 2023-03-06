; �ýű�ʹ�� HM VNISEdit �ű��༭���򵼲���

; ��װ�����ʼ���峣��
!define PRODUCT_NAME "Monitor List"
!define PRODUCT_VERSION "1.0.0.0"
!define PRODUCT_PUBLISHER "GuangZhou Garfield Huang Co., Ltd"
!define PRODUCT_WEB_SITE ""
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\Monitor.List.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

SetCompressor lzma

; ------ MUI �ִ����涨�� (1.67 �汾���ϼ���) ------
!include "MUI.nsh"

; MUI Ԥ���峣��
!define MUI_ABORTWARNING
!define MUI_ICON "D:\Work\Demo\MonitorList\monitor_list\monitor.list\Assets\Images\icon.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; ��ӭҳ��
!insertmacro MUI_PAGE_WELCOME
; ��װĿ¼ѡ��ҳ��
!insertmacro MUI_PAGE_DIRECTORY
; ��װ����ҳ��
!insertmacro MUI_PAGE_INSTFILES
; ��װ���ҳ��
!define MUI_FINISHPAGE_RUN "$INSTDIR\Monitor.List.exe"
!insertmacro MUI_PAGE_FINISH

; ��װж�ع���ҳ��
!insertmacro MUI_UNPAGE_INSTFILES

; ��װ�����������������
!insertmacro MUI_LANGUAGE "SimpChinese"

; ��װԤ�ͷ��ļ�
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI �ִ����涨����� ------

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
 *  �����ǰ�װ�����ж�ز���  *
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

#-- ���� NSIS �ű��༭�������� Function ���α�������� Section ����֮���д���Ա��ⰲװ�������δ��Ԥ֪�����⡣--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "��ȷʵҪ��ȫ�Ƴ� $(^Name) ���������е������" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) �ѳɹ��ش����ļ�����Ƴ���"
FunctionEnd
