#f3::
	Suspend, permit
	Suspend, toggle
return
^r::reload

^Space::RunSnips()
+^Space::RunSnipCreator()

; ^,::RunSnips()

; ^.::
	; MsgBox, %A_CaretX% %A_CaretY%
;	; VarSetCapacity(GuiThreadInfo, 48)
;	; NumPut(48, GuiThreadInfo,,"UInt")
;	; 
;	; DllCall("GetGUIThreadInfo", int, 0, ptr, &GuiThreadInfo)
;	; 
;	; left := NumGet(&GuiThreadInfo+8*4)
;	; top := NumGet(&GuiThreadInfo+9*4)
;	; 
;	; MsgBox, %left% %top%
; return

RunSnips(){
    Send, ^c
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


RunSnipCreator() {
    Run %A_ScriptDir%\FtlSnippetCreator.exe
}
