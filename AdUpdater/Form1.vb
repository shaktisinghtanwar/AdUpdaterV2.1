Imports System.DirectoryServices
Imports System.Management
Imports Microsoft.Win32

Public Class Form1

    Private Sub button1_Click(sender As Object, e As EventArgs)
        HandleJoinandRenameDomain(txtDomainName.Text, txtHostName.Text, txtUserName.Text, txtPassword.Text)
    End Sub

    Public Shared Function GetComputerName() As String
        Dim szRet As String = ""
        Dim objMC As ManagementClass
        Dim objMOC As ManagementObjectCollection
        Try
            ' caller doesn't have to catch
            objMC = New ManagementClass("Win32_ComputerSystem")
            objMOC = objMC.GetInstances
        Catch e As ManagementException
            MessageBox.Show("Caught management exception looking up WMI - name not available. ({0})", e.Message)
            Return szRet
        Catch e As Exception
            MessageBox.Show("Caught unexpected exception looking up WMI - name not available. ({0})", e.Message)
            Return szRet
        End Try

        For Each objMO As ManagementObject In objMOC
            If (Not (objMO) Is Nothing) Then
                szRet = objMO("Name").ToString
                Exit For
            End If

        Next
        Return szRet
    End Function



    Public Sub HandleJoinandRenameDomain(ByVal szDomain As String, ByVal szNewHostname As String, ByVal szUsername As String, ByVal szPassword As String)


        Dim szCurrentHostname As String = GetComputerName()
        Dim szCurrent As String = GetDomainName()
        ' need to try and set hostname before domain join since it cannot be done beforehand (tried)
        Dim objMO As ManagementObject = New ManagementObject(("Win32_ComputerSystem.Name='" _
                        + (szCurrentHostname + "'")))
        If (Not (objMO) Is Nothing) Then
            Dim result As ManagementBaseObject
            ' documented at http://msdn.microsoft.com/en-us/library/aa392154%28VS.85%29.aspx
            objMO.Scope.Options.EnablePrivileges = True
            objMO.Scope.Options.Authentication = AuthenticationLevel.PacketPrivacy
            objMO.Scope.Options.Impersonation = ImpersonationLevel.Impersonate
            ' RenameComputer(szCurrentHostname, szNewHostname, objMO)
            'Unjoin From Workgroup first

            'Dim manage As ManagementObject = New ManagementObject(String.Format("Win32_ComputerSystem.Name='{0}'", Environment.MachineName))
            'Dim args As Object() = {"Workgroup", Nothing, Nothing, Nothing}
            'manage.InvokeMethod("UnJoinDomainOrWorkgroup", args)

            JoinComputerToDomain(szCurrent, szDomain, szCurrentHostname, szUsername, szPassword, objMO)
        Else
            MessageBox.Show("Join Domain: Failed to open computer management object.")
            Return
        End If



    End Sub

    Public Shared Function GetDomainName() As String
        Dim szRet As String = ""
        Dim objMC As ManagementClass
        Dim objMOC As ManagementObjectCollection
        Try
            ' caller does not have to catch
            objMC = New ManagementClass("Win32_ComputerSystem")
            objMOC = objMC.GetInstances
        Catch e As ManagementException
            MessageBox.Show("Caught management exception on getting domain name: {0}", e.Message)
            Return szRet
        Catch e As Exception
            MessageBox.Show("Caught unexpected exception on getting domain name: {0}", e.Message)
            Return szRet
        End Try

        For Each objMO As ManagementObject In objMOC
            If (Not (objMO) Is Nothing) Then
                If CType(objMO("partofdomain"), Boolean) Then
                    szRet = objMO("domain").ToString
                    MessageBox.Show("System is part of domain '{0}'", szRet)
                Else
                    MessageBox.Show("System is part of workgroup, no domain.")
                End If

            End If

        Next
        Return szRet
    End Function




    Private Sub JoinComputerToDomain(ByVal szCurrent As String, ByVal szDomain As String, ByVal szCurrentHostname As String, ByVal szUsername As String, ByVal szPassword As String, ByVal objMO As ManagementObject)
        '' JoinAndSetName(txtHostName.Text)
        'Dim domain As String = txtDomainName.Text
        'Dim password As String = txtPassword.Text
        'Dim username As String = txtUserName.Text
        'Dim OU As String = TreeView1.SelectedNode.Tag.ToString().Replace("LDAP://na.holcim.net/", "") '' "OU=Computers,DC=example,DC=com"
        'Dim managementObject As ManagementObject = New ManagementObject("Win32_ComputerSystem.Name='" + Environment.MachineName + "'")
        'managementObject.Scope.Options.Authentication = AuthenticationLevel.PacketPrivacy
        'managementObject.Scope.Options.Impersonation = ImpersonationLevel.Impersonate
        'managementObject.Scope.Options.EnablePrivileges = True
        'Dim mode As Integer = (1 Or 2)
        'Dim parameters() As String = New String() {domain, password, username, OU, mode}
        'Dim returnCode As Integer = Convert.ToInt32(managementObject.InvokeMethod("JoinDomainOrWorkgroup", parameters))
        'MessageBox.Show("Domain join has completed with code " & returnCode.ToString())
        'Return
        'If ("" <> szCurrent) Then
        '    If (szCurrent.ToUpper = szDomain.ToUpper) Then
        '        ' already correct 
        '        MessageBox.Show(("Join Domain: Already joined to Host: '" _
        '                        + (szCurrentHostname + ("' domain: '" _
        '                        + (szCurrent + "'")))))
        '        ' since if this was true then hostname could not have been changed so we are done.
        '        Return
        '    Else
        '        ' else, it's incorrect, send a failure (user must unjoin)      
        '        MessageBox.Show(("Join Domain: Failed: Already joined to '" _
        '                        + (szCurrent + "'. Can not attach to multiple domains.")))
        '        Return
        '    End If

        'End If



        Dim query As ManagementBaseObject
        'Try
        '    ' we catch
        '    query = objMO.GetMethodParameters("JoinDomainOrWorkgroup")
        'Catch ex As ManagementException
        '    MessageBox.Show("Join Domain: Failed to look up local computer for join")
        '    Return
        'End Try
        '' szCurrentHostname = Environment.MachineName
        Try
            MessageBox.Show(TreeView1.SelectedNode.Tag)
            Dim dirEntry As DirectoryEntry = New DirectoryEntry(TreeView1.SelectedNode.Tag, txtUserName.Text, txtPassword.Text)

            ''  MessageBox.Show("Joined Domain 1")

            ''dirEntry.Properties("name").Add(szCurrentHostname)

            'For Each item As String In dirEntry.Properties.PropertyNames
            '    Console.WriteLine(item)
            'Next

            MessageBox.Show("CN=" + szCurrentHostname)
            Dim newComputer As DirectoryEntry = dirEntry.Children.Add("CN=" + szCurrentHostname, "computer")
            newComputer.CommitChanges()
            newComputer.Properties("sAMAccountName").Value = szCurrentHostname + "$"
            newComputer.CommitChanges()
            newComputer.Properties("userAccountControl").Value = &H1000

            '' dirEntry.RefreshCache() '' refreshing all attributes here
            newComputer.CommitChanges()
            dirEntry.CommitChanges()
            ''   MessageBox.Show("Joined Domain 2")
            dirEntry.Close()

            MessageBox.Show("Joined Domain")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        'query("Name") = szDomain
        'query("Password") = szPassword
        'query("UserName") = szUsername
        'query("FJoinOptions") = (1 + (2 + 1024))
        '' + 256 (tried with and with 256
        'MessageBox.Show(String.Format("Attempting WMI method JoinDomainOrWorkgroup({0}, ***, {1}, null, {2}).", query("Name").ToString, query("UserName").ToString, query("FJoinOptions").ToString))
        'Try
        '    Dim result = objMO.InvokeMethod("JoinDomainOrWorkgroup", query, Nothing)
        '    If (0 <> CType(result("ReturnValue"), UInteger)) Then
        '        MessageBox.Show(("Join Domain: Failed (" _
        '                        + (CType(result("ReturnValue"), UInteger) + ") to execute join request")))
        '        Return
        '    End If

        'Catch e As ManagementException
        '    MessageBox.Show(("Join Domain: Failed (" _
        '                    + (CType(e.ErrorCode, UInteger) + (") to execute join request: " + e.Message))))
        '    Return
        'End Try
        MessageBox.Show("Joined Domain")
    End Sub

    Public Function JoinAndSetName(ByVal newName As String) As Boolean
        '' _lh.Log(LogHandler.LogType.Debug, String.Format("Joining domain and changing Machine Name from '{0}' to '{1}'...", Environment.MachineName, newName))
        ' Get WMI object for this machine
        Dim wmiObject As ManagementObject = New ManagementObject(New ManagementPath(("Win32_ComputerSystem.Name='" _
                            + (Environment.MachineName + "'"))))
        Try
            ' Obtain in-parameters for the method
            Dim inParams As ManagementBaseObject = wmiObject.GetMethodParameters("JoinDomainOrWorkgroup")
            inParams("Name") = txtDomainName.Text
            inParams("Password") = txtPassword.Text
            inParams("UserName") = txtUserName.Text
            inParams("FJoinOptions") = 3
            ' Magic number: 3 = join to domain and create computer account
            '' /''/ _lh.Log(LogHandler.LogType.Debug, String.Format("Joining machine to domain under name '{0}'...", inParams("Name")))
            ' Execute the method and obtain the return values.
            Dim joinParams As ManagementBaseObject = wmiObject.InvokeMethod("JoinDomainOrWorkgroup", inParams, Nothing)
            '' _lh.Log(LogHandler.LogType.Debug, String.Format("JoinDomainOrWorkgroup return code: '{0}'", joinParams("ReturnValue")))
            ' Did it work?
            If (CType(joinParams.Properties("ReturnValue").Value, UInteger) <> 0) Then
                ' Join to domain didn't work
                MessageBox.Show(String.Format("JoinDomainOrWorkgroup failed with return code: '{0}'", joinParams("ReturnValue")))
                Return False
            End If

        Catch e As ManagementException
            ' Join to domain didn't work
            MessageBox.Show(String.Format("Unable to join domain '{0}'", txtDomainName.Text))
            Return False
        End Try

        ' Join to domain worked - now change name
        Dim inputArgs As ManagementBaseObject = wmiObject.GetMethodParameters("Rename")
        inputArgs("Name") = newName
        inputArgs("Password") = "domain_account_password"
        inputArgs("UserName") = "domain_account"
        ' Set the name
        Dim nameParams As ManagementBaseObject = wmiObject.InvokeMethod("Rename", inputArgs, Nothing)
        '' _lh.Log(LogHandler.LogType.Debug, String.Format("Machine Rename return code: '{0}'", nameParams("ReturnValue")))
        If (CType(nameParams.Properties("ReturnValue").Value, UInteger) <> 0) Then
            ' Name change didn't work
            MessageBox.Show(String.Format("Unable to change Machine Name from '{0}' to '{1}'", Environment.MachineName, newName))
            Return False
        End If

        ' All ok
        Return True
    End Function

    Public Function EnumerateOU(ByVal OuDn As String) As ArrayList


        Dim alObjects As ArrayList = New ArrayList
        Try
            Dim directoryObject As DirectoryEntry = New DirectoryEntry("LDAP://" + OuDn, txtUserName.Text, txtPassword.Text)
            For Each child As DirectoryEntry In directoryObject.Children
                Dim childPath As String = child.Path.ToString
                alObjects.Add(childPath.Remove(0, 7))
                'remove the LDAP prefix from the path
                child.Close()
                child.Dispose()
            Next
            directoryObject.Close()
            directoryObject.Dispose()
        Catch e As DirectoryServicesCOMException
            Console.WriteLine(("An Error Occurred: " + e.Message.ToString))
        End Try

        Return alObjects
    End Function

    Public Sub AddComputerToOU(ByVal ldapPath As String, ByVal computerName As String, ByVal Hostname As String)
        Dim dirEntry As DirectoryEntry = New DirectoryEntry(ldapPath, txtUserName.Text, txtPassword.Text)
        Dim newComputer As DirectoryEntry = dirEntry.Children.Add("CN = Hostname", computerName)
        newComputer.Properties("sAMAccountName").Value = (Hostname + "$")
        newComputer.Properties("UserAccountControl").Value = 4128
        newComputer.CommitChanges()

    End Sub
    Public Shared Function SetMachineName(ByVal newName As String) As Boolean
        Dim key As RegistryKey = Registry.LocalMachine
        Dim activeComputerName As String = "SYSTEM\CurrentControlSet\Control\ComputerName\ActiveComputerName"
        Dim activeCmpName As RegistryKey = key.CreateSubKey(activeComputerName)
        activeCmpName.SetValue("ComputerName", newName)
        activeCmpName.Close()
        Dim computerName As String = "SYSTEM\CurrentControlSet\Control\ComputerName\ComputerName"
        Dim cmpName As RegistryKey = key.CreateSubKey(computerName)
        cmpName.SetValue("ComputerName", newName)
        cmpName.Close()
        Dim _hostName As String = "SYSTEM\CurrentControlSet\services\Tcpip\Parameters\"
        Dim hostName As RegistryKey = key.CreateSubKey(_hostName)
        hostName.SetValue("Hostname", newName)
        hostName.SetValue("NV Hostname", newName)
        hostName.Close()
        Return True
    End Function
    Private Sub RenameComputer(ByVal szCurrentHostname As String, ByVal szNewHostname As String, ByVal objMO As ManagementObject)

        Dim mo As ManagementObject = New ManagementObject("root\CIMV2", ("Win32_ComputerSystem.Name='" _
                + (System.Environment.MachineName + "'")), Nothing)
        Dim mbo As ManagementBaseObject = mo.GetMethodParameters("Rename")
        mbo("Name") = szNewHostname
        mbo("Password") = txtPassword.Text
        mbo("UserName") = txtUserName.Text
        Dim mbo2 As ManagementBaseObject = mo.InvokeMethod("Rename", mbo, Nothing)
        MessageBox.Show("New: {0}", mbo2("ReturnValue").ToString)
        If mbo2("ReturnValue") = "0" Then
            System.Diagnostics.Process.Start("shutdown", "/r /t 0")
        End If



        'Dim rc As Boolean = True

        'Try
        '    Dim machineNode As DirectoryEntry = Nothing
        '    machineNode = New DirectoryEntry("WinNT://" & szCurrentHostname)
        '    machineNode.Username = txtUserName.Text
        '    machineNode.Password = txtPassword.Text
        '    machineNode.AuthenticationType = AuthenticationTypes.Secure
        '    machineNode.Rename("CN=" & szNewHostname)
        '    machineNode.CommitChanges()
        'Catch e As Exception
        '    Dim msg As String = e.Message
        '    Dim stacktrace As String = e.StackTrace
        '    MessageBox.Show(e.Message)
        'End Try



        '' SetMachineName(szNewHostname)

        'Return



        'If ((szCurrentHostname.ToUpper <> szNewHostname.ToUpper) AndAlso ("" <> szNewHostname)) Then
        '    Try
        '        MessageBox.Show("Join Domain: Setting Hostname")
        '        Dim query2 As ManagementBaseObject
        '        query2 = objMO.GetMethodParameters("Rename")
        '        query2("Name") = szNewHostname
        '        query2("Password") = Nothing
        '        query2("UserName") = Nothing
        '        ' AddComputerToOU(szCurrentHostname, szNewHostname)
        '        MessageBox.Show("Attempting WMI method Rename({0}, ***).", query2("Name").ToString)
        '        Dim result = objMO.InvokeMethod("Rename", query2, Nothing)
        '        If (0 <> CType(result("ReturnValue"), UInteger)) Then
        '            MessageBox.Show("Failed to set Computer name, code " + result("ReturnValue").ToString())
        '            MessageBox.Show(("Failed to set Computer name, code " + result("ReturnValue").ToString))
        '        End If

        '        MessageBox.Show("Host name change request successful - ASM must be rebooted.")
        '    Catch e As InvalidOperationException
        '        MessageBox.Show(e.Message)
        '        MessageBox.Show(("Join Domain: Failed: " + e.Message))
        '        Return
        '    Catch e As ManagementException
        '        MessageBox.Show(e.Message)
        '        MessageBox.Show(("Join Domain: Failed change hostname: " + e.Message))
        '        Return
        '    End Try

        'End If

    End Sub


    Private Sub Rename_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub RenameHostname_Click(sender As Object, e As EventArgs) Handles RenameHostname.Click
        Dim szCurrentHostname As String = GetComputerName()
        Dim objMO As ManagementObject = New ManagementObject(("Win32_ComputerSystem.Name='" _
                + (szCurrentHostname + "'")))
        If (Not (objMO) Is Nothing) Then
            'Dim result As ManagementBaseObject
            ' documented at http://msdn.microsoft.com/en-us/library/aa392154%28VS.85%29.aspx
            objMO.Scope.Options.EnablePrivileges = True
            objMO.Scope.Options.Authentication = AuthenticationLevel.PacketPrivacy
            objMO.Scope.Options.Impersonation = ImpersonationLevel.Impersonate
            RenameComputer(szCurrentHostname, txtHostName.Text, objMO)

        End If
    End Sub
    Private Sub SurroundingSub()
        Dim manage As ManagementObject = New ManagementObject(String.Format("Win32_ComputerSystem.Name='{0}'", Environment.MachineName))
        Dim args As Object() = {"WorkgroupName", Nothing, Nothing, Nothing}
        manage.InvokeMethod("JoinDomainOrWorkgroup", args)
    End Sub
    Private Sub RenameDomain_Click(sender As Object, e As EventArgs) Handles RenameDomain.Click
        Dim manage As ManagementObject = New ManagementObject(String.Format("Win32_ComputerSystem.Name='{0}'", Environment.MachineName))
        Dim args As Object() = {txtDomainName.Text, Nothing, Nothing, Nothing}
        manage.InvokeMethod("JoinDomainOrWorkgroup", args)

      


                    'Dim szCurrentHostname As String = GetComputerName()
                    'Dim szCurrent As String = GetDomainName()
                    '' need to try and set hostname before domain join since it cannot be done beforehand (tried)
                    'Dim objMO As ManagementObject = New ManagementObject(("Win32_ComputerSystem.Name='" _
                    '                + (szCurrentHostname + "'")))
                    'If (Not (objMO) Is Nothing) Then
                    '    'Dim result As ManagementBaseObject
                    '    ' documented at http://msdn.microsoft.com/en-us/library/aa392154%28VS.85%29.aspx
                    '    objMO.Scope.Options.EnablePrivileges = True
                    '    objMO.Scope.Options.Authentication = AuthenticationLevel.PacketPrivacy
                    '    objMO.Scope.Options.Impersonation = ImpersonationLevel.Impersonate
                    '    JoinComputerToDomain(szCurrent, txtDomainName.Text, szCurrentHostname, txtUserName.Text, txtPassword.Text, objMO)
                    'End If
    End Sub

    Dim ds As DataTable

    Private Function EnumerateOURecursive(ByVal OuDn As String) As ArrayList
        'Dim rootDSE As DirectoryEntry = New DirectoryEntry("LDAP://RootDSE", txtUserName.Text, txtPassword.Text)
        'Dim defaultContext As String = rootDSE.Properties("defaultNamingContext")(0).ToString()
        Dim domainRoot As DirectoryEntry = New DirectoryEntry("LDAP://" & txtDomainName.Text, txtUserName.Text, txtPassword.Text)
        Dim ouSearcher As DirectorySearcher = New DirectorySearcher(domainRoot)
        ouSearcher.SearchScope = SearchScope.OneLevel
        ouSearcher.PropertiesToLoad.Add("ou")
        ouSearcher.Filter = "(objectCategory=organizationalUnit)"
        Dim alObjects As ArrayList = New ArrayList
        For Each deResult As SearchResult In ouSearcher.FindAll()
            Dim ouName As String = deResult.Properties("ou")(0).ToString()
            Dim tempNode As New TreeNode(ouName)
            tempNode.Tag = deResult.Path
            '' node.Nodes.Add(tempNode)

            '' TreeView1.Nodes.Add(GetChildNode(ouName, deResult.Path))
            TreeView1.Nodes.Add(tempNode)
            alObjects.Add(ouName)
            '' Return


            'Dim ADentry As DirectoryEntr
        Next


        Return alObjects
    End Function


    Private Function GetChildNode(ByRef node As TreeNode) As TreeNode
        '' Dim node As TreeNode = New TreeNode(OuDn)
        Dim ADentry = New DirectoryEntry(node.Tag, txtUserName.Text, txtPassword.Text)
        Dim ouSearcher As DirectorySearcher = New DirectorySearcher(ADentry)
        ouSearcher.SearchScope = SearchScope.OneLevel
        ouSearcher.PropertiesToLoad.Add("ou")
        ouSearcher.Filter = "(objectCategory=organizationalUnit)"
        Try
            Dim alObjects As ArrayList = New ArrayList
            For Each deResult As SearchResult In ouSearcher.FindAll()
                Dim ouName As String = deResult.Properties("ou")(0).ToString()
                Dim tempNode As New TreeNode(ouName)
                tempNode.Tag = deResult.Path
                node.Nodes.Add(tempNode)



                'Dim ADentry As DirectoryEntr
            Next
        Catch ex As Exception

        End Try

        Return node
    End Function


    Private Sub FetchOU_Click(sender As Object, e As EventArgs) Handles btnFatchOrg.Click
        '      Dim szCurrentHostname As String = GetComputerName()
        'AddComputerToOU(szCurrentHostname, textBox3.Text)
        '  cmbOU.DataSource = EnumerateOU(txtDomainName.Text)
        cmbOU.DataSource = EnumerateOURecursive(txtDomainName.Text)


    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtDomainName.Text = "na.holcim.net"
        txtUserName.Text = "jafimlan"
        txtPassword.Text = "Lafarge.14"
        ImpersonationDemo.Impersonate(txtUserName.Text, txtDomainName.Text, txtPassword.Text)
    End Sub

    Private Sub TreeView1_Click(sender As Object, e As EventArgs) Handles TreeView1.Click

    End Sub

    Private Sub TreeView1_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick
        GetChildNode(e.Node)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            ImpersonationDemo.Impersonate(txtUserName.Text, txtDomainName.Text, txtPassword.Text)
            Dim wmiObject As ManagementObject = New ManagementObject(New ManagementPath(("Win32_ComputerSystem.Name='" _
                                + (Environment.MachineName + "'"))))
            Dim inParams As ManagementBaseObject = wmiObject.GetMethodParameters("JoinDomainOrWorkgroup")
            inParams("Name") = txtDomainName.Text
            inParams("Password") = txtPassword.Text
            inParams("UserName") = txtUserName.Text
            Dim OU As String = TreeView1.SelectedNode.Tag.ToString().Replace("LDAP://na.holcim.net/", "") '' "OU=Computers,DC=example,DC=com"
            MessageBox.Show(OU)
            inParams("AccountOU") = OU
            inParams("FJoinOptions") = 3
            ' Magic number: 3 = join to domain and create computer account
            MessageBox.Show(String.Format("Joining machine to domain under name '{0}'...", inParams("Name")))
            Dim joinParams As ManagementBaseObject = wmiObject.InvokeMethod("JoinDomainOrWorkgroup", inParams, New InvokeMethodOptions())
            MessageBox.Show(String.Format("JoinDomainOrWorkgroup return code: '{0}'", joinParams("ReturnValue")))
            ' Did it work?
            If (CType(joinParams.Properties("ReturnValue").Value, UInteger) <> 0) Then
                ' Join to domain didn't work
                MessageBox.Show(String.Format("JoinDomainOrWorkgroup failed with return code: '{0}'", joinParams("ReturnValue")))
            Else

                System.Diagnostics.Process.Start("shutdown", "/r /t 0")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class



Public Class SymDebug

    Public Enum Severity

        emerg = 0

        alert

        crit

        err

        warning

        notice

        info

        debug

        none
    End Enum

    Public Shared Sub EmitString(ByVal sev As Severity, ByVal szFormat As String, ByVal ParamArray args() As Object)
        Debug.Print(szFormat, args)
    End Sub
End Class
