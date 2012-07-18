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
ECHO   2. Configure your environment
ECHO   3. Rebase from Upstream
ECHO   4. Run Wonga.QA.Tests
ECHO   5. Run Meta and Core tests
ECHO   6. Run Wonga.QA.Generators
ECHO   7. Service test configuration
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
	:CD %Root% && FOR /R %%0 IN (*.sln) DO %MSBuild% %%0 /v:m || PAUSE
	%MsBuild% %Run%\Wonga.QA.Tests.build /t:Build /v:m || PAUSE
GOTO MENU

:2
	SET /P TestingTarget=Enter your testing target(v3 [deployto] flag):
	SETX QAFTestTarget %TestingTarget%
	REM echo Removing existing settings
	REM rd %APPDATA%\v3qa /s /q
	REM echo Creating settings directory @ %APPDATA%\v3qa
	REM mkdir %APPDATA%\v3qa
	REM echo Copying %Run%\config\%TestingTarget%.v3qaconfig to %APPDATA%\v3qa\%TestingTarget%.v3qaconfig
	REM copy %Run%\config\%TestingTarget%.v3qaconfig %APPDATA%\v3qa\%TestingTarget%.v3qaconfig
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
	REM SET /P AUT=Enter AUT (E.g. Uk, Za, Ca, Wb, Pl): 
	REM SET /P SUT=Enter SUT (E.g. Dev, WIP, UAT, RC, WIPRelease, RCRelease, Live): 
	%MsBuild% %Run%\Wonga.QA.Tests.build /t:SanityTest /v:m || PAUSE
GOTO MENU

:6
	SET /P Origin=Path to v3 [..\v3]: 
	CHOICE /C ACM /M "Api, Cs or Msmq"	
	IF ERRORLEVEL 3 CALL :GENERATE Msmq
	IF ERRORLEVEL 2 CALL :GENERATE Cs
	IF ERRORLEVEL 1 CALL :GENERATE Api
GOTO MENU

:7
 
ECHO.
ECHO   1. Configure service test
ECHO   2. Restore orginal configuration
ECHO   0. Menu
ECHO.

CHOICE /C 120 /M "But if you already know, how can I make a choice?" /N
IF ERRORLEVEL 3 GOTO MENU
IF ERRORLEVEL 2 GOTO 72
IF ERRORLEVEL 1 GOTO 71

GOTO EOF

:71 
	SET /P TestingTarget=Enter your server name (e.g. risk):
	SETX QAFTestTarget %TestingTarget%	
	powershell -command "& {. %Run%\EditEndPointConfig.ps1; configservice %TestingTarget% }"
 GOTO MENU
 
:72
	SET /P TestingTarget=Enter your server name to restore(e.g.risk):
	SETX QAFTestTarget %TestingTarget%
	powershell -command "& {. %Run%\EditEndPointConfig.ps1; configservice_undo %TestingTarget% }"
 GOTO MENU
 

:META
	%MsBuild% %Run%\Wonga.QA.Tests.build /t:Test /p:Files=Meta || PAUSE
GOTO EOF

:CORE
	%MsBuild% %Run%\Wonga.QA.Tests.build /t:Merge;Test /p:TestFilter="Category:CoreTest" || PAUSE 
GOTO EOF

:GENERATE
	%MsBuild% %Src%\Wonga.QA.Generators\Wonga.QA.Generators.sln /v:m || PAUSE
	%Bin%\Wonga.QA.Generators.%1.exe %Origin%
GOTO EOF

:EOF