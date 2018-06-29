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
        Me.txtUserName = New System.Windows.Forms.TextBox()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.label4 = New System.Windows.Forms.Label()
        Me.txtHostName = New System.Windows.Forms.TextBox()
        Me.txtDomainName = New System.Windows.Forms.TextBox()
        Me.label3 = New System.Windows.Forms.Label()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.RenameDomain = New System.Windows.Forms.Button()
        Me.RenameHostname = New System.Windows.Forms.Button()
        Me.cmbOU = New System.Windows.Forms.ComboBox()
        Me.btnFatchOrg = New System.Windows.Forms.Button()
        Me.TreeView1 = New System.Windows.Forms.TreeView()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtUserName
        '
        Me.txtUserName.Location = New System.Drawing.Point(104, 103)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(122, 20)
        Me.txtUserName.TabIndex = 3
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(104, 135)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.Size = New System.Drawing.Size(122, 20)
        Me.txtPassword.TabIndex = 4
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(45, 135)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(53, 13)
        Me.label4.TabIndex = 20
        Me.label4.Text = "Password"
        '
        'txtHostName
        '
        Me.txtHostName.Location = New System.Drawing.Point(104, 74)
        Me.txtHostName.Name = "txtHostName"
        Me.txtHostName.Size = New System.Drawing.Size(122, 20)
        Me.txtHostName.TabIndex = 2
        '
        'txtDomainName
        '
        Me.txtDomainName.Location = New System.Drawing.Point(104, 47)
        Me.txtDomainName.Name = "txtDomainName"
        Me.txtDomainName.Size = New System.Drawing.Size(122, 20)
        Me.txtDomainName.TabIndex = 1
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(38, 76)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(60, 13)
        Me.label3.TabIndex = 16
        Me.label3.Text = "Host Name"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(38, 106)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(60, 13)
        Me.label2.TabIndex = 15
        Me.label2.Text = "User Name"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(24, 50)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(74, 13)
        Me.label1.TabIndex = 14
        Me.label1.Text = "Domain Name"
        '
        'RenameDomain
        '
        Me.RenameDomain.Location = New System.Drawing.Point(250, 44)
        Me.RenameDomain.Name = "RenameDomain"
        Me.RenameDomain.Size = New System.Drawing.Size(129, 23)
        Me.RenameDomain.TabIndex = 5
        Me.RenameDomain.Text = "Rename Domain"
        Me.RenameDomain.UseVisualStyleBackColor = True
        '
        'RenameHostname
        '
        Me.RenameHostname.Location = New System.Drawing.Point(250, 73)
        Me.RenameHostname.Name = "RenameHostname"
        Me.RenameHostname.Size = New System.Drawing.Size(129, 23)
        Me.RenameHostname.TabIndex = 6
        Me.RenameHostname.Text = "Rename HostName"
        Me.RenameHostname.UseVisualStyleBackColor = True
        '
        'cmbOU
        '
        Me.cmbOU.FormattingEnabled = True
        Me.cmbOU.Location = New System.Drawing.Point(250, 103)
        Me.cmbOU.Name = "cmbOU"
        Me.cmbOU.Size = New System.Drawing.Size(129, 21)
        Me.cmbOU.TabIndex = 7
        '
        'btnFatchOrg
        '
        Me.btnFatchOrg.Location = New System.Drawing.Point(250, 132)
        Me.btnFatchOrg.Name = "btnFatchOrg"
        Me.btnFatchOrg.Size = New System.Drawing.Size(129, 32)
        Me.btnFatchOrg.TabIndex = 8
        Me.btnFatchOrg.Text = "Fetch organiational unit"
        Me.btnFatchOrg.UseVisualStyleBackColor = True
        '
        'TreeView1
        '
        Me.TreeView1.Location = New System.Drawing.Point(400, 44)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(491, 615)
        Me.TreeView1.TabIndex = 21
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(250, 228)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(129, 28)
        Me.Button2.TabIndex = 22
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(919, 675)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.TreeView1)
        Me.Controls.Add(Me.btnFatchOrg)
        Me.Controls.Add(Me.cmbOU)
        Me.Controls.Add(Me.RenameHostname)
        Me.Controls.Add(Me.RenameDomain)
        Me.Controls.Add(Me.txtUserName)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.txtHostName)
        Me.Controls.Add(Me.txtDomainName)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Name = "Form1"
        Me.Text = "LH"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents txtUserName As System.Windows.Forms.TextBox
    Private WithEvents txtPassword As System.Windows.Forms.TextBox
    Private WithEvents label4 As System.Windows.Forms.Label
    Private WithEvents txtHostName As System.Windows.Forms.TextBox
    Private WithEvents txtDomainName As System.Windows.Forms.TextBox
    Private WithEvents label3 As System.Windows.Forms.Label
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents RenameDomain As System.Windows.Forms.Button
    Friend WithEvents RenameHostname As System.Windows.Forms.Button
    Friend WithEvents cmbOU As System.Windows.Forms.ComboBox
    Friend WithEvents btnFatchOrg As System.Windows.Forms.Button
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents Button2 As System.Windows.Forms.Button

End Class
