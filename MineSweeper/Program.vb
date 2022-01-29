Imports System

Module Program
    Private _showMines As Boolean = False
    Private Const NumberOfMines As Integer = 20
    Private Const MapSize As Integer = 9
    Private ReadOnly Alphabet As List(Of Char) = "abcdefghijklmnopqrstuvwxyz".ToCharArray().ToList()
    Private ReadOnly Map As Square()() = Enumerable.Range(0, MapSize).Select(Function(x)
                                                                                 Return Enumerable.Range(0, MapSize).Select(Function(y)
                                                                                                                                Return New Square(False)
                                                                                                                            End Function).ToArray()
                                                                             End Function).ToArray()






    Sub DrawMap(map As Square()())
        Console.Clear()
        Enumerable.Range(0, MapSize).Select(Function(e)
                                                Console.SetCursorPosition(e + 1, 0)
                                                Console.Write(e)
                                                Console.SetCursorPosition(0, e + 1)
                                                Console.Write(Alphabet(e))
                                                Return True
                                            End Function).ToArray()
        map.IterateThroughArrayOfArrays(Function(item, x, y)
                                            Console.SetCursorPosition(x + 1, y + 1)

                                            If _showMines And item.Mine And item.Flag = False Then
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

    End Sub


    Sub Main(args As String())
        PlaceMines(Map, NumberOfMines)
        Dim message As String = ""
        While True
            DrawMap(Map)
            Console.WriteLine(message)
            Console.Write("Enter location:")
            message = ""
            Dim read As String = Console.ReadLine().ToLower()
            Dim flag As Boolean = read.Contains("=f")
            read = read.Replace("=f", "")
            Dim parts As String() = read.Split(",")
            If parts.Length <> 2 Then
                message = "Invalid"
                Continue While
            End If

            Dim y As Integer = Alphabet.IndexOf(parts(0))
            Dim x As Integer
            If Integer.TryParse(parts(1), x) = False Then
                message = "Invalid x pos"
                Continue While
            End If

            If x < 0 Or x > Map.Length - 1 Then
                message = "Invalid x pos"
            ElseIf y < 0 Or y > Map(x).Length - 1 Then
                message = "Invalid y pos"
            ElseIf Map(x)(y).Mine And flag = False Then
                _showMines = True
                DrawMap(Map)
                Console.WriteLine("Game Over! Mine")
                Return
            Else
                If flag Then
                    If Map(x)(y).Around Is Nothing Then
                        Map(x)(y).Flag = True
                    Else
                        message = "Invalid flag move"
                        Continue While
                    End If

                Else
                    SquareFunctions.CountMineForSquare(Map, x, y)
                End If

                If SquareFunctions.WonGame(Map) Then
                    Console.WriteLine("Game Over! you win")
                    Return
                End If
            End If
        End While
    End Sub
End Module
