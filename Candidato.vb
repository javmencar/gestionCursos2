Imports System.Collections.Generic
Public Class Candidato
    Inherits DatosPersonales
    Private mEstecTest, mEstecDinam, mEstecEntr,
        mInaemMujer, mInaemDiscap, mInaemBajaCon, mInaemJoven, mInaemOtros As Double
    Public Property EstecTest As String
        Get
            Return mEstecTest.ToString("##,##0.00")
            ' Return CStr(mEstecTest)
        End Get
        Set(ByVal Value As String)
            mEstecTest = CDbl(Value)
        End Set
    End Property
    Public Property EstecDinam As String
        Get
            Return mEstecDinam.ToString("##,##0.00")
            ' Return CStr(mEstecTest)
        End Get
        Set(ByVal Value As String)
            mEstecDinam = CDbl(Value)
        End Set
    End Property
    Public Property EstecEntr As String
        Get
            Return mEstecEntr.ToString("##,##0.00")
            ' Return CStr(mEstecTest)
        End Get
        Set(ByVal Value As String)
            mEstecEntr = CDbl(Value)
        End Set
    End Property
    Public Property InaemMujer As String
        Get
            Return mInaemMujer.ToString("##,##0.00")
            ' Return CStr(mEstecTest)
        End Get
        Set(ByVal Value As String)
            mInaemMujer = CDbl(Value)
        End Set
    End Property
    Public Property InaemDiscap As String
        Get
            Return mInaemDiscap.ToString("##,##0.00")
            ' Return CStr(mEstecTest)
        End Get
        Set(ByVal Value As String)
            mInaemDiscap = CDbl(Value)
        End Set
    End Property
    Public Property InaemBajaCon As String
        Get
            Return mInaemBajaCon.ToString("##,##0.00")
            ' Return CStr(mEstecTest)
        End Get
        Set(ByVal Value As String)
            mInaemBajaCon = CDbl(Value)
        End Set
    End Property
    Public Property InaemJoven As String
        Get
            Return mInaemJoven.ToString("##,##0.00")
            ' Return CStr(mEstecTest)
        End Get
        Set(ByVal Value As String)
            mInaemJoven = CDbl(Value)
        End Set
    End Property
    Public Property InaemOtros As String
        Get
            Return mInaemOtros.ToString("##,##0.00")
            ' Return CStr(mEstecTest)
        End Get
        Set(ByVal Value As String)
            mInaemOtros = CDbl(Value)
        End Set
    End Property
    Public ReadOnly Property notaEstecform As String
        Get
            Dim num As Double
            num = mEstecTest + mEstecDinam + mEstecEntr
            Return num.ToString("##,##0.00")
        End Get
    End Property
    Public ReadOnly Property notaINAEM As String
        Get
            Dim num As Double
            num = mInaemMujer + mInaemDiscap + mInaemBajaCon + mInaemJoven + mInaemOtros
            Return num.ToString("##,##0.00")
        End Get
    End Property
End Class
