

+f2::
    Input, OutputVar, L1 M
    if (OutputVar=="f")
    {
        NewFile()
    }
    else if (OutputVar=="t")
    {
        ; MsgBox, test
        ClipboardTest()
    }
    else if (OutputVar=="s")
    {
        ClipboardStartsWithTab()
    }
return

ClipboardStartsWithTab()
{
    MsgBox, %clipboard%
    ;bgn := clipboard
    ;source := clipboard
    ;bgn := StringLeft, bgn, source, 5
    ; StartsWith(clipboard, "{tab}`r`n")
    
    if(StartsWith(clipboard, "{tab}`r`n"))
    {
        rest := SubStr(clipboard, StrLen("{tab}`r`n"))
        MsgBox, %rest%
        MsgBox, Bingo2!
    }
    
    bgn := SubStr(clipboard, 1, 7)
    if(bgn=="{tab}`r`n")
    {
        MsgBox, Bingo!
        theRest := SubStr(clipboard, 8)
        ; Send, %theRest%    
        arr := StrSplit(theRest,"`n")
        Loop % arr.MaxIndex()
        {
            txt := arr[A_Index]
            Send, %txt%
            Send, {tab}
        }
    } else {
        MsgBox, %bgn%    
    }
    
}

StartsWith(str, startsStr)
{
    len := StrLen(startsStr)
    MsgBox, %len%
    start := SubStr(str, 1, len)
    MsgBox, %start%
    if(start == startsStr){
        return true
    } else {
        return false
    }
}

ClipboardTest()
{
    ; MsgBox, %clipboard%
    arr := StrSplit(clipboard,"`n")
    ; arr := StrSplit(content,"{tab}")
    Loop % arr.MaxIndex()
    {
        txt := arr[A_Index]
        ; MsgBox %txt%
        Send, %txt%
        Send, {tab}
    }
}

NewFile()
{
    WinGetActiveStats, Title, Width, Height, X, Y
    MouseMove, Width / 2, Height - 100, 0

    Click, Left, 1
    Sleep, 310
    Click, Right, 1
    Sleep, 10
    SendRaw, w
    Sleep, 10
    SendRaw, w
    Sleep, 10
    Send, {Right}
    Sleep, 10
    SendRaw, t
}