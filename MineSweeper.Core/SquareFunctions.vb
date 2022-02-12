Public Module SquareFunctions
    ' I know there is the secruity RNG 
    Private Random As Random = New Random(Guid.NewGuid().GetHashCode())

    Public Function GenerateMap(xSize As Integer, ySize As Integer) As Square()()
        Return Enumerable.Range(0, xSize).Select(Function(x) Enumerable.Range(0, ySize).Select(Function(y) New Square(False)).ToArray()).ToArray()
    End Function

    Public Sub PlaceMines(ByVal map As Square()(), numberOfMines As Integer)
        Dim total As Integer = map.Select(Function(t) t.Length).Sum()

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

        Return map.SelectMany(Function(e) e.Select(Function(b) b)).Any(
            Function(e) (e.Mine = True AndAlso e.Flag = False) Or (e.Mine = False AndAlso e.Flag = True)) = False

    End Function
    Public Sub CountMineForSquare(map As Square()(), x As Integer, y As Integer, Optional selectedManually As Boolean = True)

        Dim square As Square = map(x)(y)
        If square.Around IsNot Nothing Or (selectedManually = False AndAlso square.Flag) Then
            ' we have already solved this
            Return
        End If


        Dim mines As Integer = 0
        IterateThroughNeighbourMapSquares(map, x, y, Sub(xpos, ypos) If map(xpos)(ypos).Mine Then mines += 1)

        square.Flag = False
        square.Around = mines
        If mines = 0 Then
            IterateThroughNeighbourMapSquares(map, x, y, Sub(xpos, ypos) CountMineForSquare(map, xpos, ypos, False))
        End If
    End Sub


    Private Sub IterateThroughNeighbourMapSquares(map As Square()(), x As Integer, y As Integer, action As Action(Of Integer, Integer))
        For Each xpos In {x - 1, x, x + 1}
            For Each ypos In {y - 1, y, y + 1}
                If xpos < 0 Or ypos < 0 Then
                    Continue For
                ElseIf xpos > map.Length - 1 Then
                    Continue For
                ElseIf ypos > map(xpos).Length - 1 Then
                    Continue For
                End If
                action(xpos, ypos)
            Next
        Next
    End Sub
End Module
