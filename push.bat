@echo off
set /p apiKey= Please input the ApiKey from Nuget:
nuget.exe push -Source https://api.nuget.org/v3/index.json "src\CLRStats\bin\Debug\CLRStats.1.0.0.nupkg" -ApiKey %apiKey%
pause