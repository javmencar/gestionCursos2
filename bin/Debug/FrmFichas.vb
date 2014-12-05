Imports System.Data.SqlClient
Public Class FrmFichas
    Dim nuevo, fotoCambiada As Boolean
    Public DP As Ficha
    Dim tipo As Integer
    Public cat As String
    Public cn As SqlConnection
    Dim NuIdDP, idcur As Integer
    Dim GuardaComentarios As String
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
        If tipo = 3 Then
            Me.GbCalificacion.Visible = True
        Else
            Me.GbCalificacion.Visible = False
        End If
        Call cargarcomboCursos()
        If nuevo = True Then
            DP = New Ficha
            Me.cboCursos.Enabled = True
            Me.cmdModificar.Text = "CREAR NUEVA FICHA"
            Me.cmdCancelar.Text = "Cancelar La Creación"
            Me.cmdCambiarFoto.Text = "Insertar Foto"
            Me.PicBx1.Image = Image.FromFile("C:\GIT\GestionCursos1\Resources\female-silhouette_0.jpg")
            Me.PicBx1.Tag = "C:\GIT\GestionCursos1\Resources\female-silhouette_0.jpg"
            ' NuIdDP = cogerUltimaId() + 1
            ' MsgBox("UltimaID + 1=  " & NuIdDP)
            Me.txtFNac.Text = "01/01/1900"
            Me.txtFecEntr.Text = "01/01/1900"
            Me.txtInFecha.Text = "01/01/1900"
            Me.OptAptoPendiente.Select()
            Me.cboCursos.Enabled = True
            Me.GbCalificacion.Enabled = False
            Me.GbCalificacion.Visible = False
        Else
            'Call desplegarfichaPasadaPorValor()
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
    Private Sub desplegarfichaPasadaPorValor()
        'sirve para ver los datos del objeto que se ha pasado como valor desde frmListado en un msgbox
        Dim acum As String = ""
        With DP
            For i As Integer = 1 To DP.listaValores.Count
                acum &= String.Format("{0}=  {1}" & vbCrLf, .listadoNombres(i - 1), .listaValores(i))
            Next
        End With
        MsgBox(acum)
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
            If .Fnac <> "1/1/1900" Then
                Me.txtFNac.Text = .Fnac
            Else
                Me.txtFNac.Text = "1/1/1900"
            End If
            ' Me.txtFNac.Text = CStr(.FecEntr)
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
            If .InFecha <> "1/1/1900" Then
                Me.txtInFecha.Text = .InFecha
            Else
                Me.txtInFecha.Text = "1/1/1900"
            End If
            ' Me.txtInFecha.Text = CStr(.InFecha)
            Me.txtNivelEstudios.Text = MeterSaltosDeLinea(.NivelEstudios)
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
            If .FecEntr <> "1/1/1900" Then
                Me.txtFecEntr.Text = .FecEntr
            Else
                Me.txtFecEntr.Text = "1/1/1900"
            End If
            ' Me.txtFecEntr.Text = CStr(.FecEntr)
            Me.txtValoracion.Text = MeterSaltosDeLinea(.Valoracion)

            '######
            'Posiblemente tenga que sacar esto de aqui a un sub para tener en cuenta las notas
            If Not IsNothing(.Apto) Then
                Dim esta As Boolean
                Select Case .Apto
                    Case "Apto"
                        Me.optAptoSi.Select()
                        Me.cmdAñadirAAlumnos.Visible = True
                        If nuevo = True Then
                            Me.cmdAñadirAAlumnos.Enabled = True
                        Else
                            If cat = "Alumnos" Or cat = "Profesores" Then
                                esta = EstaEnTablas(.Id)
                                If esta = False Then Me.cmdAñadirAAlumnos.Enabled = True
                            End If
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
                GuardaComentarios = MeterSaltosDeLinea(.Comentarios)
                '    Me.lblComentariosEscritos.Text = GuardaComentarios
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
            If Not IsNothing(.EstecTest) Then
                Me.MtxtEstecTest.Text = .EstecTest
            Else
                Me.MtxtEstecTest.Text = "00.00"
            End If
            If Not IsNothing(.EstecDinam) Then
                Me.MtxtEstecDinam.Text = .EstecDinam
            Else
                Me.MtxtEstecDinam.Text = "00.00"
            End If
            If Not IsNothing(.EstecEntr) Then
                Me.MtxtEstecEntr.Text = .EstecEntr
            Else
                Me.MtxtEstecEntr.Text = "00.00"
            End If
            If Not IsNothing(.notaEstecform) Then
                Me.MtxtEstecNOTA.Text = .notaEstecform
            Else
                Me.MtxtEstecNOTA.Text = "00.00"
            End If
            If Not IsNothing(.InaemMujer) Then
                Me.MtxtInaemMujer.Text = .InaemMujer
            Else
                Me.MtxtInaemMujer.Text = "00.00"
            End If
            If Not IsNothing(.InaemDiscap) Then
                Me.MtxtInaemDiscap.Text = .InaemDiscap
            Else
                Me.MtxtInaemDiscap.Text = "00.00"
            End If
            If Not IsNothing(.InaemBajaCon) Then
                Me.MtxtInaemBajaContr.Text = .InaemBajaCon
            Else
                Me.MtxtInaemBajaContr.Text = "00.00"
            End If
            If Not IsNothing(.InaemJoven) Then
                Me.MtxtInaemJoven.Text = .InaemJoven
            Else
                Me.MtxtInaemJoven.Text = "00.00"
            End If
            If Not IsNothing(.InaemOtros) Then
                Me.MtxtInaemOtros.Text = .InaemOtros
            Else
                Me.MtxtInaemOtros.Text = "00.00"
            End If
            If Not IsNothing(.InaemMujer) Then
                Me.MtxtInaemMujer.Text = .InaemMujer
            Else
                Me.MtxtInaemMujer.Text = "00.00"
            End If
            If Not IsNothing(.notaINAEM) Then
                Me.MtxtInaemNOTA.Text = .notaINAEM
            Else
                Me.MtxtInaemNOTA.Text = "00.00"
            End If
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
    Private Function QuitarSaltosDeLinea(ByVal str As String) As String
        str = str.Replace(vbCrLf, "#")
        Return str
    End Function
    Private Function MeterSaltosDeLinea(ByVal str As String) As String
        str = str.Replace("#", vbCrLf)
        Return str
    End Function
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
                err = comprobarformatofecha(Me.txtFNac.Text)
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
                    err = comprobarformatofecha(Me.txtInFecha.Text)
                    If err <> 0 Then
                        MsgBox("Fecha de nacimiento incorrecta." & vbCrLf & "Se cargará 01/01/1900")
                        .Fnac = "01/01/1900"
                    Else
                        .InFecha = Me.txtInFecha.Text
                    End If
                Else
                    .InInaem = "False"
                End If
                .NivelEstudios = QuitarSaltosDeLinea(Me.txtNivelEstudios.Text)
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
                    err = comprobarformatofecha(Me.txtFecEntr.Text)
                    If err <> 0 Then
                        MsgBox("Fecha de nacimiento incorrecta." & vbCrLf & "Se cargará 01/01/1900")
                        .Fnac = "01/01/1900"
                    Else
                        .FecEntr = Me.txtFecEntr.Text
                    End If
                    .Valoracion = QuitarSaltosDeLinea(Me.txtValoracion.Text)
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
                .Comentarios = QuitarSaltosDeLinea(GuardaComentarios)
                .Curso = idcur
                'For Each c As Control In Me.GbCalificacion.Controls
                '    If TypeOf (c) Is MaskedTextBox Then
                '        If c.Text = "00,00" Then c.Text = "00.00"
                '    End If
                'Next
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
            Dim FichaRellenada As Ficha
            Dim fallos As List(Of String)
            If nuevo = True Then
                fallos = camposvacios()
                'ademas de campos vacios , quiero que compruebe el DNI y el NumSS
                If tipo = 3 Then
                    If Me.lblCurso.Text = "" Then Throw New miExcepcion("Debe elegir el curso al que se va a apuntar el alumno")
                End If
                Dim comprobado As Boolean
                If Me.txtDNI.Text <> "" Then
                    comprobado = ValidaNif(Me.txtDNI.Text)
                    If comprobado = False Then
                        fallos.Add("El DNI introducido NO es válido.")
                    End If
                End If
                Dim numSSSinBarras As String = Me.txtNumSS.Text.Replace("/", "")
                If numSSSinBarras <> "" Then
                    comprobado = ValidaNumSS(numSSSinBarras)
                    If comprobado = False Then
                        fallos.Add("El Numero de la Seguridad Social NO es válido")
                    End If
                End If
                'Hasta aqui las validaciones del DNI y NumSS
                '########
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
                    NuIdDP = cogerUltimaId("DatosPersonales")
                    If NuIdDP = -1 Then Throw New miExcepcion("Error al calcular la ultima ID")
                    'meto la id en el objeto ficha, porque luego lo voy a utilizar
                    FichaRellenada.Id = NuIdDP
                    ' con esto inserto en la tabla alumnos, profesores o candidatos
                    Dim comprob As Integer = insertarEnTablacategoria(NuIdDP)
                    If comprob = -1 Then Throw New miExcepcion(String.Format("Problema al insertar en {0}", cat))
                    'y ahora inserto en la tabla cursos y secundarias
                    Dim idt, idc As Integer
                    idt = cogerUltimaId(cat)
                    idc = FichaRellenada.Curso
                    Dim comprobarcursos = insertarEnTablaSecundariaCursos(idt, idc)
                Else    ' no hace falta insertar en BBDD, ya está en ambas tablas
                    'cargo los datos del objeto ya creado
                    Dim comprobacion As Boolean = cargarCambiosEnDPYaCreado(FichaRellenada)
                    If comprobacion = False Then Throw New miExcepcion("Error al cargar los datos personales de la ficha")
                End If
                'con la ficha creada o modificada, meto las notas , si las hay y el curso
                If FichaRellenada.notaEstecform <> "0.00" Or FichaRellenada.notaINAEM <> "0.00" Then
                    Dim comprobaNotas As Integer = cargarNotas(FichaRellenada)
                    'que solo compruebe errores en los candidatos
                    If tipo = 3 AndAlso comprobaNotas = 0 Then Throw New miExcepcion("Error al meter las notas")
                End If
            Else ' si no hay nada en el objeto es que ha habido error al crearlo
                If nuevo = True Then
                    Throw New miExcepcion("Cambie los campos necesarios para poder Crear la ficha" & vbCrLf &
                                          " o Pulse Salir")
                Else
                    Throw New miExcepcion("Cambie los campos necesarios para poder Modificar la ficha" & vbCrLf &
                                          " o Pulse Salir")
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
    Private Function insertarEnTablaSecundariaCursos(ByVal itab As Integer, ByVal icu As Integer) As Integer
        Dim control As Integer = 0
        Dim cortado As String = cat.Substring(0, 2)
        Dim sql As String = String.Format("INSERT INTO {0}_Cursos (Id{1},IdCur) VALUES ({2},{3})", cat, cortado, itab, icu)
        Try
            cn.Open()
            Dim cmd As New SqlCommand(sql, cn)
            control = cmd.ExecuteNonQuery
            If control < 0 Then Throw New miExcepcion
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cn.Close()
        End Try
        Return control
    End Function
    Private Function cargarNotas(ByRef fi As Ficha) As Integer
        Dim cn2 As New SqlConnection(ConeStr)
        Dim j As Integer = 0
        Try
            If Not IsNothing(fi.notaEstecform) Or Not IsNothing(fi.notaINAEM) Then
                Dim NotasAcumuladas As String = ""
                For i As Integer = 29 To 37
                    Dim cambiacomas1, cambiacomas2 As String
                    cambiacomas1 = fi.listadoNombres(i - 1).ToString.Replace(",", ".")
                    cambiacomas2 = fi.listaValores(i).ToString.Replace(",", ".")
                    If fi.listaValores(i) <> "0.00" Then
                        NotasAcumuladas &= String.Format(", {0}={1}", cambiacomas1, cambiacomas2)
                    End If
                Next
                NotasAcumuladas = NotasAcumuladas.Substring(2)
                'saco la idDP con 1 subconsulta y asi me ahorro una busqueda. Lo pongo asi para que sea mas legible
                Dim subconsulta As String = String.Format("(SELECT Candidatos.Id FROM Candidatos, DatosPersonales WHERE Candidatos.IdDP=DatosPersonales.Id AND DatosPersonales.Id={0})", fi.Id)                ' MsgBox(subconsulta)
                Dim sql As String = String.Format("UPDATE Candidatos SET {0} WHERE Candidatos.id={1}", NotasAcumuladas, subconsulta)                ' MsgBox(sql)
                cn2.Open()
                Dim cmd As New SqlCommand(sql, cn2)
                j = cmd.ExecuteNonQuery                '  If j > 0 Then MsgBox("notas cargadas")
            End If
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cn.Close()
            cn2.Close()
        End Try
        Return j
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

        '   If DP.DNI <> Me.txtDNI.Text Then cambios.Add(String.Format("El campo 'DNI' va a ser cambiado de '{0}' a '{1}", DP.DNI, Me.txtDNI.Text))
        If DP.DNI <> Me.txtDNI.Text Then
            comprobado = ValidaNif(Me.txtDNI.Text)
            If comprobado = False Then
                cambios.Add("El DNI introducido no es válido.")
            Else
                cambios.Add(String.Format("El campo 'DNI' va a ser cambiado de '{0}' a '{1}", DP.DNI, Me.txtDNI.Text))
            End If
        End If
        '  If DP.NumSS <> numSSSinBarras Then cambios.Add(String.Format("El campo 'Numero de la Seguridad Social' va a ser cambiado de '{0}' a '{1}", DP.NumSS, Me.txtNumSS.Text))
        If DP.NumSS <> numSSSinBarras Then
            comprobado = ValidaNumSS(numSSSinBarras)
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
            '   le quito la primera coma
            Datos = Datos.Substring(1)
            Dim sql As String = String.Format("UPDATE DatosPersonales SET {0} Where DatosPersonales.Id={1}", Datos, CInt(dat.Id))
            ' MsgBox(sql)
            cn.Open()
            Dim cmd As New SqlCommand(sql, cn)
            Dim j As Integer = cmd.ExecuteNonQuery()
            '   No debería dar distinto de 1, porque solo debería afectar a un registro
            If j <> 1 Then Throw New miExcepcion("error en la insercion")
            'Aqui meto las notas
            
        Catch ex2 As miExcepcion
            Return False
        Catch ex As Exception
            Return False
        Finally
            cn.Close()

        End Try
        Return True
    End Function
    Public Function CrearNuevoDPEnBaseDeDatos(ByVal Dat As Ficha) As Boolean
        'INSERT INTO 
        'ojo que aqui hay que hacer dos consultas distintas, una por datos personales y otra po candidatos
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
            '  MsgBox(sql)
            cn.Open()
            Dim cmd As New SqlCommand(sql, cn)
            Dim j As Integer = cmd.ExecuteNonQuery()
            '   No debería dar distinto de 1, porque solo debería afectar a un registro
            If j <> 1 Then Throw New miExcepcion("error en la insercion")
            'recojo la nueva IdDP en la variable publica
            NuIdDP = cogerUltimaId("DatosPersonales")
            ' MsgBox("Datos personales introducidos en la base de datos")
        Catch ex2 As miExcepcion
            MsgBox(ex2.ToString)
            Return False
        Catch ex As Exception
            MsgBox(ex.ToString)
            Return False
        Finally
            cn.Close()
        End Try
        Return True
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
            Me.LstExpSector.Items.RemoveAt(Me.LstExpSector.SelectedIndex)            ' MsgBox("Sector eliminado del listado")
        End If
    End Sub
    Private Sub cmdCancelar_Click(sender As Object, e As EventArgs) Handles cmdCancelar.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub
    Private Sub cmdSalir_Click(sender As Object, e As EventArgs) Handles cmdSalir.Click
        Me.DialogResult = Windows.Forms.DialogResult.Abort
    End Sub
    Public Function cogerUltimaId(ByVal str As String) As Integer
        Dim i As Integer = 0
        cn = New SqlConnection(ConeStr)
        Try
            cn.Open()
            Dim sql As String = "" ' "select top 1 DatosPersonales.id from DatosPersonales order by id desc"
            sql = String.Format("SELECT TOP 1 {0}.Id FROM {0} ORDER BY Id DESC", str)
            Dim cmd As New SqlCommand(sql, cn)
            i = cmd.ExecuteScalar
        Catch ex As Exception
            i = -1
        Finally
            cn.Close()
        End Try
        Return i
    End Function
    Public Function insertarEnTablacategoria(ByVal nid As Integer) As Integer
        Dim i As Integer
        cn = New SqlConnection(ConeStr)
        Try
            cn.Open()
            Dim sql As String = String.Format("INSERT INTO {0} ({0}.idDP) VALUES ({1})", cat, nid)         
            Dim cmd As New SqlCommand(sql, cn)
            i = cmd.ExecuteNonQuery
        Catch ex As Exception
            i = -1
        Finally
            cn.Close()
        End Try
        Return i
    End Function
    Private Sub cmdCambiarFoto_Click(sender As Object, e As EventArgs) Handles cmdCambiarFoto.Click
        Call CambiarFoto()
    End Sub
    Private Sub cargarFotoEnFormulario(ByVal I As Integer)
        Try
            'Dim id As String = "22"
            Dim sql As String = String.Format("select FotoPath from DatosPersonales WHERE Id={0})", I)
            cn.Open()
            Dim cmd As New SqlCommand(sql, cn)
            Dim str As String = ""
            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.Read Then
                Me.PicBx1.ImageLocation = dr(0)
                Me.PicBx1.Show()
                'Me.PicBx1.Load(str)
                Me.LblFoto.Tag = dr(1)
            Else
                Throw New miExcepcion("No hay foto cargada")
            End If
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
        If OFGSelectImage.ShowDialog = Windows.Forms.DialogResult.OK Then
            PicBx1.Image = Image.FromFile(OFGSelectImage.FileName)
            Dim Path As String = ""
            'El Path lo deberé ajustar una vez sepa donde se guardarán los archivos del programa
            If nuevo = False Then
                'por ahora es en GIT,pero lo más seguro es que sea en S:\
                'Que es donde guardan las cosas en el servidor.
                Path = (String.Format("{0}Ficha{1}.bmp", PathFotos, DP.Id))
                PicBx1.Image.Save(Path)
            Else
                Path = String.Format("{0}FichaNew.bmp", PathFotos)
            End If
            ' PicBx1.Image.Save(Path)
            ' MsgBox(String.Format("Imagen guardada en {0}", Path))
            'asi guardo la ubicacion de la foto en el picturebox
            PicBx1.Tag = Path
        End If
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
        Dim dc As String = GetDCNumSegSocial(NssSCC, False)
        Dim s As String = NSS.Substring(NSS.Length - 2, 2)
        If s = dc Then Return True
        Return False
    End Function
    Private Sub cmdBorrar_Click(sender As Object, e As EventArgs) Handles cmdBorrar.Click
        Dim respuesta1, respuesta2 As MsgBoxResult
        'Dim nombre As String = DP.Nombre & DP.Apellido1 & DP.Apellido2

        respuesta1 = MsgBox(String.Format("Ha seleccionado la ficha:" & vbCrLf &
                                          " ' {0} {1} {2} '" & vbCrLf &
                                          " para ser borrada" & vbCrLf &
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
        Dim num, idtabla As Integer
        Dim sqlidtabla, sqlborratabla, sqlDatosPersonales As String
        sqlidtabla = String.Format("Select {0}.Id from DatosPersonales, {0} where DatosPersonales.Id={0}.IdDP and DatosPersonales.Id={1}", cat, i)
        MsgBox(sqlidtabla)
        sqlDatosPersonales = String.Format("DELETE FROM DatosPersonales WHERE DatosPersonales.Id={0}", DP.Id)
        ' MsgBox(sqlDatosPersonales)
        Dim cn2 As New SqlConnection(ConeStr)
        Try
            Dim cmdIdtabla, cmdDelTabla, cmdDelDP As SqlCommand
            cn.Open()
            'hago la consulta para obtener la ID del Alumno
            cmdIdtabla = New SqlCommand(sqlidtabla, cn)
            idtabla = cmdIdtabla.ExecuteScalar
            If idtabla < 0 Then Throw New miExcepcion(String.Format("Error al obtener la Id de {0}", cat))
            cn.Close()
            'con el Id correcto, lo cargo en la sql de borrado
            sqlborratabla = String.Format("delete from {0} where {0}.id={1}", cat, idtabla)
            'MsgBox(sqlalumnos)
            'Abro otra vez para borrar en tabla Alumnos o profesores
            cn.Open()
            cmdDelTabla = New SqlCommand(sqlborratabla, cn)
            num = cmdDelTabla.ExecuteNonQuery
            If num < 0 Then Throw New miExcepcion(String.Format("Error al borrar de {0}", cat))
            cn2.Open()
            cmdDelDP = New SqlCommand(sqlDatosPersonales, cn2)
            num = cmdDelDP.ExecuteNonQuery
            If num < 0 Then Throw New miExcepcion(String.Format("Error al borrar datos personales en {0}", cat))

        Catch ex2 As miExcepcion
            Return False
            MsgBox(ex2.ToString)
        Catch ex As Exception
            Return False
            MsgBox(ex.ToString)
        Finally
            cn2.Close()
            cn.Close()
        End Try
        Return True
    End Function
    Private Sub cmdAñadirAAlumnos_Click(sender As Object, e As EventArgs) Handles cmdAñadirAAlumnos.Click
        If Me.optAptoSi.Checked = False Then
            MsgBox("El candidato no está calificado como Apto")
        Else
            Dim cn2 As New SqlConnection(ConeStr)
            '####
            'TO DO:
            'Quitar de tabla candidatos y añadir a la tabla alumnos.
            '######
            If Not IsNothing(DP.Curso) Then '   si hay DP.Curso es porque ya se han metido datos en la ficha
                Try
                    Dim sqlIdCand, sqlDelCandCur, sqlDelCand, sqlidal, sqlinsAl, sqlInsAlCur As String
                    Dim idcand, idal, contr As Integer
                    sqlIdCand = String.Format("SELECT C.Id FROM Candidatos c, DatosPersonales DP where  Dp.id=C.IdDP and DP.Id={0}", DP.Id)
                    idcand = ejecutarConsultaScalar(sqlIdCand)
                    sqlDelCandCur = String.Format("DELETE FROM Candidatos_Cursos WHERE idCa={0}", idCand)
                    contr = ejecutarConsultaNonQuery(sqlDelCandCur)
                    If contr <> 1 Then Throw New miExcepcion("Error al borrar de candidatos_cursos")
                    contr = 0
                    sqlDelCand = String.Format("DELETE FROM Candidatos WHERE id={0}", idCand)
                    contr = ejecutarConsultaNonQuery(sqlDelCand)
                    If contr <> 1 Then Throw New miExcepcion("Error al borrar de candidatos")
                    contr = 0
                    sqlinsAl = String.Format("INSERT INTO Alumnos (IdDP) VALUES ({0})", DP.Id)
                    contr = ejecutarConsultaNonQuery(sqlinsAl)
                    If contr <> 1 Then Throw New miExcepcion("Error al Insertar en Alumnos")
                    contr = 0
                    sqlidal = String.Format("SELECT Id FROM Alumnos WHERE IdDP={0}", DP.Id)
                    idal = ejecutarConsultaScalar(sqlidal)
                    If idal <= 0 Then Throw New miExcepcion("Error recuperar Ultima Id en Alumnos")
                    sqlInsAlCur = String.Format("INSERT INTO Alumnos_Cursos (IdAl,IdCur) VALUES ({0},{1})", idal, DP.Curso)
                    contr = ejecutarConsultaNonQuery(sqlInsAlCur)
                    If contr <> 1 Then Throw New miExcepcion("Error al Insertar en Alumnos_Cursos")
                    MsgBox(String.Format("{0} {1} Cambiado a Alumno del curso {2}", DP.Nombre, DP.Apellido1, Me.lblNombreCurso.Text))
                Catch ex2 As miExcepcion

                    MsgBox(ex2.ToString)
                Catch ex As Exception

                    MsgBox(ex.ToString)
                Finally
                    cn.Close()

                End Try

            End If


        End If

    End Sub
    Private Function ejecutarConsultaScalar(ByVal str As String) As Integer
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
    Private Function ejecutarConsultaNonQuery(ByVal str As String) As Integer
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

    Private Sub optAptoSi_Click(sender As Object, e As EventArgs) Handles optAptoSi.Click
        Me.cmdAñadirAAlumnos.Visible = True
        Me.cmdAñadirAAlumnos.Enabled = True
    End Sub
    Private Sub OptAptoNo_Click(sender As Object, e As EventArgs) Handles OptAptoNo.Click
        Me.cmdAñadirAAlumnos.Visible = False
        Me.cmdAñadirAAlumnos.Enabled = False
    End Sub
    Private Sub OptAptoPendiente_Click(sender As Object, e As EventArgs) Handles OptAptoPendiente.Click
        Me.cmdAñadirAAlumnos.Visible = True
        Me.cmdAñadirAAlumnos.Enabled = False
    End Sub

    Private Sub cmdAñadirComentarios_Click(sender As Object, e As EventArgs) Handles cmdAñadirComentarios.Click
        Dim frm As New FrmComentarios(DP)
        If frm.ShowDialog = Windows.Forms.DialogResult.OK Then
            GuardaComentarios = DP.Comentarios

        Else            '      MsgBox("No se ha guardado el comentario")
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