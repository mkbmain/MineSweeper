Imports System

Module Program
    Private Map As Square()()
    Private Const _MapSize As Integer = 9
    Private Alphabet As List(Of Char) = "abcdefghijklmnopqrstuvwxyz".ToCharArray().ToList()
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

        Dim x As Integer = Random.Next(0, map.Length)
        Dim y As Integer = Random.Next(0, map(x).Length)
        If (map(x)(y).Mine) Then
            PlaceMine(map, count + 1)
            Return
        End If
        map(x)(y).Mine = True

    End Sub


    Sub DrawMap(ByVal map As Square()())
        Console.Clear()
        Enumerable.Range(0, _MapSize).Select(Function(e)
                                                 Console.SetCursorPosition(e + 1, 0)
                                                 Console.Write(e)
                                                 Console.SetCursorPosition(0, e + 1)
                                                 Console.Write(Alphabet(e))
                                                 Return True
                                             End Function).ToArray()
        map.IterateThroughArrayOfArrays(Function(item, x, y)
                                            Console.SetCursorPosition(x + 1, y + 1)

                                            If item.Mine And item.Flag = False Then
                                                Console.Write("*")
                                            End If

                                            If item.Flag Then
                                                Console.Write("F")
                                            ElseIf item.Around Is Nothing Then
                                                Console.Write(" ")
                                            Else
                                                Console.Write(item.Around)
                                            End If

                                            Return False
                                        End Function)

        Console.WriteLine()
        Console.WriteLine()
        Console.WriteLine("to go to a squre type in letter,row i.e b,5")
        Console.WriteLine("to place a flag type in letter,row i.e b,5=f")
        Console.Write("Enter location:")
    End Sub

    Private Sub CalulateMines(ByVal x As Integer, ByVal y As Integer)

        If (Map(x)(y).Around IsNot Nothing) Then
            Return
        End If
        Dim allYPos As Integer() = {y - 1, y, y + 1}
        Dim allXPos As Integer() = {x - 1, x, x + 1}

        Dim mines As Integer = 0

        For Each xpos In allXPos
            For Each ypos In allYPos
                If xpos < 0 Or ypos < 0 Then
                    Continue For
                End If
                If xpos > Map.Length - 1 Then
                    Continue For
                End If
                If ypos > Map(xpos).Length - 1 Then
                    Continue For
                End If

                If Map(xpos)(ypos).Mine Then
                    mines += 1
                End If
            Next
        Next

        Map(x)(y).Flag = False
        Map(x)(y).Around = mines
    End Sub


    Private Function WonGame() As Boolean

        Dim allSquares As IEnumerable(Of Square) = Map.SelectMany(Function(e)
                                                                      Return e.Select(Function(b)
                                                                                          Return b
                                                                                      End Function)
                                                                  End Function)

        Dim missedMine As Boolean = allSquares.Where(Function(e)
                                                         Return e.Mine = True And e.Flag = False
                                                     End Function).Any()

        Dim wrongMine As Boolean = allSquares.Where(Function(e)
                                                        Return e.Mine = False And e.Flag = True
                                                    End Function).Any()

        Return missedMine = False And wrongMine = False

    End Function


    Sub Main(args As String())
        Map = GenerateMap(_MapSize)
        PlaceMines(Map, 20)
        Dim run As Boolean = True
        While run
            DrawMap(Map)
            Dim read As String = Console.ReadLine().ToLower()
            Dim flag As Boolean = read.Contains("=f")
            read = read.Replace("=f", "")
            Dim parts As String() = read.Split(",")
            If parts.Length <> 2 Then
                Console.WriteLine("Invalid")
                Continue While
            End If

            Dim y As Integer = Alphabet.IndexOf(parts(0))
            Dim x As Integer
            If Integer.TryParse(parts(1), x) = False Then
                Console.WriteLine("Invalid x pos")
                Continue While
            End If

            If x < 0 Or x > Map.Length - 1 Then
                Console.WriteLine("Invalid x pos")
            ElseIf y < 0 Or y > Map(x).Length - 1 Then
                Console.WriteLine("Invalid y pos")
            ElseIf Map(x)(y).Mine And flag = False Then
                Console.WriteLine("Game Over! Mine")
                Return
            Else
                If flag Then
                    If Map(x)(y).Around Is Nothing Then
                        Map(x)(y).Flag = True
                    Else
                        Console.WriteLine("invalid flag move")
                        Continue While
                    End If

                Else
                    CalulateMines(x, y)
                End If

                If WonGame() Then
                    Console.WriteLine("Game Over! you win")
                    Return
                End If
            End If
        End While
    End Sub
End Module
