<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.cmdCrear = New System.Windows.Forms.Button()
        Me.CmdGestionar = New System.Windows.Forms.Button()
        Me.cmdAjustes = New System.Windows.Forms.Button()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'cmdCrear
        '
        Me.cmdCrear.Location = New System.Drawing.Point(56, 148)
        Me.cmdCrear.Name = "cmdCrear"
        Me.cmdCrear.Size = New System.Drawing.Size(204, 186)
        Me.cmdCrear.TabIndex = 0
        Me.cmdCrear.Text = "CREAR O MODIFICAR" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " CURSOS, MODULOS " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Y" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "FICHAS DE ALUMNOS" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "      O PROFESORES"
        Me.cmdCrear.UseVisualStyleBackColor = True
        '
        'CmdGestionar
        '
        Me.CmdGestionar.Location = New System.Drawing.Point(332, 148)
        Me.CmdGestionar.Name = "CmdGestionar"
        Me.CmdGestionar.Size = New System.Drawing.Size(214, 186)
        Me.CmdGestionar.TabIndex = 1
        Me.CmdGestionar.Text = "GESTIONAR LOS CURSOS" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Y" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "LOS ALUMNOS Y PROFESORES"
        Me.CmdGestionar.UseVisualStyleBackColor = True
        '
        'cmdAjustes
        '
        Me.cmdAjustes.Enabled = False
        Me.cmdAjustes.Location = New System.Drawing.Point(339, 43)
        Me.cmdAjustes.Name = "cmdAjustes"
        Me.cmdAjustes.Size = New System.Drawing.Size(84, 41)
        Me.cmdAjustes.TabIndex = 2
        Me.cmdAjustes.Text = "Entrar en Ajustes"
        Me.cmdAjustes.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(125, 56)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(208, 17)
        Me.CheckBox1.TabIndex = 3
        Me.CheckBox1.Text = "Entrar en ajustes de La Base de Datos"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(651, 449)
        Me.Controls.Add(Me.CheckBox1)
        Me.Controls.Add(Me.cmdAjustes)
        Me.Controls.Add(Me.CmdGestionar)
        Me.Controls.Add(Me.cmdCrear)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdCrear As System.Windows.Forms.Button
    Friend WithEvents CmdGestionar As System.Windows.Forms.Button
    Friend WithEvents cmdAjustes As System.Windows.Forms.Button
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox

End Class
