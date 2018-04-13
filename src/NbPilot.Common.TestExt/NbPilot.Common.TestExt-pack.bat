@echo off

echo ---- Pack ----

set packageName=NbPilot.Common.TestExt
echo packageName

REM nuget pack %packageName%.csproj -IncludeReferencedProjects
REM nuget pack %packageName%.csproj -properties Configuration=Release

nuget pack %packageName%.csproj -symbols

pause

