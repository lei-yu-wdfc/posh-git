@ECHO OFF
CLS

SET Root=%0\..\..
SET Run=%Root%\run
SET Src=%Root%\src
SET Bin=%Root%\bin

SET MSBuild=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe /nologo

:MENU
ECHO.
ECHO   1. Build all solutions
ECHO   2. Set SUT ^& AUT
ECHO   3. Run Wonga.QA.Tests
ECHO   4. Run Wonga.QA.Tests for all AUTs against RC
ECHO   5. Run Wonga.QA.Generators.Api
ECHO   6. Run Wonga.QA.Generators.Msmq
ECHO   7. Run Wonga.QA.Generators.Db
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
	SET /P AUT=Enter AUT (E.g. Uk, Za, Ca, Wb): 
	SET /P SUT=Enter SUT (E.g. Dev, WIP, RC): 
	SETX AUT %AUT% > NUL
	SETX SUT %SUT% > NUL
GOTO MENU

:3
	%MsBuild% %Run%\Wonga.QA.Tests.build || PAUSE
GOTO MENU

:4
	%MsBuild% %Run%\Wonga.QA.Tests.build /t:Build;Merge /v:m || PAUSE
	CALL :TEST RC Uk
	CALL :TEST RC Za
	CALL :TEST RC Ca
	CALL :TEST RC Wb
GOTO MENU

:5
	CALL :GENERATE Api
GOTO MENU

:6
	CALL :GENERATE Msmq
GOTO MENU

:7
	CALL :GENERATE Db
GOTO MENU

:TEST
	%MsBuild% %Run%\Wonga.QA.Tests.build /t:Config;Test /p:SUT=%1;AUT=%2 || PAUSE
GOTO EOF

:GENERATE
	SET /P Origin=Enter path to v3 [..\v3]: 
	%MsBuild% %Src%\Wonga.QA.Generators\Wonga.QA.Generators.sln /v:m || PAUSE
	%Bin%\Wonga.QA.Generators\Wonga.QA.Generators.%1.exe %Origin%
GOTO EOF

:EOF