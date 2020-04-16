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
    ;bgn := SubStr(clipboard, 1, 7)
    ;if(bgn=="{tab}`r`n")
    ; if(StartsWith(clipboard, "{tab}`r`n"))
    if(StartsWith(clipboard, "{tab}"))
    {
        RunTabSequence()
    }
    else
    {
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
}


RunSnipCreator() {
    Run %A_ScriptDir%\FtlSnippetCreator.exe
}

RunTabSequence() {
    ; theRest := SubStr(clipboard, 8)
    ; Send, %theRest% 
    ; arr := StrSplit(theRest,"`n")
    arr := StrSplit(clipboard,"`n")
    arr.Remove(0)
    Loop % arr.MaxIndex()
    {
        txt := arr[A_Index]
        Send, %txt%
        Send, {tab}
    }
}

StartsWith(str, startsStr)
{
    len := StrLen(startsStr) 
    ; MsgBox, %len%
    start := SubStr(str, 1, len)
    ; MsgBox, %start%
    if(start == startsStr){
        return true
    } else {
        return false
    }
}
