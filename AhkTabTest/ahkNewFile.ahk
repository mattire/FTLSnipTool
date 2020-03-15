

+f2::
    Input, OutputVar, L1 M
    if (OutputVar=="f")
    {
        ; MsgBox, file
        NewFile()
    }
return

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