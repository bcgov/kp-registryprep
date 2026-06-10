<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtStatus = New System.Windows.Forms.TextBox()
        Me.procXML = New System.Windows.Forms.Button()
        Me.RegistryDate = New System.Windows.Forms.DateTimePicker()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.registryVol = New System.Windows.Forms.TextBox()
        Me.processPDF = New System.Windows.Forms.Button()
        Me.chkRepo = New System.Windows.Forms.Button()
        Me.openRegDir = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.UserNameTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.PasswordTextBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtStatus
        '
        Me.txtStatus.Location = New System.Drawing.Point(16, 177)
        Me.txtStatus.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtStatus.Multiline = True
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(595, 379)
        Me.txtStatus.TabIndex = 0
        '
        'procXML
        '
        Me.procXML.Enabled = False
        Me.procXML.Location = New System.Drawing.Point(417, 63)
        Me.procXML.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.procXML.Name = "procXML"
        Me.procXML.Size = New System.Drawing.Size(195, 28)
        Me.procXML.TabIndex = 1
        Me.procXML.Text = "Process XML Files"
        Me.procXML.UseVisualStyleBackColor = True
        '
        'RegistryDate
        '
        Me.RegistryDate.Location = New System.Drawing.Point(155, 98)
        Me.RegistryDate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RegistryDate.Name = "RegistryDate"
        Me.RegistryDate.Size = New System.Drawing.Size(253, 22)
        Me.RegistryDate.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 106)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 17)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Registry Date"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(16, 74)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(126, 17)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Registry Volume"
        '
        'registryVol
        '
        Me.registryVol.Location = New System.Drawing.Point(155, 66)
        Me.registryVol.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.registryVol.Name = "registryVol"
        Me.registryVol.Size = New System.Drawing.Size(253, 22)
        Me.registryVol.TabIndex = 5
        '
        'processPDF
        '
        Me.processPDF.Location = New System.Drawing.Point(417, 98)
        Me.processPDF.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.processPDF.Name = "processPDF"
        Me.processPDF.Size = New System.Drawing.Size(195, 28)
        Me.processPDF.TabIndex = 6
        Me.processPDF.Text = "Process PDF Files"
        Me.processPDF.UseVisualStyleBackColor = True
        '
        'chkRepo
        '
        Me.chkRepo.Location = New System.Drawing.Point(155, 130)
        Me.chkRepo.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.chkRepo.Name = "chkRepo"
        Me.chkRepo.Size = New System.Drawing.Size(255, 28)
        Me.chkRepo.TabIndex = 7
        Me.chkRepo.Text = "Check Repository"
        Me.chkRepo.UseVisualStyleBackColor = True
        '
        'openRegDir
        '
        Me.openRegDir.Enabled = False
        Me.openRegDir.Location = New System.Drawing.Point(417, 130)
        Me.openRegDir.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.openRegDir.Name = "openRegDir"
        Me.openRegDir.Size = New System.Drawing.Size(195, 28)
        Me.openRegDir.TabIndex = 8
        Me.openRegDir.Text = "Open New Reg Dir"
        Me.openRegDir.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(93, 17)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "User Name:"
        '
        'UserNameTextBox
        '
        Me.UserNameTextBox.Location = New System.Drawing.Point(155, 9)
        Me.UserNameTextBox.Name = "UserNameTextBox"
        Me.UserNameTextBox.Size = New System.Drawing.Size(253, 22)
        Me.UserNameTextBox.TabIndex = 10
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 7.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(16, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 17)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Password:"
        '
        'PasswordTextBox
        '
        Me.PasswordTextBox.Location = New System.Drawing.Point(155, 37)
        Me.PasswordTextBox.Name = "PasswordTextBox"
        Me.PasswordTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.PasswordTextBox.Size = New System.Drawing.Size(253, 22)
        Me.PasswordTextBox.TabIndex = 12
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(621, 636)
        Me.Controls.Add(Me.PasswordTextBox)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.UserNameTextBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.openRegDir)
        Me.Controls.Add(Me.chkRepo)
        Me.Controls.Add(Me.processPDF)
        Me.Controls.Add(Me.registryVol)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.RegistryDate)
        Me.Controls.Add(Me.procXML)
        Me.Controls.Add(Me.txtStatus)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "Form1"
        Me.Text = "Registry Preparation App"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtStatus As System.Windows.Forms.TextBox
    Friend WithEvents procXML As System.Windows.Forms.Button
    Friend WithEvents RegistryDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents registryVol As System.Windows.Forms.TextBox
    Friend WithEvents processPDF As System.Windows.Forms.Button
    Friend WithEvents chkRepo As System.Windows.Forms.Button
    Friend WithEvents openRegDir As System.Windows.Forms.Button
    Friend WithEvents Label3 As Label
    Friend WithEvents UserNameTextBox As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents PasswordTextBox As TextBox
End Class
