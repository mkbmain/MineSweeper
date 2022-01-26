Imports System

Module Program
    Private Map As Square()()
    Private Const _MapSize As Integer = 9
    Private Alphabet As Char() = "abcdefghijklmnopqrstuvwxyz".ToCharArray()
    Private Random As Random = New Random(Guid.NewGuid().GetHashCode())
    Private Function GenerateMap(ByVal mapsize As Integer) As Square()()
        Return Enumerable.Range(0, mapsize).Select(Function(x)
                                                       Return Enumerable.Range(0, mapsize).Select(Function(y)
                                                                                                      Return New Square(False)
                                                                                                  End Function).ToArray()
                                                   End Function).ToArray()
    End Function

    Public Sub PlaceMines(ByVal map As Square()(), ByVal numberOfMines As Integer)
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

    Private Sub PlaceMine(ByVal map As Square()(), Optional ByVal count As Integer = 0)
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

        Dim x As Integer = random.Next(0, map.Length)
        Dim y As Integer = random.Next(0, map(x).Length)
        If (map(x)(y).Mine) Then
            PlaceMine(map, count + 1)
            Return
        End If
        map(x)(y).Mine = True

    End Sub


    Sub DrawMap(ByVal map As Square()())
        Console.Clear()
        Enumerable.Range(1, _MapSize).Select(Function(e)
                                                 Console.SetCursorPosition(e, 0)
                                                 Console.Write(e)
                                                 Console.SetCursorPosition(0, e)
                                                 Console.Write(Alphabet(e - 1))
                                                 Return True
                                             End Function).ToArray()
        map.IterateThroughArrayOfArrays(Function(item, x, y)
                                            Console.SetCursorPosition(x + 1, y + 1)
                                            If item.Mine Then
                                                Console.Write("*")
                                                Return False
                                            End If
                                            If item.Around Is Nothing Then
                                                Console.Write(" ")

                                            Else
                                                Console.Write(item.Around)
                                            End If

                                            Return False
                                        End Function)

        Console.WriteLine()
        Console.WriteLine()
        Console.Write("Enter location i.e b,5")
    End Sub


    Sub Main(args As String())
        Map = GenerateMap(_MapSize)
        PlaceMines(Map, 20)
        DrawMap(Map)
        Console.ReadLine()
    End Sub
End Module
