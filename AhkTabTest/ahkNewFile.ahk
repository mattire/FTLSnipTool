

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
    bgn := SubStr(clipboard, 1, 5)
    if(bgn=="{tab}")
    {
        MsgBox, Bingo!
    } else {
        MsgBox, %bgn%    
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