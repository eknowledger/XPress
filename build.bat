@ECHO OFF
set /P build="Choose build type? [R]elease, [T]est, [D]ebug: "

SET script='./debug.ps1'
if /i "%build:~,1%" EQU "r" (SET script='./release.ps1')
if /i "%build:~,1%" EQU "t" (SET script='./debug-cover.ps1')


PUSHD %~dp0
PowerShell.exe -NoProfile -ExecutionPolicy Bypass -Command "& %script%"

IF %errorlevel% neq 0 PAUSE

