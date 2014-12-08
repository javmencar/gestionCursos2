Imports System.Data.SqlClient
Public Class Funciones
    Dim cn As SqlConnection
    Public Sub New(ByVal c As SqlConnection)
        cn = c
        cn = New SqlConnection
    End Sub

    Function ValidaNif(ByVal nif As String) As Boolean
        Dim n As Long
        Dim letras() As String = {"T", "R", "W", "A", "G", "M", "Y", "F", "P", "D", "X", "B", "N", "J", "Z", "S", "Q", "V", "H", "L", "C", "K", "E"}
        If (Len(nif) = 9 AndAlso IsNumeric(nif.Substring(0, 8))) Then
            n = CLng(nif.Substring(0, 8))
            n = n Mod 23
            If (UCase(nif.Substring(8)) = letras(CInt(n))) Then
                Return True
            End If
        End If
        Return False
    End Function

    Public Function GetDCNumSegSocial(ByVal numSegSocial As String, _
                                ByVal esNumEmpresa As Boolean) As String
        If (numSegSocial.Length > 10) OrElse (numSegSocial.Length = 0) Then _
            Throw New System.ArgumentException()
        Dim regex As New System.Text.RegularExpressions.Regex("[^0-9]")
        If (regex.IsMatch(numSegSocial)) Then _
            Throw New System.ArgumentException()
        Try
            Dim dcProv As String = numSegSocial.Substring(0, 2)
            Dim numero As String = numSegSocial.Substring(2, numSegSocial.Length - 2)
            Select Case numero.Length
                Case 8
                    If (esNumEmpresa) Then
                        Return String.Empty
                    Else
                        If (numero.Chars(0) = "0"c) Then
                            numero = numero.Remove(0, 1)
                        End If
                    End If
                Case 7
                    If (esNumEmpresa) Then
                        If (numero.Chars(0) = "0"c) Then
                            numero = numero.Remove(0, 1)
                        End If
                    End If
                Case 6
                    If (Not (esNumEmpresa)) Then
                        numero = numero.PadLeft(7, "0"c)
                    End If
                Case Else
                    If (esNumEmpresa) Then
                        numero = numero.PadLeft(6, "0"c)
                    Else
                        numero = numero.PadLeft(7, "0"c)
                    End If
            End Select
            Dim naf As Int64 = Convert.ToInt64(dcProv & numero)
            naf = naf - (naf \ 97) * 97
            Return String.Format("{0:00}", naf)
        Catch
            Return String.Empty
        End Try
    End Function
    Public Function ValidaNumSS(ByVal NSS As String) As Boolean

        Dim CP As String = NSS.Substring(0, 2)
        If CP = "  " Then Return False
        If Not IsNumeric(CP) AndAlso (CInt(CP) < 1 Or CInt(CP) > 50) Then Return False
        Dim NssSCC As String = NSS.Substring(0, NSS.Length - 2)
        Dim dc As String = Me.GetDCNumSegSocial(NssSCC, False)
        Dim s As String = NSS.Substring(NSS.Length - 2, 2)
        If s = dc Then Return True
        Return False
    End Function
    Public Function ejecutarConsultaScalar(ByVal str As String) As Integer

        Dim control As Integer = 0
        Try
            cn.Open()
            Dim cmd As New SqlCommand(str, cn)
            control = cmd.ExecuteScalar
            cn.Close()
        Catch ex As Exception
            control = -1
        Finally
            cn.Close()
        End Try
        Return control
    End Function
    Public Function ejecutarConsultaNonQuery(ByVal str As String) As Integer
        Dim control As Integer = 0
        Try
            cn.Open()
            Dim cmd As New SqlCommand(str, cn)
            control = cmd.ExecuteNonQuery
            cn.Close()
        Catch ex As Exception
            control = -1
        Finally
            cn.Close()
        End Try
        Return control
    End Function
    Public Function comprobarformatofecha(ByVal s As String) As Integer
        Try
            Dim fechacorrecta As Date = DateTime.Parse(s)
        Catch ex1 As FormatException
            Return 1
        Catch ex2 As InvalidCastException
            Return 2
        Catch ex3 As ArgumentException
            Return 3
        Catch ex4 As Exception
            Return 4
        End Try
        Return 0
    End Function
End Class
