REM delete existing nuget packages
del *.nupkg
powershell -noexit -executionpolicy bypass -File "nuget-pack.ps1"
pause