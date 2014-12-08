Imports System.Data.SqlClient
'formulario borrable, es para hacer pruebas
Public Class Form2
 
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.txtConexion.Text = ConeStr
        Me.txtPathExportacion.Text = PathExportacionInicial
        Me.txtPathFotos.Text = PathFotos
    End Sub

    Private Sub cmdCambiarConexion_Click(sender As Object, e As EventArgs) Handles cmdCambiarConexion.Click
        Dim resp As MsgBoxResult
        resp = MsgBox(String.Format("Va a cambiar la conexion de la base de datos de:" & vbCrLf &
                                  "'{0}' a '{1}'" & vbCrLf &
                                  "¿Está seguro?", ConeStr, Me.txtConexion.Text), MsgBoxStyle.YesNo)
        If resp = MsgBoxResult.Yes Then
            ConeStr = Me.txtConexion.Text
        Else
            MsgBox("Cambio abortado a peticion del usuario")
        End If
    End Sub

    Private Sub cmdCambiarPathFoto_Click(sender As Object, e As EventArgs) Handles cmdCambiarPathFoto.Click
        Dim resp As MsgBoxResult
        resp = MsgBox(String.Format("Va a cambiar la ubicacion de las fotos de las fichas de:" & vbCrLf &
                                  "'{0}' a '{1}'" & vbCrLf &
                                  "¿Está seguro?", lblPathFoto, Me.txtPathFotos.Text), MsgBoxStyle.YesNo)
        If resp = MsgBoxResult.Yes Then
            PathFotos = Me.txtPathFotos.Text
        Else
            MsgBox("Cambio abortado a peticion del usuario")
        End If
    End Sub

    Private Sub cmdCambiarPathExportacion_Click(sender As Object, e As EventArgs) Handles cmdCambiarPathExportacion.Click
        Dim resp As MsgBoxResult
        resp = MsgBox(String.Format("Va a cambiar el destino de los datos exportados  de:" & vbCrLf &
                                  "'{0}' a '{1}'" & vbCrLf &
                                  "¿Está seguro?", PathExportacionInicial, Me.txtPathExportacion.Text), MsgBoxStyle.YesNo)
        If resp = MsgBoxResult.Yes Then
            PathExportacionInicial = Me.txtPathExportacion.Text
        Else
            MsgBox("Cambio abortado a peticion del usuario")
        End If
    End Sub
End Class