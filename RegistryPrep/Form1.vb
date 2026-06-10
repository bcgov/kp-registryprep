Imports System.Collections.ObjectModel
Imports System.Text.RegularExpressions
Imports System.Configuration

Public Class Form1
  Dim sourceServerPath = ConfigurationManager.AppSettings("SourceServerPath")
  Dim destinationServerPath = ConfigurationManager.AppSettings("DestinationServerPath")
  Dim currPath = destinationServerPath & ConfigurationManager.AppSettings("CurrentPath")

  Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    status("Program is Ready")
  End Sub

  Sub status(ByVal newtext As String)
    txtStatus.Text = txtStatus.Text & vbCrLf & newtext
    txtStatus.Select(txtStatus.Text.Length - newtext.ToString.Length, 0)
    txtStatus.ScrollToCaret()
    txtStatus.Update()
  End Sub

  Private Sub procXML_Click(sender As Object, e As EventArgs) Handles procXML.Click
    status("Processing Registry XML Files")

    Dim newxml = ""
    Dim filecontents

    Dim files As ReadOnlyCollection(Of String)
    files = My.Computer.FileSystem.GetFiles(currPath & ConfigurationManager.AppSettings("XMLWorkPath"), FileIO.SearchOption.SearchAllSubDirectories, "*.xml")

    For Each file In files
      filecontents = My.Computer.FileSystem.ReadAllText(file)
      newxml = newxml & vbCrLf & filecontents
    Next

    Dim xmlHeader = My.Computer.FileSystem.ReadAllText(currPath & ConfigurationManager.AppSettings("XMLHeaderFile"))

    xmlHeader = Regex.Replace(xmlHeader, "~registryDate~", getMonth(CDate(RegistryDate.Text).Month) & " " & CDate(RegistryDate.Text).Day & ", " & CDate(RegistryDate.Text).Year)
    xmlHeader = Regex.Replace(xmlHeader, "~registryVol~", registryVol.Text)
    xmlHeader = Regex.Replace(xmlHeader, "~registryOutputDir~", getMonth(CDate(RegistryDate.Text).Month) & " " & CDate(RegistryDate.Text).Day)

    newxml = Regex.Replace(newxml, "^\s*<public_notice>", xmlHeader)
    newxml = Regex.Replace(newxml, "</public_notice>\s*<public_notice>", "")

    My.Computer.FileSystem.WriteAllText(currPath & ConfigurationManager.AppSettings("CombinedXMLFile"), newxml, False)

    Dim targetFolder = destinationServerPath & ConfigurationManager.AppSettings("WorkingPath") & getMonth(CDate(RegistryDate.Text).Month) & " " & CDate(RegistryDate.Text).Day
    If Not My.Computer.FileSystem.DirectoryExists(targetFolder) Then
      My.Computer.FileSystem.CreateDirectory(targetFolder)
    End If

    runXSL(currPath & ConfigurationManager.AppSettings("CombinedXMLFile"), currPath & ConfigurationManager.AppSettings("CrnXslFile"), currPath & ConfigurationManager.AppSettings("OutputXMLFile"))

  End Sub

  Function runXSL(ByVal origXML, ByVal XSLT, ByVal outputFile)
    Dim myProcess As Process = New Process()

    Dim runcmd = "java -jar " & currPath & ConfigurationManager.AppSettings("SaxonFile") & " -s:""" & origXML & """ -xsl:""" & XSLT & """ -o:""" & outputFile & """"

    Dim sysFolder As String = System.Environment.GetFolderPath(Environment.SpecialFolder.System)
    '
    ' Create the command line. 
    '
    myProcess.StartInfo.FileName = ConfigurationManager.AppSettings("CmdFile")
    myProcess.StartInfo.Arguments = "/C " & ConfigurationManager.AppSettings("ComputerDrive") & " && " & runcmd & " && exit"
    '
    ' Start the process in a hidden window.
    ' 
    myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
    myProcess.StartInfo.CreateNoWindow = True
    myProcess.Start()
    '
    ' Kill the process if it doesn't finish in one minute.
    ' 
    'myProcess.WaitForExit(1)

    Dim timeTick = 0

    While Not myProcess.HasExited
      myProcess.Refresh()
    End While

    If Not myProcess.HasExited Then
      myProcess.Kill()
      Console.WriteLine("ERROR: XML did not translate")
      Return False
    End If

    myProcess.Close()

    While Not My.Computer.FileSystem.FileExists(outputFile)
      Console.Write(".")
      Threading.Thread.Sleep(100)
    End While

    Return True
  End Function

  Function getMonth(monthNum)
    If monthNum = 1 Then
      Return "January"
    ElseIf monthNum = 2 Then
      Return "February"
    ElseIf monthNum = 3 Then
      Return "March"
    ElseIf monthNum = 4 Then
      Return "April"
    ElseIf monthNum = 5 Then
      Return "May"
    ElseIf monthNum = 6 Then
      Return "June"
    ElseIf monthNum = 7 Then
      Return "July"
    ElseIf monthNum = 8 Then
      Return "August"
    ElseIf monthNum = 9 Then
      Return "September"
    ElseIf monthNum = 10 Then
      Return "October"
    ElseIf monthNum = 11 Then
      Return "November"
    ElseIf monthNum = 12 Then
      Return "December"
    Else
      Return ""
    End If

  End Function

  Function getPDFText(ByVal origPDF As String, ByVal destTXT As String)

    Dim myProcess As Process = New Process()
    Dim runcmd = currPath & ConfigurationManager.AppSettings("AgentFile") & " --src=""" & origPDF & """ --dest=""" & destTXT & """"

    'Dim sysFolder As String = System.Environment.GetFolderPath(Environment.SpecialFolder.System)

    '
    ' Create the command line. 
    '
    status("***Running: " & runcmd)

    Try
      myProcess.StartInfo.FileName = ConfigurationManager.AppSettings("CmdFile")
      myProcess.StartInfo.Arguments = "/C " & ConfigurationManager.AppSettings("ComputerDrive") & " && " & runcmd & " && exit"
      '
      ' Start the process in a hidden window.
      ' 
      myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Maximized
      myProcess.StartInfo.UseShellExecute = False
      myProcess.StartInfo.CreateNoWindow = True
      myProcess.Start()
      '
      ' Kill the process if it doesn't finish in one minute.
      ' 
      myProcess.WaitForExit(1)

      Dim timeTick = 0

      While Not myProcess.HasExited
          myProcess.Refresh()
      End While

      If Not myProcess.HasExited Then
          myProcess.Kill()
          Console.WriteLine("ERROR: PDF did not convert to TXT")
          Return False
      End If

      myProcess.Close()


    Catch ex As Exception
      status("")
      status("---ERROR---")
      status("Exception: " & ex.Message)
      status("---ERROR---")
    End Try

    While Not My.Computer.FileSystem.FileExists(destTXT)
      status(".")
      Threading.Thread.Sleep(1000)
    End While

    Threading.Thread.Sleep(2000)

    Return True
  End Function

  Private Sub processPDF_Click(sender As Object, e As EventArgs) Handles processPDF.Click
    Dim options As RegexOptions = RegexOptions.IgnorePatternWhitespace
    options = RegexOptions.Singleline

    Dim files As ReadOnlyCollection(Of String)
    Dim allPDFText As String
    Dim destTXT As String
    Dim fileContents As String
    Dim aryCRN(100, 2)
    Dim indCRN As Integer
    Dim destHTML As String
    Dim htmlTEXT As String

    Dim aryPDF As Array
    Dim indPDF As Integer

    Dim captureFlag = False

    If String.IsNullOrWhiteSpace(UserNameTextBox.Text.Trim()) Or String.IsNullOrWhiteSpace(PasswordTextBox.Text.Trim()) Then
      MsgBox("Please enter your IDIR user name and password.")
    Else
      Try
        allPDFText = ""
        indCRN = 0
        status("Clearing PDF Work Directory")
        files = My.Computer.FileSystem.GetFiles(currPath & ConfigurationManager.AppSettings("PDFWorkPath"), FileIO.SearchOption.SearchAllSubDirectories, "*.*")
        For Each file In files
          My.Computer.FileSystem.DeleteFile(file, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.DeletePermanently)
        Next

        status("Grabbing PDFs")
        Open_Remote_Connection(UserNameTextBox.Text, PasswordTextBox.Text)

        files = My.Computer.FileSystem.GetFiles(sourceServerPath, FileIO.SearchOption.SearchTopLevelOnly, "*.pdf")

        For Each file In files
          status("Copying: " & file.Substring(file.LastIndexOf("\") + 1))
          My.Computer.FileSystem.CopyFile(file, currPath & ConfigurationManager.AppSettings("PDFWorkPath") & "\" & file.Substring(file.LastIndexOf("\") + 1))
        Next

        status("Loading Headers")
        Dim fileCRNHeader = My.Computer.FileSystem.ReadAllText(currPath & ConfigurationManager.AppSettings("CrnHeaderFile"))
        Dim CRNHeaders = Split(fileCRNHeader, vbCrLf)

        status("Processing Registry PDF Files")

        files = My.Computer.FileSystem.GetFiles(currPath & ConfigurationManager.AppSettings("PDFWorkPath"), FileIO.SearchOption.SearchAllSubDirectories, "*.pdf")

        For Each file In files
          status("   Processing: " & file.Substring(file.LastIndexOf("\") + 1))

          destTXT = currPath & ConfigurationManager.AppSettings("PDFWorkPath") & "\" & file.Substring(file.LastIndexOf("\") + 1)

          'status("   Debug1: " & destTXT)

          destTXT = destTXT.Substring(0, destTXT.Length - 3) & "txt"

          'status("   Debug2: " & destTXT)

          Try
            allPDFText = allPDFText & getPDFText(file, destTXT)
          Catch ex As Exception
            status("")
            status("---ERROR---")
            status("Exception: " & ex.Message)
            status("---ERROR---")
          End Try


          'status("   Debug3: " & destTXT)

          status("   Parcing PDF Text")
          fileContents = My.Computer.FileSystem.ReadAllText(destTXT)

          fileContents = Regex.Replace(fileContents, "^.*?", "", options)
          fileContents = Regex.Replace(fileContents, "Mailing.Address:.*?$", "", options)
          fileContents = fileContents.Replace(vbCrLf & "             ", " ")
          fileContents = fileContents.Replace("", "")
          fileContents = fileContents.Replace(vbCrLf & vbCrLf, vbCrLf)
          'fileContents = fileContents.Replace("Business Corporations Act", "<em>Business Corporations Act</em>")

          aryPDF = Split(fileContents, vbCrLf)
          indPDF = 0

          While indPDF < aryPDF.GetUpperBound(0)

            Dim currline = aryPDF(indPDF)

            If isHeader(currline, CRNHeaders) Then
              If captureFlag Then
                indCRN = indCRN + 1
              End If

              status("   Found: " & Trim(aryPDF(indPDF)))

              If aryPDF(indPDF + 1) = "" Or aryPDF(indPDF + 1).ToString.Contains("The Registrar of Companies") Then
                aryCRN(indCRN, 0) = Trim(aryPDF(indPDF)).ToString.Replace("FULL ", "")
              Else
                aryCRN(indCRN, 0) = Trim(aryPDF(indPDF)).ToString.Replace("FULL ", "") & " " & Trim(aryPDF(indPDF + 1)).ToString
              End If

              captureFlag = True
            End If

            If captureFlag Then
              Dim checkCRN = aryCRN(indCRN, 1)
              Dim checkCurrline = aryPDF(indPDF)

              aryCRN(indCRN, 1) = aryCRN(indCRN, 1) & Trim(aryPDF(indPDF)) & vbCrLf

            End If

            indPDF = indPDF + 1

          End While

          indCRN = indCRN + 1
          captureFlag = False
        Next

        aryCRN = SortArray(aryCRN)
        aryCRN = combineRegCanc(aryCRN)
        aryCRN = SortArray(aryCRN)

        indCRN = 0

        Dim htmlHeader = My.Computer.FileSystem.ReadAllText(currPath & ConfigurationManager.AppSettings("HtmlHeaderFile"))
        Dim htmlFooter = My.Computer.FileSystem.ReadAllText(currPath & ConfigurationManager.AppSettings("HtmlFooterFile"))

        htmlHeader = Regex.Replace(htmlHeader, "~registryDate~", getMonth(CDate(RegistryDate.Text).Month) & " " & CDate(RegistryDate.Text).Day & ", " & CDate(RegistryDate.Text).Year)
        htmlHeader = Regex.Replace(htmlHeader, "~registryVol~", registryVol.Text)

        While aryCRN(indCRN, 0) <> ""
          htmlTEXT = HTMLformat(aryCRN(indCRN, 1))

          destHTML = currPath & ConfigurationManager.AppSettings("OutputPath") & numpad(indCRN + 1) & ".htm"

          My.Computer.FileSystem.WriteAllText(destHTML, htmlHeader & vbCrLf & htmlTEXT & vbCrLf & htmlFooter, False)

          indCRN = indCRN + 1
        End While

        status("Moving Files to New Dir")

        Dim newDir = destinationServerPath & ConfigurationManager.AppSettings("CorporationsPath") & CDate(RegistryDate.Text).Year & "\" & getMonth(CDate(RegistryDate.Text).Month).ToString.Substring(0, 3) & numpad(CDate(RegistryDate.Text).Day)

        If Not My.Computer.FileSystem.DirectoryExists(newDir) Then
          My.Computer.FileSystem.CreateDirectory(newDir.ToLower)
        End If

        files = My.Computer.FileSystem.GetFiles(currPath & ConfigurationManager.AppSettings("OutputPathShort"), FileIO.SearchOption.SearchTopLevelOnly, "*.htm")

        For Each file In files
          My.Computer.FileSystem.MoveFile(file, newDir & "\" & file.Substring(file.LastIndexOf("\") + 1), True)
        Next

        openRegDir.Text = "Open New " & newDir.Substring(newDir.LastIndexOf("\") + 1) & " Dir"
        openRegDir.Enabled = True

        Close_Remote_Connection("net use " & sourceServerPath & "\ /delete /yes")

        status("Done!")
      Catch ex As Exception
        status("Error: " & ex.Message)
        MsgBox("Error: " & ex.Message)
      End Try
    End If
  End Sub

  Function combineRegCanc(aryCRN As Array)
    Dim index = 0
    Dim newTXT As String
    Dim newTXT2 As String

    Dim upper = aryCRN.GetUpperBound(0)

    While Not IsNothing(aryCRN(index + 1, 0))
      If aryCRN(index, 0).contains("REGISTRATIONS CANCELLED") And aryCRN(index + 1, 0).contains("REGISTRATIONS CANCELLED") Then

        newTXT = aryCRN(index + 1, 1) & vbCrLf & aryCRN(index, 1)

        newTXT2 = Regex.Replace(newTXT, "(REGISTRATIONS.CANCELLED.*?)REGISTRATIONS.CANCELLED.*?Business.Corporations.Act.", "$1", RegexOptions.Singleline)
        newTXT2 = Regex.Replace(newTXT2, "^Registration.Number.*$", "", RegexOptions.Multiline)
        newTXT2 = Regex.Replace(newTXT2, "(\n)\n+", "$1", RegexOptions.Singleline)
        newTXT2 = newTXT2.Replace(vbCrLf & vbCrLf, vbCrLf)
        newTXT2 = newTXT2.Replace(vbCrLf & vbCrLf, vbCrLf)
        newTXT2 = newTXT2.Replace("Section 397", "")

        aryCRN(index, 1) = newTXT2

        index = index + 1
        While Not IsNothing(aryCRN(index + 1, 0))
          aryCRN(index, 0) = aryCRN(index + 1, 0)
          aryCRN(index, 1) = aryCRN(index + 1, 1)

          index = index + 1
        End While

        aryCRN(index, 0) = ""
        aryCRN(index, 1) = ""

        index = index + 1
      End If
      index = index + 1
    End While

    Return aryCRN

  End Function

  Function isHeader(currline As String, CRNHeaders As Array)

    Dim index = 0
    Dim foundHeader = False

    currline = Trim(currline.ToUpper)

    While index <= CRNHeaders.GetUpperBound(0)
      If currline = Trim(CRNHeaders(index).ToString.ToUpper) And Trim(currline) <> "" Then
        foundHeader = True
      End If
      index = index + 1
    End While

    Return foundHeader

  End Function

  Function SortArray(arytoSort)
    Dim aryIND = 0
    Dim aryIND2 = 0
    Dim strTemp1
    Dim strTemp2

    While arytoSort(aryIND, 0) <> ""
      aryIND2 = aryIND + 1

      While arytoSort(aryIND2, 0) <> ""
        If arytoSort(aryIND, 0).ToString > arytoSort(aryIND2, 0).ToString Then
          strTemp1 = arytoSort(aryIND, 0)
          strTemp2 = arytoSort(aryIND, 1)

          arytoSort(aryIND, 0) = arytoSort(aryIND2, 0)
          arytoSort(aryIND, 1) = arytoSort(aryIND2, 1)

          arytoSort(aryIND2, 0) = strTemp1
          arytoSort(aryIND2, 1) = strTemp2
        End If
        aryIND2 = aryIND2 + 1
      End While

      aryIND = aryIND + 1
    End While

    Return arytoSort
  End Function

  Function numpad(anumber As String)
    If anumber.Length = 1 Then
      Return "0" & anumber
    Else
      Return anumber
    End If

  End Function

  Function HTMLformat(htmltext)
    Dim options As RegexOptions = RegexOptions.Singleline

    Dim aryHTML = Split(htmltext, vbCrLf)
    Dim indHTML As Integer = 0
    Dim newHTML As String = ""

    Dim foundDate As Boolean = False
    Dim addNBSP As Boolean = False

    While indHTML < aryHTML.GetUpperBound(0)

      Dim currline = aryHTML(indHTML)

      If indHTML = 0 Then
        newHTML = newHTML & "<h4>" & Trim(aryHTML(indHTML)).Replace("FULL ", "") & "</h4>" & vbCrLf

        If aryHTML(indHTML + 1) = "" Then
          newHTML = newHTML & "<p>"
          indHTML = indHTML + 2
        ElseIf aryHTML(indHTML + 1).Contains("The Registrar of Companies") Then
          newHTML = newHTML & "<p>"
          indHTML = indHTML + 1
        Else
          newHTML = newHTML & "<p><b>" & Trim(aryHTML(indHTML + 1)) & "</b></p>" & vbCrLf
          newHTML = newHTML & "<p>"
          indHTML = indHTML + 2
        End If

      ElseIf foundDate Then
        'newHTML = newHTML & "<tr><td valign=""top"">" & Replace(aryHTML(indHTML), "      ", "</td><td valign=top>") & "</td></tr>" & vbCrLf
        If IsDate(Trim(aryHTML(indHTML))) Then
          'status("   Found Date: " & Trim(aryHTML(indHTML)))
          newHTML = newHTML & "<tr><td colspan=""2""><b>" & Trim(aryHTML(indHTML)) & "</b></td></tr>" & vbCrLf
        Else
          If aryHTML(indHTML) <> "" Then
            If (aryHTML(indHTML).Contains("   ") Or aryHTML(indHTML).Substring(0, 3) = "LLC") And aryHTML(indHTML).Substring(0, 2) <> "  " Then
              If addNBSP Then
                newHTML = newHTML & "<tr><td valign=""top"" style=""width:75px"">" & Regex.Replace(aryHTML(indHTML), "^(.*?\b)\s\s\s+", "$1</td><td valign=""top"">") & "</td></tr>" & vbCrLf
                addNBSP = False
              Else
                If aryHTML(indHTML).Substring(0, 3) = "LLC" Then
                  newHTML = newHTML & "<tr><td valign=""top"" style=""width:75px"">" & Regex.Replace(aryHTML(indHTML), "^(.*?\b)\s\s+", "$1</td><td valign=""top"">") & "</td></tr>" & vbCrLf
                Else
                  newHTML = newHTML & "<tr><td valign=""top"">" & Regex.Replace(aryHTML(indHTML), "^(.*?\b)\s\s\s+", "$1</td><td valign=""top"">") & "</td></tr>" & vbCrLf
                End If

              End If
            Else
              newHTML = Regex.Replace(newHTML, "</td></tr>\s*$", " " & aryHTML(indHTML) & "</td></tr>" & vbCrLf)
            End If
          End If
        End If
      End If

      If Not foundDate Then
        If IsDate(Trim(aryHTML(indHTML))) Then
          'status("   Found Date: " & Trim(aryHTML(indHTML)))

          newHTML = newHTML.Replace("Business Corporations Act", "<em>Business Corporations Act</em>")
          newHTML = newHTML & "</p>" & vbCrLf

          newHTML = newHTML & "<table cellpadding=""4"">" & vbCrLf
          newHTML = newHTML & "<tr><td colspan=""2""><b>" & Trim(aryHTML(indHTML)) & "</b></td></tr>" & vbCrLf

          addNBSP = True
          foundDate = True

          If aryHTML(indHTML + 1).Contains("Company Name") Then
            indHTML = indHTML + 1
          End If
        Else
          newHTML = newHTML & " " & Trim(aryHTML(indHTML))
        End If
      End If

      indHTML = indHTML + 1
    End While

    'Need to go back through the html and fix any data from Registrations Cancelled or Dissolutions which could have timestamps
    'put in the middle of long company names
    htmltext = ""
    htmltext = newHTML
    newHTML = ""
    aryHTML = Nothing
    aryHTML = Split(htmltext, vbCrLf)
    indHTML = 0
    Dim pattern As String = "([0-9]|0[0-9]|1[0-9]|2[0-3]):([0-5][0-9])\s*([AaPp][Mm])"
    Dim rgx As Regex = New Regex(pattern)
    Dim strRowEnd As String = "</td></tr>"
    Dim strPacificTimeText = "Pacific Time"

    While indHTML < aryHTML.GetUpperBound(0)
      Dim currline = aryHTML(indHTML)
      Dim matches As MatchCollection = rgx.Matches(currline)
      Dim match As Match = rgx.Match(currline)
      If match.Success And matches.Count = 1 Then
        Dim strTime = match.Value
        Dim arrCurrLine = Split(currline.TrimEnd(), strTime)

        If arrCurrLine.Length = 2 Then
          'If there is additional text after the timestamp other than the end cell/end row tags we might have found an issue
          If arrCurrLine(1).Trim() <> strRowEnd Then
            Dim strTempString = arrCurrLine(1).Replace(strRowEnd, "").Replace(",", "").Replace(".", "").Trim()
            'Ensure the text at the end of the line is not "Pacific Time" since we only need to move the timestamp when it is inserted in the middle of long company names
            If strTempString <> strPacificTimeText Then
              Dim strUpdatedCurrLine = arrCurrLine(0).TrimEnd() & " " & arrCurrLine(1).Replace(strRowEnd, "").Trim() & " " & strTime & " " & strPacificTimeText & "," & strRowEnd
              currline = strUpdatedCurrLine
            End If
          Else
            'Request in Jira CIVCONTENT-218 to append "Pacific Time," to timestamp in the Registrations Cancelled data
            Dim strUpdatedCurrLine = arrCurrLine(0).TrimEnd() & " " & strTime & " " & strPacificTimeText & "," & strRowEnd
            currline = strUpdatedCurrLine
          End If
        End If
      End If
      newHTML = newHTML & currline & vbCrLf
      indHTML = indHTML + 1
    End While

    newHTML = newHTML & "</table>"

    newHTML = newHTML.Replace("<p> ", "<p>")
    newHTML = Regex.Replace(newHTML, " +", " ")
    newHTML = Regex.Replace(newHTML, "<td valign=top>", "<td valign=""top"">")
    newHTML = Regex.Replace(newHTML, "&", "&#38;")

    Return newHTML
  End Function

  Function RegexCapture(ByVal origText, ByVal strToFind)
    Dim options As RegexOptions = RegexOptions.Singleline
    Dim regex As Regex = New Regex(strToFind, options)
    Dim input As String = origText

    Dim match As Match = regex.Match(input)

    Return match.Groups(1).ToString
  End Function

  Private Sub chkRepo_Click(sender As Object, e As EventArgs) Handles chkRepo.Click
    If String.IsNullOrWhiteSpace(UserNameTextBox.Text.Trim()) Or String.IsNullOrWhiteSpace(PasswordTextBox.Text.Trim()) Then
      MsgBox("Please enter your IDIR user name and password.")
    Else
      Open_Remote_Connection(UserNameTextBox.Text, PasswordTextBox.Text)
      Process.Start("explorer.exe", sourceServerPath & "\")
      Close_Remote_Connection("net use " & sourceServerPath & "\ /delete /yes")
    End If
  End Sub


  Private Sub openRegDir_Click(sender As Object, e As EventArgs) Handles openRegDir.Click
    Process.Start("explorer.exe", destinationServerPath & ConfigurationManager.AppSettings("CorporationsPath") & CDate(RegistryDate.Text).Year & "\" & getMonth(CDate(RegistryDate.Text).Month).ToString.Substring(0, 3) & numpad(CDate(RegistryDate.Text).Day))
  End Sub

  Public Sub Open_Remote_Connection(ByVal strUsername As String, ByVal strPassword As String)
    Try
      Dim procInfo As New ProcessStartInfo
      procInfo.FileName = "net"
      procInfo.Arguments = "use " & sourceServerPath & "\ /USER:" & strUsername & " " & strPassword
      procInfo.WindowStyle = ProcessWindowStyle.Hidden
      procInfo.CreateNoWindow = True

      Dim proc As New Process
      proc.StartInfo = procInfo
      proc.Start()
      proc.WaitForExit(15000)
    Catch ex As Exception
      MsgBox("Open_Remote_Connection" & vbCrLf & vbCrLf & ex.Message, 4096, "Error")
    End Try
  End Sub

  Public Sub Close_Remote_Connection(ByVal device As String)
    Shell("cmd.exe /c " & device, vbHidden)
  End Sub
End Class
