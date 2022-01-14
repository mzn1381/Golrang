@echo off
@color 1F

set version=4.0.50.0

echo.
echo *** Copying patched assemblies to GAC ...
xcopy Janus.Windows.ButtonBar.v4.dll  %windir%\assembly\GAC_MSIL\Janus.Windows.ButtonBar.v4\%version%__21d5517571b185bf\Janus.Windows.ButtonBar.v4.dll  /Y /F
echo.
xcopy Janus.Windows.CalendarCombo.v4.dll  %windir%\assembly\GAC_MSIL\Janus.Windows.CalendarCombo.v4\%version%__21d5517571b185bf\Janus.Windows.CalendarCombo.v4.dll  /Y /F
echo.
xcopy Janus.Windows.ExplorerBar.v4.dll  %windir%\assembly\GAC_MSIL\Janus.Windows.ExplorerBar.v4\%version%__21d5517571b185bf\Janus.Windows.ExplorerBar.v4.dll  /Y /F
echo.
xcopy Janus.Windows.GridEX.v4.dll  %windir%\assembly\GAC_MSIL\Janus.Windows.GridEX.v4\%version%__21d5517571b185bf\Janus.Windows.GridEX.v4.dll  /Y /F
echo.
xcopy Janus.Windows.Ribbon.v4.dll  %windir%\assembly\GAC_MSIL\Janus.Windows.Ribbon.v4\%version%__21d5517571b185bf\Janus.Windows.Ribbon.v4.dll  /Y /F
echo.
xcopy Janus.Windows.Schedule.v4.dll  %windir%\assembly\GAC_MSIL\Janus.Windows.Schedule.v4\%version%__21d5517571b185bf\Janus.Windows.Schedule.v4.dll  /Y /F 
echo.
xcopy Janus.Windows.Timeline.v4.dll  %windir%\assembly\GAC_MSIL\Janus.Windows.TimeLine.v4\%version%__21d5517571b185bf\Janus.Windows.Timeline.v4.dll  /Y /F
echo.
xcopy Janus.Windows.UI.v4.dll  %windir%\assembly\GAC_MSIL\Janus.Windows.UI.v4\%version%__21d5517571b185bf\Janus.Windows.UI.v4.dll  /Y /F
echo.
echo Done ...
echo.
pause