Imports System.Data.SqlClient

Public Class FrmComentarios
    Public Fi As Ficha
    Public cn As SqlConnection

    Sub New(ByRef dat As Ficha)

        ' Llamada necesaria para el diseñador.
        InitializeComponent()
        Fi = dat
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().

    End Sub

    Private Sub FrmComentarios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' cn = New SqlConnection(ConeStr)
        If Not IsNothing(Fi.Comentarios) Then
            Me.txtComentarios.Text = Fi.Comentarios
        End If
    End Sub
    Private Sub cmdGuardar_Click(sender As Object, e As EventArgs) Handles cmdGuardar.Click
        Fi.Comentarios = Me.txtComentarios.Text
        DialogResult = Windows.Forms.DialogResult.OK
    End Sub
    Private Sub cmdSalir_Click(sender As Object, e As EventArgs) Handles cmdSalir.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub


End Class