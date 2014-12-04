Imports System.Data.SqlClient

Public Class FrmComentarios
    Public FI As Ficha
    Public cn As SqlConnection

    Sub New(ByRef F As Ficha)

        ' Llamada necesaria para el diseñador.
        InitializeComponent()
        ' coment = com
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        FI = F
    End Sub

    Private Sub FrmComentarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' cn = New SqlConnection(ConeStr)
        If Not IsNothing(FI.Comentarios) Then
            Me.txtComentarios.Text = FI.Comentarios.Replace("#", vbCrLf)
        End If
    End Sub
    Private Sub cmdGuardar_Click(sender As Object, e As EventArgs) Handles cmdGuardar.Click
        FI.Comentarios = Me.txtComentarios.Text.Replace(vbCrLf, "#")

        DialogResult = Windows.Forms.DialogResult.OK
    End Sub
    Private Sub cmdSalir_Click(sender As Object, e As EventArgs) Handles cmdSalir.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub


End Class