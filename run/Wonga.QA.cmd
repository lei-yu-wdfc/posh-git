@ECHO OFF
CLS

SET Root=%~dp0\..
SET Run=%Root%\run
SET Src=%Root%\src
SET Bin=%Root%\bin

SET MSBuild=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe /nologo

:MENU
ECHO.
ECHO   1. Build all solutions
ECHO   2. Set SUT ^& AUT
ECHO   3. Rebase from Upstream
ECHO   4. Run Wonga.QA.Tests
ECHO   5. Run Wonga.QA.Tests.Meta for all AUTs against RC
ECHO   6. Run Wonga.QA.Generators
ECHO   7. Set ProxyMode
ECHO   0. Exit
ECHO.

CHOICE /C 12345670 /M "But if you already know, how can I make a choice?" /N

IF ERRORLEVEL 8 GOTO EOF
IF ERRORLEVEL 7 GOTO 7
IF ERRORLEVEL 6 GOTO 6
IF ERRORLEVEL 5 GOTO 5
IF ERRORLEVEL 4 GOTO 4
IF ERRORLEVEL 3 GOTO 3
IF ERRORLEVEL 2 GOTO 2
IF ERRORLEVEL 1 GOTO 1
GOTO EOF

:1
	CD %Root% && FOR /R %%0 IN (*.sln) DO %MSBuild% %%0 /v:m || PAUSE
GOTO MENU

:2
	SET /P AUT=Enter AUT (E.g. Uk, Za, Ca, Wb, Pl): 
	SET /P SUT=Enter SUT (E.g. Dev, WIP, RC): 
	SETX AUT %AUT% > NUL
	SETX SUT %SUT% > NUL
GOTO MENU

:3
	CALL git remote add upstream git@github.com:QuickbridgeLtd/v3QA.git 2> NUL
	CALL git pull --rebase upstream master
	IF ERRORLEVEL 1 EXIT /B
GOTO MENU

:4
	SET Files=
	SET /P Files=Semicolon-separated projects, blank for default (e.g. Meta;Api;Ops;Ui): 
	%MsBuild% %Run%\Wonga.QA.Tests.build /p:Files="%Files%" || PAUSE
GOTO MENU

:5
	%MsBuild% %Run%\Wonga.QA.Tests.build /t:Build /v:m || PAUSE
	CALL :META RC Uk
	CALL :META RC Za
	CALL :META RC Ca
	CALL :META RC Wb
GOTO MENU

:6
	SET /P Origin=Path to v3 [..\v3]: 
	CHOICE /C ACM /M "Api, Cs or Msmq"	
	IF ERRORLEVEL 3 CALL :GENERATE Msmq
	IF ERRORLEVEL 2 CALL :GENERATE Cs
	IF ERRORLEVEL 1 CALL :GENERATE Api
GOTO MENU

:7
	CHOICE /C YN /M "Are you working through a proxy"
	IF ERRORLEVEL 2 SETX QAFProxyMode False > NUL
	IF ERRORLEVEL 1 SETX QAFProxyMode True > NUL
GOTO MENU

:META
	%MsBuild% %Run%\Wonga.QA.Tests.build /t:Config;Test /p:Files=Meta;SUT=%1;AUT=%2 || PAUSE
GOTO EOF

:GENERATE
	%MsBuild% %Src%\Wonga.QA.Generators\Wonga.QA.Generators.sln /v:m || PAUSE
	%Bin%\Wonga.QA.Generators.%1.exe %Origin%
GOTO EOF

:EOF