#f3::
	Suspend, permit
	Suspend, toggle
return
^r::reload

,::RunSnips()

RunSnips(){
    Run %A_ScriptDir%\SnippetUtil.exe
    WinWait, Form1
    WinWaitClose, Form1
    Sleep, 50
	WinGetTitle, WinTitle, A
	StringLeft, StartStr, WinTitle, 7
	if(StartStr="MINGW64")
	{
		Send, +{insert}
	}
	else
	{
		Send, ^v
	}
}
