!include "MUI2.nsh"

; The name of the installer
Name "The A-Maze-ing Escape"

; The file to write
OutFile "bin\Windows\x86\Release\Install The A-Maze-ing Escape.exe"

; The default installation directory
InstallDir $PROGRAMFILES\TheAMazeIngEscape

; Registry key to check for directory (so if you install again, it will 
; overwrite the old one automatically)
InstallDirRegKey HKLM "Software\TheAMazeIngEscape" "Install_Dir"

; Request application privileges for Windows Vista
RequestExecutionLevel admin

;--------------------------------

; Pages

!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
  
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES
!insertmacro MUI_LANGUAGE "English"
;--------------------------------

; The stuff to install
Section "The A-Maze-ing Escape (required)"

  SectionIn RO
  
  ; Set output path to the installation directory.
  SetOutPath $INSTDIR
  
  ; Put file there
  File /r "bin\Windows\x86\Release\*"
  
  ; Write the installation path into the registry
  WriteRegStr HKLM SOFTWARE\TheAMazeIngEscape "Install_Dir" "$INSTDIR"
  
  ; Write the uninstall keys for Windows
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TheAMazeIngEscape" "DisplayName" "The A-Maze-ing Escape"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TheAMazeIngEscape" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TheAMazeIngEscape" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TheAMazeIngEscape" "NoRepair" 1
  WriteUninstaller "uninstall.exe"
  
SectionEnd

; Optional section (can be disabled by the user)
Section "Start Menu Shortcuts"

  CreateDirectory "$SMPROGRAMS\The A-Maze-ing Escape"
  CreateShortcut "$SMPROGRAMS\The A-Maze-ing Escape\Uninstall.lnk" "$INSTDIR\uninstall.exe" "" "$INSTDIR\uninstall.exe" 0
  CreateShortcut "$SMPROGRAMS\The A-Maze-ing Escape\The A-Maze-ing Escape.lnk" "$INSTDIR\The A-Maze-ing Escape.exe" "" "$INSTDIR\The A-Maze-ing Escape.exe" 0
  
SectionEnd

;--------------------------------

; Uninstaller

Section "Uninstall"
  
  ; Remove registry keys
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\TheAMazeIngEscape"
  DeleteRegKey HKLM SOFTWARE\TheAMazeIngEscape

  ; Remove directories used
  RMDir /r "$SMPROGRAMS\The A-Maze-ing Escape"
  RMDir /r "$INSTDIR"

SectionEnd
