Public Module SquareFunctions
    Private Random As Random = New Random(Guid.NewGuid().GetHashCode())

    Public Function GenerateMap(xSize As Integer, ySize As Integer) As Square()()
        Return Enumerable.Range(0, xSize).Select(Function(x)
                                                     Return Enumerable.Range(0, ySize).Select(Function(y)
                                                                                                  Return New Square(False)
                                                                                              End Function).ToArray()
                                                 End Function).ToArray()
    End Function

    Public Sub PlaceMines(ByVal map As Square()(), numberOfMines As Integer)
        Dim total As Integer = map.Select(Function(t)
                                              Return t.Length
                                          End Function).Sum()

        If numberOfMines > total Then
            Throw New ArgumentException("Can not place more mines than spaces there are")
        End If

        For i As Integer = 1 To numberOfMines
            PlaceMine(map)
        Next
    End Sub


    Private Sub PlaceMine(map As Square()(), Optional count As Integer = 0)
        If count > 3 Then
            map.IterateThroughArrayOfArrays(Function(e)
                                                If e.Mine = False Then
                                                    e.Mine = True
                                                    Return True
                                                End If
                                                Return False
                                            End Function)
            Return
        End If

        Dim x As Integer = Random.Next(0, map.Length)
        Dim y As Integer = Random.Next(0, map(x).Length)
        If (map(x)(y).Mine) Then
            PlaceMine(map, count + 1)
            Return
        End If
        map(x)(y).Mine = True

    End Sub

    Public Function WonGame(map As Square()()) As Boolean

        Return map.SelectMany(Function(e)
                                  Return e.Select(Function(b)
                                                      Return b
                                                  End Function)
                              End Function).Where(Function(e)
                                                      Return (e.Mine = True And e.Flag = False) Or (e.Mine = False And e.Flag = True)
                                                  End Function).Any() = False

    End Function
    Public Sub CountMineForSquare(map As Square()(), x As Integer, y As Integer)

        If map(x)(y).Around IsNot Nothing Then
            ' we have already solved this
            Return
        End If

        Dim mines As Integer = 0

        For Each xpos In {x - 1, x, x + 1}
            For Each ypos In {y - 1, y, y + 1}
                If xpos < 0 Or ypos < 0 Then
                    Continue For
                End If
                If xpos > map.Length - 1 Then
                    Continue For
                End If
                If ypos > map(xpos).Length - 1 Then
                    Continue For
                End If

                If map(xpos)(ypos).Mine Then
                    mines += 1
                End If
            Next
        Next

        map(x)(y).Flag = False
        map(x)(y).Around = mines
    End Sub
End Module
