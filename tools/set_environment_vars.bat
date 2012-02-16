@echo off
set /p aut=Enter AUT (Eg. WB):
set /p sut=Enter SUT (Eg. RC):
setx AUT %aut%
setx SUT %sut%