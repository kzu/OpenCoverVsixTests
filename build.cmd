@ECHO OFF
SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION
PUSHD "%~dp0" >NUL

:: Determine if MSBuild can be located. Allows for a better error message below.
where msbuild > %TEMP%\msbuild.txt
set /p msb=<%TEMP%\msbuild.txt

IF "%msb%"=="" (
    echo Please run %~n0 from a Visual Studio Developer Command Prompt.
    exit /b -1
)

set ProfAPI_ProfilerCompatibilitySetting=EnableV2Profiler
set COMPLUS_ProfAPI_ProfilerCompatibilitySetting=EnableV2Profiler
set CLRMONITOR_EXTERNAL_PROFILERS={1542C21D-80C3-45E6-A56C-A9C1E4BEB7B8}
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST src\.nuget\nuget.exe goto restore
md src\.nuget
copy %CACHED_NUGET% src\.nuget\nuget.exe > nul

:restore
IF NOT EXIST build\packages.config goto run
src\.nuget\NuGet.exe install build\packages.config -OutputDirectory build\packages -ExcludeVersion

:run
"%msb%" %~dp0\build.proj /t:Build /p:Dev=. /p:Configuration=Debug /nologo /v:normal /maxcpucount /nr:true %1 %2 %3 %4 %5 %6 %7 %8 %9

POPD >NUL
ENDLOCAL
ECHO ON