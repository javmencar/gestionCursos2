Imports System.Data.SqlClient
Public Class FrmFichas
    Dim nuevo, fotoCambiada As Boolean
    Public DP As Ficha
    Dim tipo As Integer
    Public cat As String
    Public cn As SqlConnection
    Dim NuIdDP, idcur As Integer
    Dim GuardaComentarios As String
    Dim Funciones1 As Funciones
    Sub New(ByVal Da As Ficha, ByVal ti As Integer, ByVal nw As Boolean)
        ' Llamada necesaria para el diseñador.
        InitializeComponent()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        nuevo = nw
        tipo = ti
        DP = Da
        Select Case tipo
            Case 1
                cat = "Alumnos"
            Case 2
                cat = "Profesores"
            Case 3
                cat = "Candidatos"
        End Select
    End Sub

    Private Sub FrmFichas_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Funciones1 = New Funciones()
        cn = New SqlConnection(ConeStr)
        Me.lblComentariosEscritos.Text = ""
        Me.lblCurso.Text = ""
        Me.txtId.Enabled = False
        Me.LstExpSector.Items.Clear()
        Me.CboExpSector.SelectedIndex = -1
        Me.LstExpSector.Enabled = True
        NuIdDP = -1
        Me.cmdAñadirAAlumnos.Visible = False
        Me.lblComentariosEscritos.Enabled = False
        If tipo = 3 Then Me.GbCalificacion.Visible = True Else  Me.GbCalificacion.Visible = False
        Call cargarcomboCursos()
        If nuevo = True Then
            DP = New Ficha
            Me.cboCursos.Enabled = True
            Me.cmdModificar.Text = "CREAR NUEVA FICHA"
            Me.cmdCancelar.Text = "Cancelar La Creación"
            Me.cmdCambiarFoto.Text = "Insertar Foto"
            Me.PicBx1.Image = Image.FromFile("C:\GIT\GestionCursos1\Resources\female-silhouette_0.jpg")
            Me.PicBx1.Tag = "C:\GIT\GestionCursos1\Resources\female-silhouette_0.jpg"
            Me.txtFNac.Text = "01/01/1900"
            Me.txtFecEntr.Text = "01/01/1900"
            Me.txtInFecha.Text = "01/01/1900"
            Me.OptAptoPendiente.Select()
            Me.cboCursos.Enabled = True
            Me.GbCalificacion.Enabled = False
            Me.GbCalificacion.Visible = False
        Else
            Me.cboCursos.Enabled = False
            Me.cmdModificar.Text = "GUARDAR LA FICHA"
            Me.cmdCancelar.Text = "Cancelar La Modificación"
            Me.cmdCambiarFoto.Text = "Cambiar Foto"
            Me.cmdBorrar.Visible = True
            Me.cmdBorrar.Enabled = True
            Me.GbCalificacion.Enabled = True
            Me.GbCalificacion.Visible = True
            Call rellenarCamposDesdeObjeto(DP)
        End If
    End Sub
    Private Sub cargarcomboCursos()
        Try
            Dim sql As String = "select cursos.Id, cursos.CodCur, cursos.nombre from Cursos ORDER BY Cursos.Id DESC"
            cn.Open()
            Dim cmd As New SqlCommand(sql, cn)
            Dim dr As SqlDataReader
            If nuevo = True Then ' si es nueva ficha, simplemente cargo el combo para poder elegir
                dr = cmd.ExecuteReader
                While dr.Read
                    Me.cboCursos.Items.Add(String.Format("{0}_{1}_{2}", dr(0), dr(1), dr(2)))
                End While
            Else
                dr = cmd.ExecuteReader
                While dr.Read
                    Me.cboCursos.Items.Add(String.Format("{0}_{1}_{2}", dr(0), dr(1), dr(2)))
                    If dr(0) = DP.Curso Then
                        idcur = dr(0)
                        Me.lblNombreCurso.Text = dr(2)
                    End If
                End While
            End If
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cn.Close()
        End Try
    End Sub
    Private Sub rellenarCamposDesdeObjeto(ByVal Datos As Ficha)
        With Datos
            Me.txtId.Text = CStr(.Id)
            Me.txtApellido1.Text = .Apellido1
            Me.txtApellido2.Text = .Apellido2
            Me.txtNombre.Text = .Nombre
            Me.txtDNI.Text = .DNI
            Me.txtNumSS.Text = .NumSS
            If .Fnac <> "1/1/1900" Then Me.txtFNac.Text = .Fnac  Else  Me.txtFNac.Text = "1/1/1900"
                Me.txtLugNac.Text = .LugNac
                Me.txtEdad.Text = CStr(.Edad)
                Me.txtTel1.Text = .Tel1
                Me.txtTel2.Text = .Tel2
                Me.txtDomicilio.Text = .Domicilio
                Me.txtCP.Text = .CP
                Me.txtPoblacion.Text = .Poblacion
                If .InInaem = True Then
                    Me.optInaemSi.Select()
                Else
                    Me.OptInaemNo.Select()
                End If
                If .InFecha <> "1/1/1900" Then Me.txtInFecha.Text = .InFecha Else Me.txtInFecha.Text = "1/1/1900"
            If Not IsNothing(.NivelEstudios) Then Me.txtNivelEstudios.Text = Funciones1.MeterSaltosDeLinea(.NivelEstudios)
                'hago una matriz con la string de experiencia y la vuelco en el listbox
                'controlo si hay algo en el string
                Me.LstExpSector.Items.Clear()
                If Not IsNothing(.ExpSector) Then
                    Dim sectores() As String = .ExpSector.Split("/")
                    For Each s As String In sectores
                        Me.LstExpSector.Items.Add(s)
                    Next
                End If
                Me.CboTallaCamiseta.SelectedItem = .TallaCamiseta
                Me.CboTallaPantalon.SelectedItem = .TallaPantalon
                Me.txtTallaCalzado.Text = CStr(.TallaZapato)
                Me.txtEntrevistador.Text = .Entrevistador
                If .FecEntr <> "1/1/1900" Then Me.txtFecEntr.Text = .FecEntr Else Me.txtFecEntr.Text = "1/1/1900"
            If Not IsNothing(.Valoracion) Then Me.txtValoracion.Text = Funciones1.MeterSaltosDeLinea(.Valoracion)
                '######
                'Posiblemente tenga que sacar esto de aqui a un sub para tener en cuenta las notas
            If tipo = 3 Then
                Me.GbRdBtCalificaciones.Visible = True
                If Not IsNothing(.Apto) Then

                    Select Case .Apto
                        Case "Apto"
                            Me.optAptoSi.Select()
                            Me.cmdAñadirAAlumnos.Visible = True
                            If nuevo = True Then
                                Me.cmdAñadirAAlumnos.Enabled = True
                            End If
                        Case "No Apto"
                            Me.OptAptoNo.Select()
                        Case "Pendiente"
                            Me.OptAptoPendiente.Select()
                            Me.cmdAñadirAAlumnos.Visible = True
                            Me.cmdAñadirAAlumnos.Enabled = False
                    End Select
                Else
                    Me.OptAptoPendiente.Select()
                End If
            Else
                Me.GbRdBtCalificaciones.Visible = False
            End If
            '#############
            If Not IsNothing(.PathFoto) Then
                Me.PicBx1.ImageLocation = .PathFoto
                Me.PicBx1.Show()
                Me.PicBx1.Tag = .PathFoto
            Else
                Me.PicBx1.Image = Image.FromFile("C:\GIT\GestionCursos1\Resources\female-silhouette_0.jpg")
                Me.PicBx1.Tag = "C:\GIT\GestionCursos1\Resources\female-silhouette_0.jpg"
            End If
            Me.txtEmail.Text = .Email
            If Not IsNothing(.Comentarios) Then
                Me.lblComentarios.Text = "HAY COMENTARIOS"
                Me.lblComentarios.BackColor = Color.Red
                Me.cmdAñadirComentarios.Text = "Acceder a Comentarios"
                GuardaComentarios = Funciones1.MeterSaltosDeLinea(.Comentarios)
            End If
            If Not IsNothing(.Curso) Then
                Me.lblNombreCurso.Text = .Curso
                Dim aux(3) As String
                For i As Integer = 0 To Me.cboCursos.Items.Count - 1
                    aux = Me.cboCursos.Items.Item(i).ToString.Split("_")
                    If aux(0) = .Curso Then
                        Me.lblNombreCurso.Text = aux(2)
                        Exit For
                    End If
                Next
            End If
        End With
        Call rellenarNotas(Datos)
    End Sub
    Private Sub rellenarNotas(ByVal CA As Ficha)
        With CA
            If Not IsNothing(.EstecTest) Then Me.MtxtEstecTest.Text = .EstecTest Else Me.MtxtEstecTest.Text = "00.00"
            If Not IsNothing(.EstecDinam) Then Me.MtxtEstecDinam.Text = .EstecDinam Else  : Me.MtxtEstecDinam.Text = "00.00"
            If Not IsNothing(.EstecEntr) Then Me.MtxtEstecEntr.Text = .EstecEntr Else  : Me.MtxtEstecEntr.Text = "00.00"
            If Not IsNothing(.notaEstecform) Then Me.MtxtEstecNOTA.Text = .notaEstecform Else Me.MtxtEstecNOTA.Text = "00.00"
            If Not IsNothing(.InaemMujer) Then Me.MtxtInaemMujer.Text = .InaemMujer Else  : Me.MtxtInaemMujer.Text = "00.00"
            If Not IsNothing(.InaemDiscap) Then Me.MtxtInaemDiscap.Text = .InaemDiscap Else Me.MtxtInaemDiscap.Text = "00.00"
            If Not IsNothing(.InaemBajaCon) Then Me.MtxtInaemBajaContr.Text = .InaemBajaCon Else Me.MtxtInaemBajaContr.Text = "00.00"
            If Not IsNothing(.InaemJoven) Then Me.MtxtInaemJoven.Text = .InaemJoven Else Me.MtxtInaemJoven.Text = "00.00"
            If Not IsNothing(.InaemOtros) Then Me.MtxtInaemOtros.Text = .InaemOtros Else Me.MtxtInaemOtros.Text = "00.00"
            If Not IsNothing(.InaemMujer) Then Me.MtxtInaemMujer.Text = .InaemMujer Else Me.MtxtInaemMujer.Text = "00.00"
            If Not IsNothing(.notaINAEM) Then Me.MtxtInaemNOTA.Text = .notaINAEM Else Me.MtxtInaemNOTA.Text = "00.00"
            'y aqui meter lo de apto y no apto
        End With
    End Sub
    Private Function EstaEnTablas(ByVal i As String) As Boolean
        Try
            Dim sql As String = String.Format("SELECT {0}.Id FROM DatosPersonales, {0} " &
                                              "WHERE DatosPersonales.Id={0}.IdDP AND DatosPersonales.Id={1}", cat, i)
            Dim cmd As SqlCommand
            cn.Open()
            cmd = New SqlCommand(sql, cn)
            Dim j As Integer = cmd.ExecuteScalar
            If j > 0 Then Return True
        Catch ex As Exception
            Return False
        End Try
        Return False
    End Function
    'Private Function QuitarSaltosDeLinea(ByVal str As String) As String
    '    str = str.Replace(vbCrLf, "#")
    '    Return str
    'End Function
    'Private Function MeterSaltosDeLinea(ByVal str As String) As String
    '    str = str.Replace("#", vbCrLf)
    '    Return str
    'End Function
    Private Function rellenarObjetoDesdeCampos() As Ficha
        Dim D1 As New Ficha
        Try
            Dim reparador As String = ""
            With D1
                If Me.txtId.Text <> "" Then
                    .Id = Me.txtId.Text
                End If
                .Apellido1 = Me.txtApellido1.Text
                .Apellido2 = Me.txtApellido2.Text
                .Nombre = Me.txtNombre.Text
                .DNI = Me.txtDNI.Text
                .NumSS = Me.txtNumSS.Text.Replace("/", "")
                Dim err As Integer = 0
                err = Funciones1.comprobarformatofecha(Me.txtFNac.Text)
                ' err = comprobarformatofecha(Me.txtFNac.Text)
                If err <> 0 Then
                    MsgBox("Fecha de nacimiento incorrecta." & vbCrLf & "Se cargará 01/01/1900")
                    .Fnac = "01/01/1900"
                Else
                    .Fnac = Me.txtFNac.Text
                End If
                .LugNac = Me.txtLugNac.Text
                If Me.txtEdad.Text = "" Then Me.txtEdad.Text = "0"
                .Edad = CInt(Me.txtEdad.Text)
                reparador = Me.txtTel1.Text.Replace("-", "")
                .Tel1 = reparador
                reparador = Me.txtTel2.Text.Replace("-", "")
                .Tel2 = reparador
                .Domicilio = Me.txtDomicilio.Text
                .CP = Me.txtCP.Text
                .Poblacion = Me.txtPoblacion.Text
                If Me.optInaemSi.Checked = True Then
                    'ojo, la propiedad alumno.Ininaem es un string
                    .InInaem = "True"
                    err = Funciones1.comprobarformatofecha(Me.txtInFecha.Text)
                    ' err = comprobarformatofecha(Me.txtInFecha.Text)
                    If err <> 0 Then
                        MsgBox("Fecha de nacimiento incorrecta." & vbCrLf & "Se cargará 01/01/1900")
                        .Fnac = "01/01/1900"
                    Else
                        .InFecha = Me.txtInFecha.Text
                    End If
                Else
                    .InInaem = "False"
                End If
                If Me.txtNivelEstudios.Text <> "" Then .NivelEstudios = Funciones1.QuitarSaltosDeLinea(Me.txtNivelEstudios.Text)
                    Dim expSect1 As String = ""
                    If Me.LstExpSector.Items.Count > 0 Then
                        For Each l As String In Me.LstExpSector.Items
                            expSect1 &= String.Format("/{0}", l)
                        Next
                        expSect1 = expSect1.Substring(1)
                    End If
                    .ExpSector = expSect1
                    If Me.CboTallaCamiseta.SelectedIndex <> -1 Then
                        .TallaCamiseta = Me.CboTallaCamiseta.SelectedItem.ToString
                    End If
                    If Me.CboTallaPantalon.SelectedIndex <> -1 Then
                        .TallaPantalon = Me.CboTallaPantalon.SelectedItem.ToString
                    End If
                    If Me.txtTallaCalzado.Text = "" Then Me.txtTallaCalzado.Text = "0"
                    .TallaZapato = CInt(Me.txtTallaCalzado.Text)
                    .Entrevistador = Me.txtEntrevistador.Text
                If Me.txtEntrevistador.Text <> "" Or Me.txtValoracion.Text <> "" Then
                    err = Funciones1.comprobarformatofecha(Me.txtFecEntr.Text)
                    ' err = comprobarformatofecha(Me.txtFecEntr.Text)
                    If err <> 0 Then
                        MsgBox("Fecha de nacimiento incorrecta." & vbCrLf & "Se cargará 01/01/1900")
                        .Fnac = "01/01/1900"
                    Else
                        .FecEntr = Me.txtFecEntr.Text
                    End If
                    If Not IsNothing(.Valoracion) Then .Valoracion = Funciones1.QuitarSaltosDeLinea(Me.txtValoracion.Text)
                    '###
                    'Esto habrá que tocarlo
                    If Me.optAptoSi.Checked = True Then
                        .Apto = "Apto"
                    ElseIf Me.OptAptoNo.Checked = True Then
                        .Apto = "No Apto"
                    Else
                        .Apto = "Pendiente"
                    End If
                    '####
                Else
                End If
                    .PathFoto = Me.PicBx1.Tag
                .Email = Me.txtEmail.Text
                If GuardaComentarios <> "" Then .Comentarios = Funciones1.QuitarSaltosDeLinea(GuardaComentarios)
                    .Curso = idcur
                    If nuevo = False Then
                        .EstecTest = Me.MtxtEstecTest.Text
                        .EstecDinam = Me.MtxtEstecDinam.Text
                        .EstecEntr = Me.MtxtEstecEntr.Text
                        .InaemMujer = Me.MtxtInaemMujer.Text
                        .InaemDiscap = Me.MtxtInaemDiscap.Text
                        .InaemJoven = Me.MtxtInaemJoven.Text
                        .InaemBajaCon = Me.MtxtInaemBajaContr.Text
                        .InaemOtros = Me.MtxtInaemOtros.Text
                    End If

                    .cargarlistas() 'Llamo al procedimiento de la clase que carga listadoNombres y listavalores
            End With
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
            D1 = Nothing
        Catch ex As Exception
            MsgBox(ex.ToString)
            D1 = Nothing
        End Try
        Return D1
    End Function
    Private Sub cmdModificar_Click(sender As Object, e As EventArgs) Handles cmdModificar.Click
        Call ModificarOCrear()
    End Sub
    Private Sub ModificarOCrear()
        'Lo he metido en un sub  porque se repite todo en añadir a alumnos
        Try
            Dim comprobado As Boolean
            Dim FichaRellenada As Ficha
            Dim fallos As List(Of String)
            If nuevo = True Then
                fallos = camposvacios()
                'ademas de campos vacios , quiero que compruebe el DNI y el NumSS
                If tipo = 3 Then
                    If Me.cboCursos.SelectedIndex = -1 Then Throw New miExcepcion("Debe elegir el curso al que se va a apuntar el alumno")
                End If
                If Me.txtDNI.Text <> "" Then comprobado = Funciones1.ValidaNif(Me.txtDNI.Text)
                If comprobado = False Then fallos.Add("El DNI introducido NO es válido.")
            Dim numSSSinBarras As String = Me.txtNumSS.Text.Replace("/", "")
            If numSSSinBarras <> "" Then
                comprobado = Funciones1.ValidaNumSS(numSSSinBarras)
                If comprobado = False Then
                    fallos.Add("El Numero de la Seguridad Social NO es válido")
                End If
            End If
            'Hasta aqui las validaciones del DNI y NumSS
            Else
            fallos = fallosEnCampos()
            End If
            If fallos.Count > 0 Then
                Dim respuesta As MsgBoxResult
                Dim recogefallos As String = ""
                For Each s As String In fallos
                    recogefallos &= String.Format(" '{0}'" & vbCrLf, s)
                Next
                respuesta = MsgBox("Está creando una ficha con estas incidencias: " & vbCrLf & recogefallos &
                            vbCrLf & "¿Seguro que desea seguir?", MsgBoxStyle.YesNo)
                If respuesta = MsgBoxResult.No Then Throw New miExcepcion("Operación cancelada a peticion del usuario")
            End If
            'aceptado seguir con los fallos (o bien si no los había) continuamos
            FichaRellenada = rellenarObjetoDesdeCampos()
            If Not IsNothing(FichaRellenada) Then
                If nuevo = True Then ' creo una ficha, porque es nuevo
                    Dim comprobacion As Boolean = CrearNuevoDPEnBaseDeDatos(FichaRellenada)
                    If comprobacion = False Then Throw New miExcepcion("Error al cargar los datos personales de la ficha")
                    NuIdDP = Funciones1.ejecutarConsultaScalar("SELECT TOP 1 DatosPersonales.Id FROM DatosPersonales ORDER BY Id DESC")
                    If NuIdDP = -1 Then Throw New miExcepcion("Error al calcular la ultima ID")
                    'meto la id en el objeto ficha, porque luego lo voy a utilizar
                    FichaRellenada.Id = NuIdDP
                    ' con esto inserto en la tabla alumnos, profesores o candidatos
                    Dim comprob As Integer = Funciones1.ejecutarConsultaNonQuery(String.Format("INSERT INTO {0} ({0}.idDP) VALUES ({1})", cat, NuIdDP))
                    '  Dim comprob As Integer =  comprob = ejecutarConsultaNonQuery(String.Format("INSERT INTO {0} ({0}.idDP) VALUES ({1})", cat, NuIdDP))
                    If comprob = -1 Then Throw New miExcepcion(String.Format("Problema al insertar en {0}", cat))
                    'y ahora inserto en la tabla cursos y secundarias
                    Dim idt, idc As Integer
                    idt = Funciones1.ejecutarConsultaScalar(String.Format("SELECT TOP 1 {0}.Id FROM {0} ORDER BY Id DESC", cat))
                    idc = FichaRellenada.Curso
                    comprob = 0
                    Dim cortado As String = cat.Substring(0, 2)
                    comprob = Funciones1.ejecutarConsultaNonQuery(String.Format("INSERT INTO {0}_Cursos (Id{1},IdCur) VALUES ({2},{3})", cat, cortado, idt, idc))
                Else    ' no hace falta insertar en BBDD, ya está en ambas tablas
                    'cargo los datos del objeto ya creado
                    Dim comprobacion As Boolean = cargarCambiosEnDPYaCreado(FichaRellenada)
                    If comprobacion = False Then Throw New miExcepcion("Error al cargar los datos personales de la ficha")
                End If
                'con la ficha creada o modificada, meto las notas , si las hay y el curso
                If FichaRellenada.notaEstecform <> "0.00" Or FichaRellenada.notaINAEM <> "0.00" Then
                    Dim comprobaNotas As Integer = cargarNotas(FichaRellenada)
                    'que solo compruebe errores en los candidatos
                    If tipo = 3 AndAlso comprobaNotas <= 0 Then Throw New miExcepcion("Error al meter las notas")
                End If
            Else ' si no hay nada en el objeto es que ha habido error al crearlo
                If nuevo = True Then
                    Throw New miExcepcion("Cambie los campos necesarios para poder Crear la ficha" & vbCrLf & " o Pulse Salir")
                Else
                    Throw New miExcepcion("Cambie los campos necesarios para poder Modificar la ficha" & vbCrLf & " o Pulse Salir")
                End If
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
            'vuelvo a cargar los datos originales
            If nuevo = False Then
                Call rellenarCamposDesdeObjeto(DP)
            End If
            Me.DialogResult = Windows.Forms.DialogResult.None
        Catch ex As Exception
            MsgBox(ex.ToString)
            Me.DialogResult = Windows.Forms.DialogResult.None
        End Try
    End Sub
    Private Function cargarNotas(ByRef fi As Ficha) As Integer
        Dim cn2 As New SqlConnection(ConeStr)
        Dim comp As Integer = 0
        Try
            If Not IsNothing(fi.notaEstecform) Or Not IsNothing(fi.notaINAEM) Then
                Dim NotasAcumuladas, cambiacomas1, cambiacomas2 As String
                For i As Integer = 29 To 37
                    cambiacomas1 = fi.listadoNombres(i - 1).ToString.Replace(",", ".")
                    cambiacomas2 = fi.listaValores(i).ToString.Replace(",", ".")
                    If fi.listaValores(i) <> "0.00" Then
                        NotasAcumuladas &= String.Format(", {0}={1}", cambiacomas1, cambiacomas2)
                    End If
                Next
                NotasAcumuladas = NotasAcumuladas.Substring(2)
                'saco la idDP con 1 subconsulta y asi me ahorro una busqueda. Lo pongo asi para que sea mas legible
                Dim subconsulta As String = String.Format("(SELECT {0}.Id FROM {0}, DatosPersonales WHERE {0}.IdDP=DatosPersonales.Id AND DatosPersonales.Id={1})", cat, fi.Id)                ' MsgBox(subconsulta)
                Dim sql As String = String.Format("UPDATE {0} SET {1} WHERE Candidatos.id={2}", cat, NotasAcumuladas, subconsulta)
                comp = Funciones1.ejecutarConsultaNonQuery(sql)
                If comp <= 0 Then Throw New miExcepcion("No se han podido cargar las notas")
            End If
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
        End Try
        Return comp
    End Function
    Private Function camposvacios() As List(Of String)
        Dim vacios As New List(Of String)
        If Me.txtNombre.Text = "" Then vacios.Add("El campo 'Nombre' está vacío")
        If Me.txtApellido1.Text = "" Then vacios.Add("El campo 'Primer Apellido'  está vacío")
        If Me.txtApellido2.Text = "" Then vacios.Add("El campo 'Segundo Apellido' está vacío")
        If Me.txtDNI.Text = "" Then vacios.Add("El campo 'DNI' está vacío")
        Dim numSSSinBarras As String
        numSSSinBarras = Me.txtNumSS.Text.Replace("/", "") '    Quito las barras para comprobar si el campo está vacío
        If numSSSinBarras = "" Then vacios.Add("El campo 'Numero de la seguridad Social' está vacío")
        'añadir o quitar los campos que queramos comprobar
        Return vacios
    End Function
    Private Function fallosEnCampos() As List(Of String)
        Dim comprobado As Boolean
        Dim cambios As New List(Of String)
        Dim numSSSinBarras As String = Me.txtNumSS.Text.Replace("/", "") '  Quito la barra para comprobar si el campo está vacío
        If DP.Nombre <> Me.txtNombre.Text Then cambios.Add(String.Format("El campo 'Nombre' va a ser cambiado de '{0}' a '{1}", DP.Nombre, Me.txtNombre.Text))
        If DP.Apellido1 <> Me.txtApellido1.Text Then cambios.Add(String.Format("El campo 'Primer Apellido' va a ser cambiado de '{0}' a '{1}", DP.Apellido1, Me.txtApellido1.Text))
        If DP.Apellido2 <> Me.txtApellido2.Text Then cambios.Add(String.Format("El campo 'Segundo Apellido' va a ser cambiado de '{0}' a '{1}", DP.Apellido2, Me.txtApellido2.Text))
        If DP.DNI <> Me.txtDNI.Text Then
            comprobado = Funciones1.ValidaNif(Me.txtDNI.Text)
            If comprobado = False Then
                cambios.Add("El DNI introducido no es válido.")
            Else
                cambios.Add(String.Format("El campo 'DNI' va a ser cambiado de '{0}' a '{1}", DP.DNI, Me.txtDNI.Text))
            End If
        End If
        If DP.NumSS <> numSSSinBarras Then
            comprobado = Funciones1.ValidaNumSS(numSSSinBarras)
            If comprobado = False Then
                cambios.Add("El Numero de la Seguridad Social introducido no es válido.")
            Else
                cambios.Add(String.Format("El campo 'Numero de la Seguridad Social' va a ser cambiado de '{0}' a '{1}", DP.NumSS, Me.txtNumSS.Text))
            End If
        End If
        'añadir mas campos si queremos comprobarlos
        Return cambios
    End Function
    Public Function cargarCambiosEnDPYaCreado(ByVal dat As Ficha) As Boolean
        Try
            Dim Datos As String = ""
            For i As Integer = 2 To 28  ' i=2 porque listavalores empieza en 1,
                '  el primer valor (Id)no lo queremos (es autoincremental) y hasta 29 para que solo coja hasta curso
                If Not IsNothing(dat.listaValores.Item(i)) Then
                    Select Case i
                        Case 8, 21 'valores integer
                            Datos &= String.Format(", {0}={1}", dat.listadoNombres.Item(i - 1), dat.listaValores.Item(i))
                        Case 15  'valores boolean
                            If dat.listaValores.Item(i) = True Then
                                Datos &= String.Format(", {0}=1", dat.listadoNombres.Item(i - 1))
                            Else
                                Datos &= String.Format(", {0}=0", dat.listadoNombres.Item(i - 1))
                            End If
                        Case Else   'valores String (necesitamos ponerles comillas simples delante y detrás)
                            Datos &= String.Format(", {0}='{1}'", dat.listadoNombres.Item(i - 1), dat.listaValores.Item(i))
                    End Select
                End If
            Next
            Datos = Datos.Substring(1)
            Dim sql As String = String.Format("UPDATE DatosPersonales SET {0} Where DatosPersonales.Id={1}", Datos, CInt(dat.Id))
            ' MsgBox(sql)
            Dim comp As Integer = Funciones1.ejecutarConsultaNonQuery(sql)
            If comp <= 0 Then Throw New miExcepcion("error en la insercion")
            comp = 0
            ''Aqui meto las notas
            comp = cargarNotas(dat)
            If comp <= 0 Then Throw New miExcepcion
        Catch ex2 As miExcepcion
            Return False
        Catch ex As Exception
            Return False
        Finally
        End Try
        Return True
    End Function
    Public Function CrearNuevoDPEnBaseDeDatos(ByVal Dat As Ficha) As Boolean
        Try
            Dim tablas As String = ""
            Dim valores As String = ""
            For i As Integer = 2 To 28 ' i=2 porque listavalores empieza en 1,
                '  el primer valor (Id)no lo queremos (es autoincremental) y hasta 29 para que solo coja hasta curso
                If Not IsNothing(Dat.listaValores.Item(i)) Then     'solo metemos los valores que NO sean nulos
                    Select Case i
                        Case 8, 21 'valores integer
                            tablas &= String.Format(", {0}", Dat.listadoNombres.Item(i - 1))
                            valores &= String.Format(", {0}", Dat.listaValores.Item(i))
                        Case 15  'valores boolean
                            tablas &= String.Format(", {0}", Dat.listadoNombres.Item(i - 1))
                            If Dat.listaValores.Item(i) = True Then
                                valores &= String.Format(", {0}", 1)
                            Else
                                valores &= String.Format(", {0}", 0)
                            End If
                        Case Else  'valores String (necesitamos ponerles comillas simples delante y detrás a los valores)
                            tablas &= String.Format(", {0}", Dat.listadoNombres.Item(i - 1))
                            valores &= String.Format(", '{0}'", Dat.listaValores.Item(i))
                    End Select
                End If
            Next
            'Le quito la primera coma y el primer espacio a las dos variables
            tablas = tablas.Substring(2)
            valores = valores.Substring(2)
            Dim sql As String = String.Format("INSERT INTO DatosPersonales ({0}) VALUES ({1})", tablas, valores)
            Dim comp As Integer = Funciones1.ejecutarConsultaNonQuery(sql)
            If comp <= 0 Then Throw New miExcepcion("error al crear la ficha")
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
            Return False
        Catch ex As Exception
            MsgBox(ex.ToString)
            Return False
        Finally
        End Try
        Return True
    End Function
    Private Sub cmdExperiencia_Click(sender As Object, e As EventArgs) Handles cmdExperiencia.Click
        If Me.CboExpSector.SelectedIndex = -1 Then
            MsgBox("seleccione un sector de experiencia laboral a añadir al listado")
        Else
            Dim repetido As Boolean = False
            For Each s As String In Me.LstExpSector.Items
                If s.ToString = Me.CboExpSector.SelectedItem.ToString Then
                    repetido = True
                    Exit For
                End If
            Next
            If repetido = False Then
                Me.LstExpSector.Items.Add(Me.CboExpSector.SelectedItem.ToString)
                '  MsgBox("Sector añadido al listado")
            Else
                MsgBox("Ese sector ya está elegido")
            End If
        End If
    End Sub
    Private Sub cmdQuitar_Click(sender As Object, e As EventArgs) Handles cmdQuitar.Click
        If Me.LstExpSector.SelectedIndex = -1 Then
            MsgBox("Seleccione el sector que desee quitar del listado")
        Else
            Me.LstExpSector.Items.RemoveAt(Me.LstExpSector.SelectedIndex)
        End If
    End Sub
    Private Sub cmdCancelar_Click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub cmdSalir_Click(sender As Object, e As EventArgs) Handles cmdSalir.Click
        Me.DialogResult = Windows.Forms.DialogResult.Abort
    End Sub
    Private Sub cmdCambiarFoto_Click(sender As Object, e As EventArgs) Handles cmdCambiarFoto.Click
        Call CambiarFoto()
    End Sub
    Private Sub cargarFotoEnFormulario(ByVal I As Integer)
        Try
            Dim sql As String = String.Format("select FotoPath from DatosPersonales WHERE Id={0})", I)
            Dim s As String = Funciones1.ejecutarConsultaScalarString(Sql)
            If s = "NULL" Then Throw New miExcepcion("No hay Foto Cargada")
            Me.PicBx1.ImageLocation = s
            Me.PicBx1.Show()
            Me.LblFoto.Tag = s
            'cn.Open()
            'Dim cmd As New SqlCommand(sql, cn)
            'Dim str As String = ""
            'Dim dr As SqlDataReader = cmd.ExecuteReader
            'If dr.Read Then
            '    Me.PicBx1.ImageLocation = dr(0)
            '    Me.PicBx1.Show()
            '    'Me.PicBx1.Load(str)
            '    Me.LblFoto.Tag = dr(1)
            'Else
            '    Throw New miExcepcion("No hay foto cargada")
            'End If
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cn.Close()
        End Try
    End Sub
    Private Sub PictureBox1_DoubleClick(sender As Object, e As EventArgs) Handles PicBx1.DoubleClick
        Call CambiarFoto()
    End Sub
    Private Sub CambiarFoto()
        fotoCambiada = True
        ' PicBx1.Tag = ""
        OFGSelectImage.InitialDirectory = PathFotos
        OFGSelectImage.Filter = "jpg files (*.jpg)|*.jpg|bmp files (*.bmp)|*.bmp|All files (*.*)|*.*"
        If OFGSelectImage.ShowDialog = Windows.Forms.DialogResult.OK Then
            PicBx1.Image = Image.FromFile(OFGSelectImage.FileName)
            Dim Path As String = "" 'El Path por ahora es en GIT,pero lo más seguro es que sea en S:\ 'Que es donde guardan las cosas en el servidor.
            If nuevo = False Then
                Path = (String.Format("{0}Ficha{1}.bmp", PathFotos, DP.Id))
                PicBx1.Image.Save(Path)
            Else
                Path = String.Format("{0}FichaNew.bmp", PathFotos)
            End If
            ' guardo la ubicacion de la foto en el picturebox
            PicBx1.Tag = Path
        End If
    End Sub
   
    Private Sub cmdBorrar_Click(sender As Object, e As EventArgs) Handles cmdBorrar.Click
        Dim respuesta1, respuesta2 As MsgBoxResult
        respuesta1 = MsgBox(String.Format("Ha seleccionado la ficha:" & vbCrLf & " ' {0} {1} {2} '" & vbCrLf & " para ser borrada" & vbCrLf &
                                          "¿Está seguro?", DP.Nombre, DP.Apellido1, DP.Apellido2), MsgBoxStyle.YesNo)
        If respuesta1 = MsgBoxResult.No Then Throw New miExcepcion("Borrado cancelado a peticion del usuario")
        respuesta2 = MsgBox("¿Seguro que desea continuar?" & vbCrLf & "Una vez borrado no se puede recuperar", MsgBoxStyle.YesNo)
        If respuesta2 = MsgBoxResult.No Then Throw New miExcepcion("Borrado cancelado a peticion del usuario")
        Dim borrado As Boolean = False
        borrado = borrarDatosPersonales(DP.Id)
        If borrado = True Then
            MsgBox("Alumno y sus datos borrados con éxito")
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Else
            Me.DialogResult = Windows.Forms.DialogResult.None
            Throw New miExcepcion("Error al borrar")
        End If
    End Sub
    Public Function borrarDatosPersonales(ByVal i As String) As Boolean
        Try
            Dim idtabla, comp As Integer
            Dim sqlidtabla, sqlborratabla, sqlDatosPersonales As String
            sqlidtabla = String.Format("Select {0}.Id from DatosPersonales, {0} where DatosPersonales.Id={0}.IdDP and DatosPersonales.Id={1}", cat, i)
            sqlDatosPersonales = String.Format("DELETE FROM DatosPersonales WHERE DatosPersonales.Id={0}", DP.Id)
            idtabla = Funciones1.ejecutarConsultaScalar(sqlidtabla)
            If idtabla < 0 Then Throw New miExcepcion(String.Format("Error al obtener la Id de {0}", cat))
            sqlborratabla = String.Format("delete from {0} where {0}.id={1}", cat, idtabla)
            comp = Funciones1.ejecutarConsultaNonQuery(sqlborratabla)
            If comp < 0 Then Throw New miExcepcion(String.Format("Error al borrar de {0}", cat))
            comp = 0
            comp = Funciones1.ejecutarConsultaNonQuery(sqlDatosPersonales)
            If comp < 0 Then Throw New miExcepcion(String.Format("Error al borrar datos personales en {0}", cat))
        Catch ex2 As miExcepcion
            Return False
            MsgBox(ex2.ToString)
        Catch ex As Exception
            Return False
            MsgBox(ex.ToString)
        Finally
        End Try
        Return True
    End Function
    Private Sub cmdAñadirAAlumnos_Click(sender As Object, e As EventArgs) Handles cmdAñadirAAlumnos.Click
        If Me.optAptoSi.Checked = False Then
            MsgBox("El candidato no está calificado como Apto")
        Else
            If Not IsNothing(DP.Curso) Then '   si hay DP.Curso es porque ya se han metido datos en la ficha
                Try
                    Dim sqlIdCand, sqlDelCandCur, sqlDelCand, sqlidal, sqlinsAl, sqlInsAlCur As String
                    Dim idcand, idal, contr As Integer
                    sqlIdCand = String.Format("SELECT C.Id FROM Candidatos c, DatosPersonales DP where  Dp.id=C.IdDP and DP.Id={0}", DP.Id)
                    idcand = Funciones1.ejecutarConsultaScalar(sqlIdCand)
                    sqlDelCandCur = String.Format("DELETE FROM Candidatos_Cursos WHERE idCa={0}", idCand)
                    contr = Funciones1.ejecutarConsultaNonQuery(sqlDelCandCur)
                    If contr <> 1 Then Throw New miExcepcion("Error al borrar de candidatos_cursos")
                    contr = 0
                    sqlDelCand = String.Format("DELETE FROM Candidatos WHERE id={0}", idCand)
                    contr = Funciones1.ejecutarConsultaNonQuery(sqlDelCand)
                    If contr <> 1 Then Throw New miExcepcion("Error al borrar de candidatos")
                    contr = 0
                    sqlinsAl = String.Format("INSERT INTO Alumnos (IdDP) VALUES ({0})", DP.Id)
                    contr = Funciones1.ejecutarConsultaNonQuery(sqlinsAl)
                    If contr <> 1 Then Throw New miExcepcion("Error al Insertar en Alumnos")
                    contr = 0
                    sqlidal = String.Format("SELECT Id FROM Alumnos WHERE IdDP={0}", DP.Id)
                    idal = Funciones1.ejecutarConsultaScalar(sqlidal)
                    If idal <= 0 Then Throw New miExcepcion("Error recuperar Ultima Id en Alumnos")
                    sqlInsAlCur = String.Format("INSERT INTO Alumnos_Cursos (IdAl,IdCur) VALUES ({0},{1})", idal, DP.Curso)
                    contr = Funciones1.ejecutarConsultaNonQuery(sqlInsAlCur)
                    If contr <> 1 Then Throw New miExcepcion("Error al Insertar en Alumnos_Cursos")
                    MsgBox(String.Format("{0} {1} Cambiado a Alumno del curso {2}", DP.Nombre, DP.Apellido1, Me.lblNombreCurso.Text))
                Catch ex2 As miExcepcion
                    MsgBox(ex2.ToString)
                Catch ex As Exception
                    MsgBox(ex.ToString)
                Finally
                End Try
            End If
        End If
    End Sub 
    Private Sub optAptoSi_Click(sender As Object, e As EventArgs)
        Me.cmdAñadirAAlumnos.Visible = True
        Me.cmdAñadirAAlumnos.Enabled = True
    End Sub
    Private Sub OptAptoNo_Click(sender As Object, e As EventArgs)
        Me.cmdAñadirAAlumnos.Visible = False
        Me.cmdAñadirAAlumnos.Enabled = False
    End Sub
    Private Sub OptAptoPendiente_Click(sender As Object, e As EventArgs)
        Me.cmdAñadirAAlumnos.Visible = True
        Me.cmdAñadirAAlumnos.Enabled = False
    End Sub
    Private Sub cmdAñadirComentarios_Click(sender As Object, e As EventArgs) Handles cmdAñadirComentarios.Click
        Dim frm As New FrmComentarios(DP)
        If frm.ShowDialog = Windows.Forms.DialogResult.OK Then
            GuardaComentarios = DP.Comentarios
        Else
            ' MsgBox("No se ha guardado el comentario")
        End If
    End Sub
    Private Sub cboCursos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCursos.SelectedIndexChanged
        Me.lblNombreCurso.Text = ""
        Dim aux(3) As String
        aux = Split(Me.cboCursos.SelectedItem.ToString, "_")
        idcur = aux(0)
        Me.lblNombreCurso.Text = aux(2)
    End Sub
    Private Sub recargarnotaEstecForm()
        Dim num As Double = CDbl(Me.MtxtEstecTest.Text) + CDbl(Me.MtxtEstecDinam.Text) + CDbl(Me.MtxtEstecEntr.Text)
        Me.MtxtEstecNOTA.Text = num.ToString("##,##0.00")
    End Sub
    Private Sub recargarnotaEstecINAEM()
        Dim num As Double = CDbl(Me.MtxtInaemMujer.Text) + CDbl(Me.MtxtInaemDiscap.Text) + CDbl(Me.MtxtInaemBajaContr.Text) + CDbl(Me.MtxtInaemJoven.Text) + CDbl(Me.MtxtInaemOtros.Text)
        Me.MtxtInaemNOTA.Text = num.ToString("##,##0.00")
    End Sub
    Private Sub MtxtEstecTest_MouseClick(sender As Object, e As MouseEventArgs) Handles MtxtEstecTest.MouseClick
        Call recargarnotaEstecForm()
    End Sub
    Private Sub MtxtEstecDinam_MouseClick(sender As Object, e As MouseEventArgs) Handles MtxtEstecDinam.MouseClick
        Call recargarnotaEstecForm()
    End Sub
    Private Sub MtxtEstecEntr_MouseClick(sender As Object, e As MouseEventArgs) Handles MtxtEstecEntr.MouseClick
        Call recargarnotaEstecForm()
    End Sub
    Private Sub MtxtInaemOtros_MouseClick(sender As Object, e As MouseEventArgs) Handles MtxtInaemOtros.MouseClick
        recargarnotaEstecINAEM()
    End Sub
    Private Sub MtxtInaemJoven_MouseClick(sender As Object, e As MouseEventArgs) Handles MtxtInaemJoven.MouseClick
        recargarnotaEstecINAEM()
    End Sub
    Private Sub MtxtInaemBajaContr_MouseClick(sender As Object, e As MouseEventArgs) Handles MtxtInaemBajaContr.MouseClick
        recargarnotaEstecINAEM()
    End Sub
    Private Sub MtxtInaemDiscap_MouseClick(sender As Object, e As MouseEventArgs) Handles MtxtInaemDiscap.MouseClick
        recargarnotaEstecINAEM()
    End Sub
    Private Sub MtxtInaemMujer_MouseClick(sender As Object, e As MouseEventArgs) Handles MtxtInaemMujer.MouseClick
        recargarnotaEstecINAEM()
    End Sub
End Class