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
    Send, ^v
}
