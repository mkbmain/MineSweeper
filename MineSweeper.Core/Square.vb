﻿Public Class Square
    Public Mine As Boolean
    Public Around As Integer? = Nothing
    Public Flag As Boolean = False
    Public Sub New(mineAtSquare As Boolean)
        Mine = mineAtSquare
    End Sub
End Class
