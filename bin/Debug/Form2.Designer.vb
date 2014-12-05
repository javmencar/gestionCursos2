<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblConectionString = New System.Windows.Forms.Label()
        Me.txtConexion = New System.Windows.Forms.TextBox()
        Me.cmdCambiarConexion = New System.Windows.Forms.Button()
        Me.cmdCambiarPathFoto = New System.Windows.Forms.Button()
        Me.txtPathFotos = New System.Windows.Forms.TextBox()
        Me.lblPathFoto = New System.Windows.Forms.Label()
        Me.cmdCambiarPathExportacion = New System.Windows.Forms.Button()
        Me.txtPathExportacion = New System.Windows.Forms.TextBox()
        Me.LblPathExportacion = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblConectionString
        '
        Me.lblConectionString.AutoSize = True
        Me.lblConectionString.Location = New System.Drawing.Point(62, 37)
        Me.lblConectionString.Name = "lblConectionString"
        Me.lblConectionString.Size = New System.Drawing.Size(141, 13)
        Me.lblConectionString.TabIndex = 0
        Me.lblConectionString.Text = "conexion con base de datos"
        '
        'txtConexion
        '
        Me.txtConexion.Location = New System.Drawing.Point(211, 34)
        Me.txtConexion.Name = "txtConexion"
        Me.txtConexion.Size = New System.Drawing.Size(403, 20)
        Me.txtConexion.TabIndex = 1
        '
        'cmdCambiarConexion
        '
        Me.cmdCambiarConexion.Location = New System.Drawing.Point(620, 31)
        Me.cmdCambiarConexion.Name = "cmdCambiarConexion"
        Me.cmdCambiarConexion.Size = New System.Drawing.Size(75, 23)
        Me.cmdCambiarConexion.TabIndex = 2
        Me.cmdCambiarConexion.Text = "Guardar"
        Me.cmdCambiarConexion.UseVisualStyleBackColor = True
        '
        'cmdCambiarPathFoto
        '
        Me.cmdCambiarPathFoto.Location = New System.Drawing.Point(620, 78)
        Me.cmdCambiarPathFoto.Name = "cmdCambiarPathFoto"
        Me.cmdCambiarPathFoto.Size = New System.Drawing.Size(75, 23)
        Me.cmdCambiarPathFoto.TabIndex = 5
        Me.cmdCambiarPathFoto.Text = "Guardar"
        Me.cmdCambiarPathFoto.UseVisualStyleBackColor = True
        '
        'txtPathFotos
        '
        Me.txtPathFotos.Location = New System.Drawing.Point(211, 80)
        Me.txtPathFotos.Name = "txtPathFotos"
        Me.txtPathFotos.Size = New System.Drawing.Size(403, 20)
        Me.txtPathFotos.TabIndex = 4
        '
        'lblPathFoto
        '
        Me.lblPathFoto.AutoSize = True
        Me.lblPathFoto.Location = New System.Drawing.Point(62, 84)
        Me.lblPathFoto.Name = "lblPathFoto"
        Me.lblPathFoto.Size = New System.Drawing.Size(89, 13)
        Me.lblPathFoto.TabIndex = 3
        Me.lblPathFoto.Text = "Path de las Fotos"
        '
        'cmdCambiarPathExportacion
        '
        Me.cmdCambiarPathExportacion.Location = New System.Drawing.Point(620, 129)
        Me.cmdCambiarPathExportacion.Name = "cmdCambiarPathExportacion"
        Me.cmdCambiarPathExportacion.Size = New System.Drawing.Size(75, 23)
        Me.cmdCambiarPathExportacion.TabIndex = 8
        Me.cmdCambiarPathExportacion.Text = "Guardar"
        Me.cmdCambiarPathExportacion.UseVisualStyleBackColor = True
        '
        'txtPathExportacion
        '
        Me.txtPathExportacion.Location = New System.Drawing.Point(211, 128)
        Me.txtPathExportacion.Name = "txtPathExportacion"
        Me.txtPathExportacion.Size = New System.Drawing.Size(403, 20)
        Me.txtPathExportacion.TabIndex = 7
        '
        'LblPathExportacion
        '
        Me.LblPathExportacion.AutoSize = True
        Me.LblPathExportacion.Location = New System.Drawing.Point(62, 135)
        Me.LblPathExportacion.Name = "LblPathExportacion"
        Me.LblPathExportacion.Size = New System.Drawing.Size(103, 13)
        Me.LblPathExportacion.TabIndex = 6
        Me.LblPathExportacion.Text = "Path de Exportacion"
        '
        'Form2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(819, 417)
        Me.Controls.Add(Me.cmdCambiarPathExportacion)
        Me.Controls.Add(Me.txtPathExportacion)
        Me.Controls.Add(Me.LblPathExportacion)
        Me.Controls.Add(Me.cmdCambiarPathFoto)
        Me.Controls.Add(Me.txtPathFotos)
        Me.Controls.Add(Me.lblPathFoto)
        Me.Controls.Add(Me.cmdCambiarConexion)
        Me.Controls.Add(Me.txtConexion)
        Me.Controls.Add(Me.lblConectionString)
        Me.Name = "Form2"
        Me.Text = "Form2"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblConectionString As System.Windows.Forms.Label
    Friend WithEvents txtConexion As System.Windows.Forms.TextBox
    Friend WithEvents cmdCambiarConexion As System.Windows.Forms.Button
    Friend WithEvents cmdCambiarPathFoto As System.Windows.Forms.Button
    Friend WithEvents txtPathFotos As System.Windows.Forms.TextBox
    Friend WithEvents lblPathFoto As System.Windows.Forms.Label
    Friend WithEvents cmdCambiarPathExportacion As System.Windows.Forms.Button
    Friend WithEvents txtPathExportacion As System.Windows.Forms.TextBox
    Friend WithEvents LblPathExportacion As System.Windows.Forms.Label
End Class
