Imports System.Net
Imports System.IO

Public Class _default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim number As String = Trim(Request.QueryString("number"))
        Dim cos As String = Trim(Request.QueryString("cos"))
        Dim LocalUser As String = "DRCLocal"
        Dim LocalPass As String = "DRCLocal"
        Dim LocalRouteName As String = "SMPP-10.200.3.45:9001"
        Dim reply As String = ""
        Dim reply2 As String = ""


        If cos = "49" Then
            reply = "Bienvenue chez Africell,Votre numéro est 0" & number.Substring(3) & ".Tapez *111# pour decouvrir nos services,pour plus d'info appelez le 1010"
        ElseIf cos = "62" Then
            reply = "Bienvenue chez Africell! Votre numéro 0" & number.Substring(3) & " vous avez reçu 10 SMS africell-africell.Tapez *111*100# pour vérifier votre Solde"
        ElseIf cos = "66" Then
            reply = "Cher client, pour cet achat du modem africell, votre numéro 0" & number.Substring(3) & " vous avez reçu 2 GB valide pour 30 jours. Pour vérifier votre balance faite le *111*100#"
        ElseIf cos = "82" Then
            reply = "Bien aimé dans le seigneur! Votre numéro 0" & number.Substring(3) & " vous avez reçu 4u Africell-Africell."
        ElseIf cos = "147" Then
            reply = "Bienvenue chez Africell! Votre numéro 0" & number.Substring(3) & ". Votre solde est de 30 u. Pour verifier tapez *1000*2#. Info 1010"
            reply2 = "Lelo sans conditions, sans forfait, tarif ya talo moke na RDC. Benga africell-africell na 6.1u pe 9.5u pona ba réseaux misusu facturés na seconde. mituna 1010"
            QuickProcessOut(number, LocalUser, LocalPass, LocalRouteName, reply2, "Africell")
        ElseIf cos = "156" Then
            reply = "Bienvenue chez Africell! Votre numéro 0" & number.Substring(3) & ". Votre solde est de 20 u. Pour verifier tapez *111*100#. Info 1010"
            reply2 = "Lelo sans conditions, sans forfait, tarif ya talo moke na RDC. Benga africell-africell na 6.1u pe 9.5u pona ba réseaux misusu facturés na seconde. mituna 1010"
            QuickProcessOut(number, LocalUser, LocalPass, LocalRouteName, reply2, "Africell")
        ElseIf cos = "157" Then
            reply = "Bienvenue chez Africell! Votre numéro 0" & number.Substring(3) & ". Votre solde est de 20 u. Pour verifier tapez *1000*2#. Info 1010"
        ElseIf cos = "154" Then
            reply = "Bienvenu chez Africell! Votre numéro est 0" & number.Substring(3) & ". Vous avez reçu 3 minutes et 200 MBs. Pour vérifier tapez *111*100#"
        ElseIf cos = "159" Then
            reply = "Bienvenue chez Africell! Votre numéro 0" & number.Substring(3) & " . Vous avez recu 100MBs comme cadeau. Pour verifier tapez *111*100#. Info 1010"
        ElseIf cos = "177" Then
            reply = "Super!!! Grace à cet achat, Africell vous offre 20 minutes et 200 SMS Africell-Africell Valable 30 jours. Tapez *111*100# pour vérifier votre solde"
        ElseIf cos = "242" Or cos = "248" Then
            reply = "Beaucoup de Surprises ! Gagnez des unités ou des mégas ou des SMS avec Africell ! Choisissez votre propre cadeau en rechargeant ! Africell votre réseau."
            reply2 = "Surprises avec Africell ! Rechargez 20u ou plus avant le Dimanche et recevez votre bonus en répondant 1 pour les unités ou 2 pour les mégas ou 3 pour les SMS."
            QuickProcessOut(number, LocalUser, LocalPass, LocalRouteName, reply2, "Africell")
        Else
            reply = "Bienvenue chez Africell !!!Votre numero est 0" & number.Substring(3) & " vous avez reçu 10u Africell-Africell et 10 MB,Tapez *111*100# pour verifiez votre solde"
            
        End If
        QuickProcessOut(number, LocalUser, LocalPass, LocalRouteName, reply, "Africell")
        Response.Write("0")
    End Sub

    Public Function QuickProcessOut(ByVal DestinationMobile As String, ByVal SMPPUser As String, ByVal SMPPPass As String, ByVal RouteName As String, ByVal message As String, ByVal sender As String) As String
        Dim strQS As String = "http://10.100.11.22:8800/?"
        message = message.Replace("#", "%23")
        Dim CurrentDT As DateTime
        Dim SMSPushLength As System.TimeSpan
        Dim RSTime As Integer = 0
        Dim RSTimeMS As Integer = 0
        Try
            strQS = strQS & "PhoneNumber=" & DestinationMobile
            strQS = strQS & "&Text=" & message
            strQS = strQS & "&DCS=0"
            strQS = strQS & "&Sender=" & sender
            strQS = strQS & "&RouteName=" & RouteName
            strQS = strQS & "&User=" & SMPPUser
            strQS = strQS & "&Password=" & SMPPPass
            ' strQS = strQS & "&ReceiptRequested=No"
            Dim HttpWReq As HttpWebRequest
            Dim HttpWResp As HttpWebResponse
            Dim streamResponse As Stream
            Dim streamRead As StreamReader
            Dim HTTPWebRespStr As String = ""
            Dim httpWebR As String = ""
            CurrentDT = Now

            HttpWReq = CType(WebRequest.Create(strQS), HttpWebRequest)
            HttpWReq.Method = WebRequestMethods.Http.Get
            HttpWReq.Timeout = 120 * 1000 ' 120 seconds
            HttpWResp = CType(HttpWReq.GetResponse(), HttpWebResponse)
            If HttpWReq.HaveResponse Then
                streamResponse = HttpWResp.GetResponseStream()
                streamRead = New StreamReader(streamResponse)
                HTTPWebRespStr = CStr(streamRead.ReadToEnd())
                streamResponse.Close()
                streamRead.Close()
                HttpWResp.Close()
            End If
            SMSPushLength = Now.Subtract(CurrentDT)
            RSTimeMS = SMSPushLength.TotalMilliseconds
            RSTime = (RSTimeMS / 1000)
            Dim HTTPWebRespStrNoNewLineIn As String = Replace(HTTPWebRespStr, vbNewLine, "", , , vbTextCompare)
            HTTPWebRespStrNoNewLineIn = Replace(HTTPWebRespStrNoNewLineIn, "<br>", "")
            If HTTPWebRespStrNoNewLineIn.Length > 87 Then
                HTTPWebRespStrNoNewLineIn = HTTPWebRespStrNoNewLineIn.Substring(0, 87)
            End If
            httpWebR = HTTPWebRespStrNoNewLineIn
            If httpWebR.Length > 49 Then
                httpWebR = HTTPWebRespStrNoNewLineIn.Substring(0, 49)
            End If

            Dim _ERRORCODE As String = ""
            If _ERRORCODE.IndexOf("|" + Trim(HTTPWebRespStrNoNewLineIn) + "|") = -1 Then 'sucess
                Return "1"
            End If
        Catch ex As Exception

        End Try
        Return "-11"
    End Function
End Class