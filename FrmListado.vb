Imports System.IO
Imports System.Data.SqlClient
Public Class FrmListado
    Public cn As SqlConnection
    Public cat, crit As String
    Dim tipo As Integer
    Dim listaCursos As List(Of Integer)
    Private lvwColumnSorter As ListViewColumnSorter
    Dim listaIDs As List(Of Integer)
    Dim ListaRegistros As List(Of String)
    Public Sub New(ByVal ti As Integer)
        ' Llamada necesaria para el diseñador.
        InitializeComponent()
        tipo = ti
        ' Crear una instancia de una ordenación de columna ListView y asignarla ' al control ListView. 
        lvwColumnSorter = New ListViewColumnSorter()
        Me.ListView1.ListViewItemSorter = lvwColumnSorter
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
    End Sub
    Private Sub FrmListado_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cn = New SqlConnection(ConeStr)
        Me.CmdExportar.Enabled = False
        Call cargarBusquedaUnica()
        Call cargarComboGordo()
        Me.cmdModificar.Text = "Ver / Modificar Ficha"
        Select Case tipo
            Case 1
                cat = "Alumnos"
                Me.GroupBox1.Text = "Listado de Alumnos"

            Case 2
                cat = "Profesores"
                Me.GroupBox1.Text = "Listado de Profesores"
            Case 3
                cat = "Candidatos"
                Me.GroupBox1.Text = "Listado de candidatos"
        End Select
        Me.CboFiltroGordo.SelectedIndex = -1
        PrepararListView()
    End Sub
    Private Sub cargarComboGordo()
        Try
            listaCursos = New List(Of Integer)
            'los ordeno de forma descendente para que salgan primero los ultimos
            Dim sql As String = "SELECT Cursos.Id, Cursos.CodCur FROM CURSOS ORDER BY Cursos.Id DESC"
            Dim cmd As New SqlCommand(sql, cn)
            Dim dr As SqlDataReader
            cn.Open()
            dr = cmd.ExecuteReader
            While dr.Read
                listaCursos.Add(dr(0))
                Me.CboFiltroGordo.Items.Add(dr(1))
            End While
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cn.Close()
        End Try
    End Sub
    Private Sub cargarBusquedaUnica()
        Dim camposFiltroBusquedaUnica() As String = {"DNI", "Nombre", "Apellido1", "Apellido2"}
        For Each s As String In camposFiltroBusquedaUnica
            Me.CboFiltroBusquedaUnica.Items.Add(s)
        Next
    End Sub
    Private Sub PrepararListView()
        Dim columnheader As ColumnHeader
        Me.ListView1.Refresh()
        With Me.ListView1
            .View = View.Details
            .FullRowSelect = True
            .GridLines = True
            .Sorting = SortOrder.Ascending
            .CheckBoxes = False
            .Items.Clear()
            'METER AQUI LOS CAMPOS QUE QUIERAN, POR AHORA LOS TELEFONOS
            Dim camposListview() As String = {"Id", "DNI", "Nombre", "Apellido1", "Apellido2",
                                        "Tel1", "tel2", "Inscrito en el INAEM", "", ""}
            Dim s As String = ""
            For i As Integer = 0 To camposListview.Length - 1
                If i = 0 Then
                    s = camposListview(i).ToString.PadRight(5)
                Else
                    s = camposListview(i).ToString.PadRight(20)
                End If
                columnheader = New ColumnHeader()
                columnheader.Text = s
                Me.ListView1.Columns.Add(columnheader)
            Next
            For Each columnheader In Me.ListView1.Columns
                columnheader.Width = -2
            Next
        End With
        Call cargarDatosEnListview()
    End Sub


    Private Sub cargarDatosEnListview()
        Try
            Me.ListView1.Items.Clear()
            Dim filtrado As Boolean
            Dim sql As String = ""
            Dim idcurso As Integer
            If Me.CboFiltroGordo.SelectedIndex <> -1 Then
                filtrado = True
                idcurso = Me.listaCursos.Item(Me.CboFiltroGordo.SelectedIndex)
            End If
            Dim recorte As String = cat.Substring(0, 2)

            Dim WhereFiltro As String = ""
            Select Case tipo
                Case 1
                    If filtrado = True Then
                        WhereFiltro = String.Format("  AND C.Id={0}", idcurso)
                    Else
                        WhereFiltro = ""
                    End If
                    sql = String.Format("SELECT A.Id,D.Id, D.DNI, D.Nombre, D.Apellido1, D.Apellido2, D.Fnac, D.LugNac, D.Edad, D.Domicilio,D.CP, D.Poblacion, " &
                                                     "D.Tel1, D.Tel2, D.NumSS, D.InInaem, D.InFecha, D.NivelEstudios, D.ExpSector, D.TallaCamiseta, D.TallaPantalon, " &
                                                     "D.TallaZapato, D.Entrevistador, D.FecEntr, D.Valoracion, D.Apto, D.PathFoto, D.Email, D.Comentarios " &
                                                     "FROM Alumnos AS A, DatosPersonales AS D, Alumnos_Cursos, Cursos AS C WHERE D.Id=A.IdDP AND A.Id=Alumnos_Cursos.IdAl AND C.Id=Alumnos_Cursos.IdCur{0}", WhereFiltro)
                Case 2
                    sql = String.Format("SELECT P.Id,D.Id, D.DNI, D.Nombre, D.Apellido1, D.Apellido2, D.Fnac, D.LugNac, D.Edad, D.Domicilio,D.CP, D.Poblacion, " &
                                                        "D.Tel1, D.Tel2, D.NumSS, D.InInaem, D.InFecha, D.NivelEstudios, D.ExpSector, D.TallaCamiseta, D.TallaPantalon, " &
                                                        "D.TallaZapato, D.Entrevistador, D.FecEntr, D.Valoracion, D.Apto, D.PathFoto, D.Email, D.Comentarios " &
                                                        "FROM Profesores AS P, DatosPersonales AS D WHERE D.Id=P.IdDP")
                        Case 3
                            If filtrado = True Then
                                WhereFiltro = String.Format(" AND C.Id={0}", idcurso)
                            Else
                                WhereFiltro = ""
                            End If
                            sql = String.Format("SELECT CA.Id,D.Id, D.DNI, D.Nombre, D.Apellido1, D.Apellido2, D.Fnac, D.LugNac, D.Edad, D.Domicilio,D.CP, D.Poblacion, " &
                                                                "D.Tel1, D.Tel2, D.NumSS, D.InInaem, D.InFecha, D.NivelEstudios, D.ExpSector, D.TallaCamiseta, D.TallaPantalon, " &
                                                                "D.TallaZapato, D.Entrevistador, D.FecEntr, D.Valoracion, D.Apto, D.PathFoto, D.Email, D.Comentarios, CA.EstecTest, " &
                                                                "CA.EstecDinam, CA.EstecEntr, CA.InaemMujer,Ca.InaemDiscap, CA.InaemBajaCon, CA.InaemJoven, CA.InaemOtros " &
                                                                "FROM Candidatos AS CA, DatosPersonales AS D, Candidatos_Cursos, Cursos AS C WHERE D.Id=CA.IdDP AND C.Id=Candidatos_Cursos.IdCur{0}", WhereFiltro)

                    End Select
                    MsgBox(sql)
                    cn.Open()
                    Dim cmd As New SqlCommand(sql, cn)
                    Dim dr As SqlDataReader
                    dr = cmd.ExecuteReader
                    Dim i As Integer = 0
                    While dr.Read
                        Me.ListView1.Items.Add(dr(0))
                        For j As Integer = 1 To dr.FieldCount - 1
                            If j = 7 Then
                                If dr(j).ToString = "True" Then
                                    Me.ListView1.Items(i).SubItems.Add("Sí")
                                End If
                            Else
                                Me.ListView1.Items(i).SubItems.Add(dr(j).ToString)
                            End If
                        Next
                        i += 1
                    End While
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cn.Close()
        End Try
    End Sub
    Private Sub cmdNuevo_Click(sender As Object, e As EventArgs) Handles cmdNuevo.Click
        Try
            If tipo = 1 Then
                MsgBox("No se puede crear una ficha de alumno directamente" & vbCrLf &
                       "Vuelva pulsando el boton salir o la 'X'" & vbCrLf &
                       "Y pulse el boton de crear 'Candidato a Alumno'")
            Else
                Dim dpers As New Ficha
                'en tipo llevo si es alumno, profesor o candidato; true porque es nuevo
                Dim frm As New FrmFichas(dpers, tipo, True)
                If frm.ShowDialog = Windows.Forms.DialogResult.OK Then
                    MsgBox("Insercion en la base de datos Completada")
                    Call cargarDatosEnListview()
                Else
                    Throw New miExcepcion("cancelado a peticion del usuario")
                End If
            End If
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub cmdModificar_Click(sender As Object, e As EventArgs) Handles cmdModificar.Click
        Call AccederFicha()
    End Sub

    Private Sub AccederFicha()  'Lo saco porque se duplica al poder hacerlo tb. desde dobleClick
        Dim aviso As String = cat.Substring(0, cat.Length - 1)
        Try
            If Me.ListView1.SelectedIndices.Count = 0 Then Throw New miExcepcion("Debe seleccionar un elemento del listado")
          
                Dim DP As Ficha = RellenarDatosPersonales()
                If Not IsNothing(DP) Then
                    'en tipo llevo si es alumno, profesor o candidato; false porque es modificacion de uno existente
                    Dim frm As New FrmFichas(DP, tipo, False)

                    If frm.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        Call cargarDatosEnListview()
                    Else
                        Throw New miExcepcion("proceso cancelado")
                    End If
                End If
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub CargarNotas(ByRef fi As Ficha)
        Dim cn2 As New SqlConnection(ConeStr)
        Try
            Dim subconsulta As String = String.Format("(SELECT Candidatos.Id FROM Candidatos, DatosPersonales WHERE Candidatos.IdDP=DatosPersonales.Id AND DatosPersonales.Id={0})", fi.Id)
            Dim sql2 As String = String.Format("Select EstecTest, EstecDinam, EstecEntr, InaemMujer, InaemBajaCon, InaemJoven, InaemDiscap, InaemOtros FROM Candidatos WHERE Id={0}", subconsulta)

            cn2.Open()
            Dim cmd2 As New SqlCommand(sql2, cn2)
            Dim dr2 As SqlDataReader
            dr2 = cmd2.ExecuteReader
            If dr2.Read Then
                With fi
                    If Not IsDBNull(dr2(0)) Then .EstecTest = dr2(0)
                    If Not IsDBNull(dr2(1)) Then .EstecDinam = dr2(1)
                    If Not IsDBNull(dr2(2)) Then .EstecEntr = dr2(2)
                    If Not IsDBNull(dr2(3)) Then .InaemMujer = dr2(3)
                    If Not IsDBNull(dr2(4)) Then .InaemBajaCon = dr2(4)
                    If Not IsDBNull(dr2(5)) Then .InaemJoven = dr2(5)
                    If Not IsDBNull(dr2(6)) Then .InaemDiscap = dr2(6)
                    If Not IsDBNull(dr2(7)) Then .InaemOtros = dr2(7)
                End With
            Else
                '  MsgBox("no hay notas")
            End If
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cn2.Close()
        End Try
    End Sub
    Private Function RellenarDatosPersonales() As Ficha
        Dim FIC As New Ficha
        Try
            cn = New SqlConnection(ConeStr)
            'recupero el id del elemento que quiero modificar a traves del listview
            Dim id As Integer = CInt(Me.ListView1.SelectedItems(0).Text)
            Dim Sql As String
            ' primero los datos personales
            Sql = String.Format("SELECT * FROM DatosPersonales, {0} WHERE DatosPersonales.Id={0}.IdDP and {0}.Id={1}", cat, id)
            ' End If
            cn.Open()
            Dim cmd As New SqlCommand(Sql, cn)
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader
            If dr.Read Then
                With FIC
                    If Not IsNothing(dr(0)) Then .Id = dr(0)
                    If Not IsDBNull(dr(1)) Then .DNI = dr(1)
                    If Not IsDBNull(dr(2)) Then .Nombre = dr(2)
                    If Not IsDBNull(dr(3)) Then .Apellido1 = dr(3)
                    If Not IsDBNull(dr(4)) Then .Apellido2 = dr(4)
                    If Not IsDBNull(dr(5)) Then .Fnac = dr(5)
                    If Not IsDBNull(dr(6)) Then .LugNac = dr(6)
                    If Not IsDBNull(dr(7)) Then .Edad = dr(7)
                    If Not IsDBNull(dr(8)) Then .Domicilio = dr(8)
                    If Not IsDBNull(dr(9)) Then .CP = dr(9)
                    If Not IsDBNull(dr(10)) Then .Poblacion = dr(10)
                    If Not IsDBNull(dr(11)) Then .Tel1 = dr(11)
                    If Not IsDBNull(dr(12)) Then .Tel2 = dr(12)
                    If Not IsDBNull(dr(13)) Then .NumSS = dr(13)
                    If Not IsDBNull(dr(14)) Then .InInaem = dr(14)
                    If Not IsDBNull(dr(15)) Then .InFecha = dr(15)
                    If Not IsDBNull(dr(16)) Then .NivelEstudios = dr(16)
                    If Not IsDBNull(dr(17)) Then .ExpSector = dr(17)
                    If Not IsDBNull(dr(18)) Then .TallaCamiseta = dr(18)
                    If Not IsDBNull(dr(19)) Then .TallaPantalon = dr(19)
                    If Not IsDBNull(dr(20)) Then .TallaZapato = dr(20)
                    If Not IsDBNull(dr(21)) Then .Entrevistador = dr(21)
                    If Not IsDBNull(dr(22)) Then .FecEntr = dr(22)
                    If Not IsDBNull(dr(23)) Then .Valoracion = dr(23)
                    '#### OJO Aqui con lode apto
                    If Not IsDBNull(dr(24)) Then .Apto = dr(24)
                    '#####
                    If Not IsDBNull(dr(25)) Then .PathFoto = dr(25)
                    If Not IsDBNull(dr(26)) Then .Email = dr(26)
                    If Not IsDBNull(dr(27)) Then .Comentarios = dr(27)
                    If Not IsNothing(dr(28)) Then .Curso = dr(28)
                    Call CargarNotas(FIC)
                    .cargarlistas()
                End With
            End If
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
            FIC = Nothing
        Catch ex As Exception
            MsgBox(ex.ToString)
            FIC = Nothing
        Finally
            cn.Close()
        End Try
        Return FIC
    End Function
  
    Private Sub cmdSalir_Click(sender As Object, e As EventArgs) Handles cmdSalir.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Public Function borrarDatosPersonales(ByVal i As Integer) As Integer
        Dim num, idDP As Integer
        Dim sqlIdDP As String = String.Format("SELECT DatosPersonales.Id FROM DatosPersonales, {0} WHERE DatosPersonales.Id={0}.IdDP AND {0}.id={1}", cat, CStr(i))
        Dim sqlalumnos As String = String.Format("DELETE FROM {0} WHERE {0}.id={1}", cat, CStr(i))
        Dim sqlDatosPersonales As String = "DELETE FROM DatosPersonales WHERE DatosPersonales.Id="
        Dim cn2 As New SqlConnection(ConeStr)
        Try
            cn.Open()
            Dim cmd1, cmd2, cmd3 As SqlCommand
            cmd1 = New SqlCommand(sqlIdDP, cn)
            idDP = cmd1.ExecuteScalar
            cn.Close()
            cn.Open()
            cmd2 = New SqlCommand(sqlalumnos, cn)
            num = cmd2.ExecuteNonQuery
            If num <> 1 Then Throw New miExcepcion("error al borrar")
            cn2.Open()
            sqlDatosPersonales &= CStr(idDP) ' Añado la Id obtenida al final de la consulta
            cmd3 = New SqlCommand(sqlDatosPersonales, cn2)
            num = cmd3.ExecuteNonQuery
            If num <> 1 Then Throw New miExcepcion("Error al borrar datos personales")
            'End If
        Catch ex2 As miExcepcion
            num = -1
        Catch ex As Exception
            num = -1
            MsgBox(ex.ToString)
        Finally
            cn.Close()
            cn2.Close()
        End Try
        Return num
    End Function
    Private Sub cmdBorrar_Click(sender As Object, e As EventArgs) Handles cmdBorrar.Click
        Try
            Dim respuesta1, respuesta2 As MsgBoxResult
            Dim nombre As String = ""
            Dim id As Integer = CInt(Me.ListView1.SelectedItems(0).Text)
            If Me.ListView1.SelectedItems.Count = 0 Then
                MsgBox("Debe seleccionar el elemento a borrar")
            Else
                For i As Integer = 2 To 4
                    nombre &= " " & Me.ListView1.SelectedItems.Item(0).SubItems(i).Text
                Next
                respuesta1 = MsgBox(String.Format("Ha seleccionado el elemento: ' {0} ' para ser borrado" & vbCrLf & "¿Está seguro?", nombre), MsgBoxStyle.YesNo)
                If respuesta1 = MsgBoxResult.No Then Throw New miExcepcion("Borrado cancelado a peticion del usuario")
                respuesta2 = MsgBox("¿Seguro que desea continuar?" & vbCrLf & "Una vez borrado no se puede recuperar", MsgBoxStyle.YesNo)
                If respuesta2 = MsgBoxResult.No Then Throw New miExcepcion("Borrado cancelado a peticion del usuario")
                Dim resultadoBorrar As Integer = borrarDatosPersonales(id)
                If resultadoBorrar = -1 Then Throw New miExcepcion("Error al borrar")
                MsgBox("Datos borrados con éxito")
                Call cargarDatosEnListview()
            End If
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Sub
    Private Sub cmdBuscar_Click(sender As Object, e As EventArgs) Handles cmdBuscar.Click
        If Me.CboFiltroBusquedaUnica.SelectedIndex = -1 Then
            MsgBox(" Seleccione un criterio de busqueda del combo")
        Else
            Dim pos As Integer = -1
            Dim encontrado As Boolean = False
            For i As Integer = 0 To Me.ListView1.Items.Count - 1
                If Me.ListView1.Items(i).SubItems(Me.CboFiltroBusquedaUnica.SelectedIndex + 1).Text = Me.TxtCampo.Text Then
                    encontrado = True
                    pos = i
                    Exit For
                End If
            Next
            If pos = -1 Then ' No lo ha encontrado
                MsgBox(String.Format("El {0} a buscar no se encuentra en el listado", Me.CboFiltroBusquedaUnica.SelectedItem.ToString))
                Me.TxtCampo.Focus()
                Me.TxtCampo.SelectAll()
            Else 'Lo ha encontrado
                Me.ListView1.Focus()
                Me.ListView1.Items.Item(pos).Selected = True
                Me.ListView1.SelectedItems.Item(0).Focused = True
                If Not IsNothing(Me.ListView1.FocusedItem) Then
                    Me.ListView1.FocusedItem.EnsureVisible()
                    Me.ListView1.SelectedItems.Item(0).Checked = True
                End If
                'Limpio el combo y el campo
                Me.CboFiltroBusquedaUnica.SelectedIndex = -1
                Me.TxtCampo.Text = ""
            End If
        End If
    End Sub
  
    Private Sub ChkExportar_Click(sender As Object, e As EventArgs) Handles ChkExportar.Click
        If ChkExportar.Checked = True Then
            Me.ListView1.MultiSelect = True
            Me.CmdExportar.Enabled = True
            MsgBox("Clique una vez en el listado sobre la ficha quiera exportar" & vbCrLf &
        "Cuando Cuando los haya seleccionado" & vbCrLf &
        " pulse 'Exportar Datos'")
        Else
            Me.ListView1.MultiSelect = False
            Me.ListView1.CheckBoxes = False
            Me.CmdExportar.Enabled = False
            Me.listaIDs = Nothing
            Me.ListaRegistros = Nothing
        End If
    End Sub
    Private Sub cmdFiltrar_Click(sender As Object, e As EventArgs) Handles cmdFiltrar.Click
        Call cargarDatosEnListview()
    End Sub

    Private Sub cmdQuitarFiltro_Click(sender As Object, e As EventArgs) Handles cmdQuitarFiltro.Click
        Me.CboFiltroGordo.SelectedIndex = -1
        Call cargarDatosEnListview()
    End Sub
    Private Sub ListView1_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles ListView1.ColumnClick
        ' Determinar si la columna en la que se hizo clic ya es la que se está ordenando. 
        If (e.Column = lvwColumnSorter.SortColumn) Then
            ' Revertir la dirección de ordenación actual de esta columna.
            If (lvwColumnSorter.Order = SortOrder.Ascending) Then
                lvwColumnSorter.Order = SortOrder.Descending
            Else
                lvwColumnSorter.Order = SortOrder.Ascending
            End If
        Else
            ' Establecer el número de columna que se va a ordenar; de forma predeterminada, en orden ascendente.
            lvwColumnSorter.SortColumn = e.Column
            lvwColumnSorter.Order = SortOrder.Ascending
        End If
        ' Realizar la ordenación con estas nuevas opciones de ordenación. 
        Me.ListView1.Sort()
    End Sub
    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        Call AccederFicha()
    End Sub
    Private Sub ChkExportar_Validated(sender As Object, e As EventArgs) Handles ChkExportar.Validated
        listaIDs = Nothing
        listaIDs = New List(Of Integer)
        ListaRegistros = Nothing
        ListaRegistros = New List(Of String)
        Me.ListView1.CheckBoxes = True
    End Sub
    Private Function encontrarLasId() As List(Of Integer)
        Dim lId As New List(Of Integer)
        Dim pos As Integer = 0
        For Each it As ListViewItem In Me.ListView1.Items
            If it.Checked = True Then
                pos = CInt(it.Index.ToString)
                lId.Add(CInt(Me.ListView1.Items(pos).SubItems(0).Text))
            End If
        Next
        Return lId
    End Function
    Private Function cargarencabezados() As String
        Dim s As String = ""
        Dim f As Ficha = New Ficha
        f.cargarlistas()
        Dim limite As Integer
        Select Case tipo
            Case 3
                limite = 38

            Case Else
                limite = 28
        End Select
        For i As Integer = 0 To limite
            s &= String.Format("; {0}", f.listadoNombres(i))
        Next
        s = s.Substring(2)
        Return s
    End Function

 
    Private Function recogerDatos(ByVal id As Integer) As String
        Dim registro As String = ""
        Try
            Dim sql As String = ""


            Select Case tipo
                Case 1
                    sql = String.Format("SELECT D.Id, D.DNI, D.Nombre, D.Apellido1, D.Apellido2, D.Fnac, D.LugNac, D.Edad, D.Domicilio,D.CP, D.Poblacion, " &
                                        "D.Tel1, D.Tel2, D.NumSS, D.InInaem, D.InFecha, D.NivelEstudios, D.ExpSector, D.TallaCamiseta, D.TallaPantalon, " &
                                        "D.TallaZapato, D.Entrevistador, D.FecEntr, D.Valoracion, D.Apto, D.PathFoto, D.Email, D.Comentarios " &
                                        "FROM DatosPersonales as D INNER JOIN Alumnos ON D.id=Alumnos.IdDP WHERE Alumnos.Id={0}", id)
                Case 2
                    sql = String.Format("SELECT D.Id, D.DNI, D.Nombre, D.Apellido1, D.Apellido2, D.Fnac, D.LugNac, D.Edad, D.Domicilio,D.CP, D.Poblacion, " &
                                        "D.Tel1, D.Tel2, D.NumSS, D.InInaem, D.InFecha, D.NivelEstudios, D.ExpSector, D.TallaCamiseta, D.TallaPantalon, " &
                                        "D.TallaZapato, D.Entrevistador, D.FecEntr, D.Valoracion, D.Apto, D.PathFoto, D.Email, D.Comentarios " &
                                        "FROM DatosPersonales as D INNER JOIN Profesores ON D.id=Profesores.IdDP WHERE Profesores.Id={0}", id)
                Case 3
                    sql = String.Format("SELECT D.Id, D.DNI, D.Nombre, D.Apellido1, D.Apellido2, D.Fnac, D.LugNac, D.Edad, D.Domicilio,D.CP, D.Poblacion, " &
                                        "D.Tel1, D.Tel2, D.NumSS, D.InInaem, D.InFecha, D.NivelEstudios, D.ExpSector, D.TallaCamiseta, D.TallaPantalon, " &
                                        "D.TallaZapato, D.Entrevistador, D.FecEntr, D.Valoracion, D.Apto, D.PathFoto, D.Email, D.Comentarios, " &
                                        "C.EstecTest, C.EstecDinam, C.EstecEntr, C.InaemMujer, C.InaemDiscap, C.InaemBajaCon, C.InaemJoven, C.InaemOtros " &
                                        "FROM DatosPersonales AS D INNER JOIN Candidatos as C ON D.Id=C.IdDP WHERE C.Id={0}", id)
            End Select
            cn.Open()
            Dim cmd As New SqlCommand(sql, cn)
            Dim dr As SqlDataReader
            dr = cmd.ExecuteReader
            If dr.Read Then

                For i As Integer = 0 To dr.FieldCount - 1
                    If Not IsDBNull(dr(i)) Then
                        registro &= String.Format(";{0}", dr(i))
                    Else
                        registro &= String.Format(";{0}", "NULL")
                    End If
                Next
            Else
                Throw New miExcepcion(String.Format("La ficha '{0}' no se puede cargar", CStr(id)))
            End If
            registro = registro.Substring(2)

        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
            registro = ""
        Catch ex As Exception
            MsgBox(ex.ToString)
            registro = ""
        Finally
            cn.Close()
        End Try
        Return registro
    End Function

    Private Sub ChkExportar_CheckedChanged(sender As Object, e As EventArgs) Handles ChkExportar.CheckedChanged

    End Sub

    Private Sub CmdExportar_Click(sender As Object, e As EventArgs) Handles CmdExportar.Click
        listaIDs = encontrarLasId()
        Dim listaerrores As New List(Of Integer)
        Try
            Dim res As MsgBoxResult

            Dim encabezados As String = cargarencabezados()

            Dim reg As String = ""
            ListaRegistros = New List(Of String)
            ListaRegistros.Add(encabezados)
            For i As Integer = 0 To listaIDs.Count - 1
                'la i no, capullo listaIDs(i)
                reg = recogerDatos(listaIDs(i))
                If reg <> "" Then
                    ListaRegistros.Add(reg)
                Else
                    listaerrores.Add(i)
                End If
                reg = ""
            Next

            Dim output1 As New StreamWriter(PathExportacion, False)
            'escribo el array de autores con un simbolo distinto en cada tipo de dato
            For Each str As String In ListaRegistros
                output1.WriteLine(str)
            Next
            output1.Close()
            Dim myStream As Stream
            MsgBox(String.Format("Archivo creado en '{0}'" & vbCrLf &
                                 "Podrá abrirlo desde el menú que se desplegará a continuación" & vbCrLf &
                                 "Se abre como solo lectura. Luego recuerde guardarlo en un formato excel " &
                                 vbCrLf & "como '.xls'", PathExportacion))
           
            Dim openFileDialog1 As New OpenFileDialog()
            openFileDialog1.InitialDirectory = "C:\Git\DatosExportados"
            openFileDialog1.Filter = "csv files (*.csv)|*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*"
            'openFileDialog1.FilterIndex = 2
            'openFileDialog1.RestoreDirectory = True

            'Dim oDocument As Object
            'Dim sFileName As String
            If openFileDialog1.ShowDialog() = DialogResult.OK Then
                myStream = openFileDialog1.OpenFile()
                If (myStream IsNot Nothing) Then
                    ' Insert code to read the stream here.
                    Dim LibroTrabajo As Object
                    Dim Fichero As String
                    Fichero = "C:\Git\DatosExportados\RegistrosExportados.csv" 'con el path correspondiente 
                    LibroTrabajo = GetObject(Fichero)
                    LibroTrabajo.Application.Windows("RegistrosExportados.csv").Visible = True
                    '  MsgBox("A rezar")
                End If
            Else
                '  MsgBox("y yo que sé")
            End If
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            Me.ChkExportar.Checked = False
            Me.CmdExportar.Enabled = False
            Call PrepararListView()
        End Try
    End Sub
End Class