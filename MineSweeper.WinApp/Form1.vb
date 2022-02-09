Imports MineSweeper.Core

Public Class Form1

    Private ShowMines As Boolean = False
    Private Const SizeOfSquareLabel As Integer = 25
    Private _mapSize As Integer = 25
    Private _numberOfMines As Integer = 20

    Private _map As Square()()
    Private ReadOnly _labels As List(Of SquareLabel) = New List(Of SquareLabel)
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ResetGame()
    End Sub


    Private Sub ResetGame()
        _mapSize = GetValueInRange("Enter Map Size 4-30:", 4, 30)
        _numberOfMines = GetValueInRange("Enter number of mines 1-" + ((_mapSize * _mapSize) - 1).ToString(), 1, (_mapSize * _mapSize) - 1)
        For Each item In _labels
            Me.Controls.Remove(item)
        Next

        _labels.Clear()
        _map = GenerateMap(_mapSize, _mapSize)
        PlaceMines(_map, _numberOfMines)
        _map.IterateThroughArrayOfArrays(Function(sqaure, x, y)
                                             Dim labelsq = New SquareLabel(sqaure, New Point(x, y)) With {
                                            .Location = New Point((x + 1) * SizeOfSquareLabel, (y + 1) * SizeOfSquareLabel),
                                            .Size = New Size(SizeOfSquareLabel, SizeOfSquareLabel),
                                            .BorderStyle = BorderStyle.FixedSingle,
                                            .TextAlign = ContentAlignment.MiddleCenter
                                            }
                                             _labels.Add(labelsq)
                                             Me.Controls.Add(labelsq)
                                             AddHandler labelsq.MouseClick, AddressOf SqaureLabelClick
                                             Return False
                                         End Function)

        Dim axisSize As Integer = (SizeOfSquareLabel * _mapSize) + (SizeOfSquareLabel * 4)
        Me.Size = New Size(axisSize, axisSize)

        If ShowMines Then
            DisplayMines()
        End If
    End Sub

    Private Sub DisplayMines()
        For Each item In _labels
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
                    CountMineForSquare(_map, label.SquareLocation.X, label.SquareLocation.Y)
                    label.Text = label.Square.Around

                    If label.Square.Around = 0 Then
                        For Each label In From label1 In _labels Where label1.Square.Around IsNot Nothing
                            label.Text = label.Square.Around
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

        If WonGame(_map) Then
            MessageBox.Show("You Win")
            ResetGame()
        End If
    End Sub

    Private Function GetValueInRange(ByVal caption As String, ByVal min As Integer, ByVal max As Integer) As Integer
        Dim input As Integer = GetValue(Of Integer)(caption, Me.Text)
        If input < min Or input > max Then
            Return GetValueInRange(caption, min, max)
        End If
        Return input
    End Function

    Private Shared Function GetValue(Of T As IConvertible)(ByVal text As String, ByVal title As String) As T
        Dim input As String = InputBox(text, title)
        Try
            Dim value As T = Convert.ChangeType(input, GetType(T))
            Return value
        Catch ex As Exception
            Return GetValue(Of T)(text, title)
        End Try
    End Function
End Class
