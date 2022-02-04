Imports MineSweeper.Core

Public Class Form1

    Private ShowMines As Boolean = False
    Private Const _SizeOfSquareLabel As Integer = 35
    Private Const _MapSize As Integer = 9
    Private Const _NumberOfMines As Integer = 20

    Private Map As Square()()
    Private Labels As List(Of SquareLabel) = New List(Of SquareLabel)
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ResetGame()
    End Sub

    Private Sub ResetGame()
        For Each item In Labels
            Me.Controls.Remove(item)
        Next

        Labels.Clear()
        Map = GenerateMap(_MapSize, _MapSize)
        PlaceMines(Map, _NumberOfMines)
        Map.IterateThroughArrayOfArrays(Function(sqaure, x, y)
                                            Dim labelsq = New SquareLabel(sqaure, New Point(x, y)) With {
                                            .Location = New Point((x + 1) * _SizeOfSquareLabel, (y + 1) * _SizeOfSquareLabel),
                                            .Size = New Size(_SizeOfSquareLabel, _SizeOfSquareLabel),
                                            .BorderStyle = BorderStyle.FixedSingle,
                                            .TextAlign = ContentAlignment.MiddleCenter
                                            }
                                            Labels.Add(labelsq)
                                            Me.Controls.Add(labelsq)
                                            AddHandler labelsq.MouseClick, AddressOf SqaureLabelClick
                                            Return False
                                        End Function)

        If ShowMines Then
            DisplayMines()
        End If
    End Sub

    Private Sub DisplayMines()
        For Each item In Labels
            If item.Square.Mine Then
                item.Text = "*"
            End If
        Next
    End Sub

    Private Sub SqaureLabelClick(sender As Object, e As MouseEventArgs)

        Dim label As SquareLabel = sender
        Select Case e.Button
            Case MouseButtons.Left
                label.BackColor = Me.BackColor
                If label.Square.Mine Then
                    DisplayMines()
                    MessageBox.Show("GameOver Mine Landed On")
                    ResetGame()
                ElseIf label.Square.Around Is Nothing Then
                    label.Square.Flag = False
                    CountMineForSquare(Map, label.SquareLocation.X, label.SquareLocation.Y)
                    label.Text = label.Square.Around

                    If label.Square.Around = 0 Then
                        For Each label In Labels
                            If label.Square.Around IsNot Nothing Then
                                label.Text = label.Square.Around
                            End If
                        Next
                    End If
                End If
            Case MouseButtons.Right
                If label.Square.Around IsNot Nothing Then
                    Return
                End If
                If label.Square.Flag = True Then
                    label.BackColor = Me.BackColor
                    label.Square.Flag = False
                Else
                    label.BackColor = Color.Red
                    label.Square.Flag = True
                End If
        End Select

        If WonGame(Map) Then
            MessageBox.Show("You Win")
            ResetGame()
        End If
    End Sub
End Class
