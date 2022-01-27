Imports System.Runtime.CompilerServices

Module IterateExtensions
    <Extension()>
    Public Sub IterateThroughArrayOfArrays(Of T)(arrayOfArrays As T()(), action As Action(Of T))
        arrayOfArrays.IterateThroughArrayOfArrays(Function(e)
                                                      action(e)
                                                      Return False
                                                  End Function)
    End Sub

    <Extension()>
    Public Sub IterateThroughArrayOfArrays(Of T)(arrayOfArrays As T()(), action As Func(Of T, Boolean))
        arrayOfArrays.IterateThroughArrayOfArrays(Function(e, x, y)
                                                      Return action(e)
                                                  End Function)
    End Sub

    <Extension()>
    Public Sub IterateThroughArrayOfArrays(Of T)(arrayOfArrays As T()(), action As Func(Of T, Integer, Integer, Boolean))
        For x As Integer = 0 To arrayOfArrays.Length - 1
            For y As Integer = 0 To arrayOfArrays(x).Length - 1
                If action(arrayOfArrays(x)(y), x, y) Then
                    Return
                End If
            Next
        Next
    End Sub

End Module
