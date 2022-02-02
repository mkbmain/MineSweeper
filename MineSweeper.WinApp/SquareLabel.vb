Imports MineSweeper.Core

Public Class SquareLabel
    Inherits System.Windows.Forms.Label

    Public SquareLocation As System.Drawing.Point
    Public Square As Square

    Public Sub New(refSquare As Square, location As Point)
        SquareLocation = location
        Square = refSquare
    End Sub
End Class
