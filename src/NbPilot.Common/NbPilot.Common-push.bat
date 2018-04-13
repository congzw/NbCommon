@echo off

echo ---- Push Source ----
set packageSource=http://192.168.1.182/MySymbols/NuGet
echo packageSource

nuget push *.symbols.nupkg NugetDemoApiKey -Source %packageSource%

pause