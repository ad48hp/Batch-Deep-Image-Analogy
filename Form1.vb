Imports System.Globalization
Imports System.IO
Imports System.Text.RegularExpressions
Imports BatchDeepImageAnalogy

Public Class Form1
    Public curdata As String
    Public isrunning As Boolean
    Public didalready As Boolean
    Public dastopwač As New Stopwatch
    Public imageext As String() = New String() {"jpg", "bmp", "png"}
    Public mytempbit69 As Bitmap
    Public myg As Graphics

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        If File.Exists(Application.StartupPath + "/options") Then
            Dim myreader As New StreamReader(Application.StartupPath + "/options")
            TextBox1.Text = ReadLine(myreader)
            TextBox8.Text = ReadLine(myreader)
            TextBox2.Text = ReadLine(myreader)
            TextBox3.Text = ReadLine(myreader)
            TextBox5.Text = ReadLine(myreader)
            TextBox6.Text = ReadLine(myreader)
            CheckBox1.Checked = ReadBool(myreader)
            TextBox4.Text = ReadLine(myreader)
            TextBox9.Text = ReadLine(myreader)
            CheckBox2.Checked = ReadBool(myreader)
            TextBox10.Text = ReadLine(myreader)
            CheckBox3.Checked = ReadBool(myreader)
            TextBox11.Text = ReadLine(myreader)
            TextBox12.Text = ReadLine(myreader)
            TextBox13.Text = ReadLine(myreader)
            CheckBox4.Checked = ReadBool(myreader)
            TextBox14.Text = ReadLine(myreader)
            TextBox15.Text = ReadLine(myreader).Replace("infinite", "∞")
            TextBox16.Text = ReadLine(myreader)
            TextBox17.Text = ReadLine(myreader)
            TextBox18.Text = ReadLine(myreader)
            TextBox19.Text = ReadLine(myreader)
            TextBox20.Text = ReadLine(myreader)
            CheckBox5.Checked = ReadBool(myreader)
            CheckBox6.Checked = ReadBool(myreader)
            ListBox1.SelectedIndex = ReadInteger(myreader)
            myreader.Close()
        End If
        Dim mycontrols As New List(Of Control)
        mycontrols = GetControls(Me)
        For Each mycont In mycontrols
            If mycont Is GetType(TextBox) AndAlso DirectCast(mycont, TextBox).Multiline Then
                AddHandler mycont.KeyDown, AddressOf txteventz
            End If
        Next
        isrunning = True
    End Sub

    Private Function ReadInteger(myreader As StreamReader) As Integer
        If Not myreader.EndOfStream Then
            Return myreader.ReadLine()
        Else
            Return 0
        End If
    End Function

    Private Function ReadLine(myreader As StreamReader) As String
        If Not myreader.EndOfStream Then
            Return myreader.ReadLine()
        Else
            Return ""
        End If
    End Function

    Private Function ReadBool(myreader As StreamReader) As Boolean
        If Not myreader.EndOfStream Then
            Return myreader.ReadLine()
        Else
            Return False
        End If
    End Function

    Private Sub txteventz(sender As Object, e As KeyEventArgs)
        If e.Control AndAlso e.KeyCode = Keys.A Then
            DirectCast(sender, TextBox).SelectionStart = 0
            DirectCast(sender, TextBox).SelectionLength = DirectCast(sender, TextBox).Text.Length
        End If
    End Sub

    Private Function GetControls(myctl As Control) As List(Of Control)
        Dim myctls As New List(Of Control)
        For Each mycon In myctl.Controls
            myctls.AddRange(GetControls(mycon))
            myctls.Add(mycon)
        Next
        Return myctls
    End Function

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim mywriter As New StreamWriter(Application.StartupPath + "/options")
        mywriter.WriteLine(TextBox1.Text)
        mywriter.WriteLine(TextBox8.Text)
        mywriter.WriteLine(TextBox2.Text)
        mywriter.WriteLine(TextBox3.Text)
        mywriter.WriteLine(TextBox5.Text)
        mywriter.WriteLine(TextBox6.Text)
        mywriter.WriteLine(CheckBox1.Checked)
        mywriter.WriteLine(TextBox4.Text)
        mywriter.WriteLine(TextBox9.Text)
        mywriter.WriteLine(CheckBox2.Checked)
        mywriter.WriteLine(TextBox10.Text)
        mywriter.WriteLine(CheckBox3.Checked)
        mywriter.WriteLine(TextBox11.Text)
        mywriter.WriteLine(TextBox12.Text)
        mywriter.WriteLine(TextBox13.Text)
        mywriter.WriteLine(CheckBox4.Checked)
        mywriter.WriteLine(TextBox14.Text)
        mywriter.WriteLine(TextBox15.Text.Replace("∞", "infinite"))
        mywriter.WriteLine(TextBox16.Text)
        mywriter.WriteLine(TextBox17.Text)
        mywriter.WriteLine(TextBox18.Text)
        mywriter.WriteLine(TextBox19.Text)
        mywriter.WriteLine(TextBox20.Text)
        mywriter.WriteLine(CheckBox5.Checked)
        mywriter.WriteLine(CheckBox6.Checked)
        mywriter.WriteLine(ListBox1.SelectedIndex)
        mywriter.Close()
        isrunning = False
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        GroupBox1.Enabled = Not CheckBox1.Checked
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False
        Button1.ForeColor = Color.Blue
        Button1.BackColor = Color.Green
        Button1.Text = "Processing..."
        Dim cancontinue As Boolean
        cancontinue = True
        Dim directorystyle As Boolean
        TextBox1.Text = TextBox1.Text.Replace("/", "\")
        TextBox2.Text = TextBox2.Text.Replace("/", "\")
        TextBox8.Text = TextBox8.Text.Replace("/", "\")
        While TextBox1.Text.EndsWith("\")
            TextBox1.Text = TextBox1.Text.Substring(0, TextBox1.Text.Length - 1)
        End While
        While TextBox2.Text.EndsWith("\")
            TextBox2.Text = TextBox2.Text.Substring(0, TextBox2.Text.Length - 1)
        End While
        While TextBox8.Text.EndsWith("\")
            TextBox8.Text = TextBox8.Text.Substring(0, TextBox8.Text.Length - 1)
        End While
        While TextBox17.Text.EndsWith("\")
            TextBox17.Text = TextBox17.Text.Substring(0, TextBox17.Text.Length - 1)
        End While
        If Not CheckBox5.Checked Then
            If Not Directory.Exists(TextBox1.Text) Then
                MessageBox.Show("Source directory doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            End If
        Else
            If Not File.Exists(TextBox1.Text) Then
                MessageBox.Show("Source file doesn't exist [folders can't apply for zoom-in].", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            End If
        End If
        If Not Directory.Exists(TextBox8.Text) AndAlso Not File.Exists(TextBox8.Text) Then
                MessageBox.Show("Neither source style directory or file does exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            Else
                If Directory.Exists(TextBox8.Text) AndAlso File.Exists(TextBox8.Text) Then
                MessageBox.Show("Both source style directory or file does exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            Else
                If TextBox8.Text.Contains(" ") Then
                    MessageBox.Show("The style path contains a space.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    cancontinue = False
                Else
                    If File.Exists(TextBox8.Text) Then
                        directorystyle = False
                    Else
                        directorystyle = True
                    End If
                End If
            End If
        End If
        If Not Directory.Exists(TextBox2.Text) Then
            MessageBox.Show("Destination directory doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cancontinue = False
        End If
        If Not File.Exists(TextBox3.Text) Then
            MessageBox.Show("DIA executable doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cancontinue = False
        End If
        If TextBox1.Text.Contains(" ") Then
            MessageBox.Show("Source contains a space.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cancontinue = False
        End If
        If TextBox2.Text.Contains(" ") Then
            MessageBox.Show("Destination contains a space.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cancontinue = False
        End If
        TextBox4.Text = TextBox4.Text.Replace(",", ".")
        TextBox5.Text = TextBox5.Text.Replace(",", ".")
        TextBox6.Text = TextBox6.Text.Replace(",", ".")
        TextBox9.Text = TextBox9.Text.Replace(",", ".")
        TextBox16.Text = TextBox16.Text.Replace(",", ".")
        If Not DoubleTryParse(TextBox4.Text, 1) Then
            MessageBox.Show("Lower ratio by isn't a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cancontinue = False
        End If
        If Not DoubleTryParse(TextBox5.Text, 1) Then
            MessageBox.Show("Ratio isn't a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cancontinue = False
        End If
        If Not DoubleTryParse(TextBox6.Text, 1) Then
            MessageBox.Show("Blend weights isn't a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cancontinue = False
        End If
        If Not DoubleTryParse(TextBox9.Text, 1) Then
            MessageBox.Show("Minimum ratio isn't a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cancontinue = False
        End If
        If Not Integer.TryParse(TextBox10.Text, 1) Then
            MessageBox.Show("Maximum retries isn't an integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cancontinue = False
        End If
        If CheckBox3.Checked Then
            If Not Integer.TryParse(TextBox11.Text, 1) Then
                MessageBox.Show("End after hour isn't an integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            End If
            If Not Integer.TryParse(TextBox12.Text, 1) Then
                MessageBox.Show("End after minute isn't an integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            End If
            If Not Integer.TryParse(TextBox13.Text, 1) Then
                MessageBox.Show("End after second isn't an integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            End If
        End If
        If CheckBox4.Checked Then
            If Not TextBox15.Text = "∞" AndAlso Not Integer.TryParse(TextBox15.Text, 1) Then
                MessageBox.Show("Run after done times isn't an integer or infinity sign.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            End If
            If Not DoubleTryParse(TextBox16.Text, 1) Then
                MessageBox.Show("Run after done each seconds isn't a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            End If
        End If
        If CheckBox5.Checked Then
            TextBox18.Text = TextBox18.Text.Replace(",", ".")
            If Not DoubleTryParse(TextBox18.Text, 1) Then
                MessageBox.Show("Zoom-in ratio isn't a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            End If
        End If
        If cancontinue Then
            Dim myinputs As List(Of FileInfo)
            If Not CheckBox5.Checked Then
                myinputs = New DirectoryInfo(TextBox1.Text).GetFiles().ToList()
            Else
                If (Not CheckBox6.Checked AndAlso Not File.Exists(TextBox2.Text + "/1.png")) OrElse (CheckBox6.Checked AndAlso Not File.Exists(TextBox2.Text + "/1_2.png")) Then
                    myinputs = New FileInfo() {New FileInfo(TextBox1.Text)}.ToList()
                Else
                    Dim lasti As Integer
                    lasti = 1
                    If Not CheckBox6.Checked Then
                        While File.Exists(TextBox2.Text + "/" + lasti.ToString() + ".png")
                            lasti += 1
                        End While
                        myinputs = New FileInfo() {New FileInfo(TextBox2.Text + "/" + (lasti - 1).ToString() + ".png")}.ToList()
                    Else
                        While File.Exists(TextBox2.Text + "/" + lasti.ToString() + "_2.png")
                            lasti += 1
                        End While
                        myinputs = New FileInfo() {New FileInfo(TextBox2.Text + "/" + (lasti - 1).ToString() + "_2.png")}.ToList()
                    End If
                End If
                End If
                Dim mystyles As FileInfo()
                If directorystyle Then
                    mystyles = New DirectoryInfo(TextBox8.Text).GetFiles()
                End If
                Dim diaroot As String
                diaroot = Path.GetDirectoryName(TextBox3.Text).Replace("/", "\").Substring(0, Path.GetDirectoryName(TextBox3.Text).Replace("/", "\").LastIndexOf("\"))
                If Not Directory.Exists(diaroot + "\output") Then
                    Directory.CreateDirectory(diaroot + "\output")
                End If
            Dim modelpath As String
            Dim haveshownspacebox As Boolean
                haveshownspacebox = False
                TextBox7.Text = ""
            Label9.Text = "0/" + myinputs.Count.ToString()
            Dim lastskipped As FileInfo
                lastskipped = Nothing
            Dim skippedinputs As List(Of FileInfo)
            If CheckBox5.Checked Then
                If File.Exists(diaroot + "\output\resultAB.png") Then
                    File.Delete(diaroot + "\output\resultAB.png")
                End If
                If File.Exists(diaroot + "\output\resultBA.png") Then
                    File.Delete(diaroot + "\output\resultBA.png")
                End If
            End If
            While myinputs.Count > 0
                skippedinputs = New List(Of FileInfo)
                    Dim i As Integer
                    i = 1
                    While i <= myinputs.Count
                        Dim sucessfull As Boolean
                        sucessfull = False
                        Dim sucessfull2 As Boolean
                        sucessfull2 = False
                        curdata = ""
                        Dim mytries As Integer
                        mytries = 0
                        While Not sucessfull
                        Dim timestr As String
                        timestr = TextBox11.Text.PadLeft(2, "0") + ":" + TextBox12.Text.PadLeft(2, "0") + ":" + TextBox13.Text.PadLeft(2, "0")
                        If TextBox17.Text = "" Then
                            modelpath = diaroot
                        Else
                            modelpath = TextBox17.Text
                        End If
                        If CheckBox3.Checked AndAlso ((TextBox11.Text >= DateTime.Now.Hour AndAlso DateTime.Now > DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " + timestr, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)) OrElse (TextBox11.Text < DateTime.Now.Hour AndAlso DateTime.Now > DateTime.ParseExact(DateTime.Now.AddDays(1).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) + " " + timestr, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture))) Then
                            If CheckBox4.Checked Then
                                Dim mystopwač As New Stopwatch
                                mystopwač.Reset()
                                Dim myprogram As String
                                Dim myargz As String
                                If Not TextBox14.Text.Contains(" ") Then
                                    myprogram = TextBox14.Text
                                    myargz = ""
                                Else
                                    myprogram = TextBox14.Text.Substring(0, TextBox14.Text.IndexOf(" "))
                                    myargz = TextBox14.Text.Substring(TextBox14.Text.IndexOf(" ") + 1, TextBox14.Text.Length - (TextBox14.Text.IndexOf(" ") + 1))
                                End If
                                If Not TextBox15.Text = "∞" Then
                                    Dim shouldexittim As Boolean
                                    shouldexittim = False
                                    Dim kolecko As Integer
                                    kolecko = 1
                                    While Not shouldexittim
                                        Process.Start(myprogram, myargz)
                                        If Not TextBox15.Text = "1" Then
                                            mystopwač.Start()
                                            While Not mystopwač.ElapsedMilliseconds > Double.Parse(TextBox16.Text) * 1000
                                                Application.DoEvents()
                                            End While
                                            mystopwač.Reset()
                                        End If
                                        If kolecko >= Integer.Parse(TextBox15.Text) Then
                                            shouldexittim = True
                                        End If
                                        kolecko += 1
                                    End While
                                Else
                                    Do
                                        Process.Start(myprogram, myargz)
                                        While Not mystopwač.ElapsedMilliseconds > Double.Parse(TextBox16.Text) * 1000
                                            Application.DoEvents()
                                        End While
                                        mystopwač.Reset()
                                    Loop
                                End If
                            End If
                            Environment.Exit(Environment.ExitCode)
                        End If
                        mytries += 1
                            Dim myitem As FileInfo
                            myitem = myinputs(i - 1)
                            If myitem.Name.Contains(" ") Then
                                If Not haveshownspacebox Then
                                    MessageBox.Show("The input file " + ChrW(34) + myitem.Name + ChrW(34) + " contain a space. These will be automatically removed.")
                                    haveshownspacebox = True
                                End If
                                myitem = RemoveFileZnaks(myitem, New String() {" "})
                            End If
                        If (Not CheckBox5.Checked AndAlso Not File.Exists(TextBox2.Text + "/" + myitem.Name)) OrElse CheckBox5.Checked Then
                            If PictureBox1.Image Is Nothing AndAlso Not lastskipped Is Nothing Then
                                Dim mypicname As String
                                mypicname = TextBox2.Text + "/" + Path.GetFileNameWithoutExtension(lastskipped.FullName)
                                Dim mypicext As String
                                mypicext = lastskipped.Extension.Replace(".", "")
                                If File.Exists(mypicname + "." + mypicext) Then
                                    Using myimg As New FileStream(mypicname + "." + mypicext, FileMode.Open)
                                        PictureBox1.Image = New Bitmap(myimg)
                                    End Using
                                End If
                                If File.Exists(mypicname + "_2." + mypicext) Then
                                    Using myimg2 As New FileStream(mypicname + "_2." + mypicext, FileMode.Open)
                                        PictureBox2.Image = New Bitmap(myimg2)
                                    End Using
                                End If
                            End If
                            Dim mystartinfo As New ProcessStartInfo
                            mystartinfo.FileName = TextBox3.Text
                            mystartinfo.WorkingDirectory = Path.GetDirectoryName(TextBox3.Text)
                            mystartinfo.Arguments = modelpath + "\" + " " + myitem.FullName + " "
                            If Not directorystyle Then
                                mystartinfo.Arguments += TextBox8.Text
                            Else
                                If File.Exists(TextBox8.Text + "\" + Path.GetFileName(myitem.FullName)) Then
                                    mystartinfo.Arguments += TextBox8.Text + "\" + Path.GetFileName(myitem.FullName)
                                Else
                                    Dim mystylez As FileInfo
                                    mystylez = mystyles(i - 1)
                                    If mystylez.Name.Contains(" ") Then
                                        If Not haveshownspacebox Then
                                            MessageBox.Show("The style file " + ChrW(34) + mystylez.Name + ChrW(34) + " contain a space. These will be automatically removed.")
                                            haveshownspacebox = True
                                        End If
                                        mystylez = RemoveFileZnaks(mystylez, New String() {" "})
                                    End If
                                    mystartinfo.Arguments += mystylez.FullName
                                End If
                            End If
                            mystartinfo.Arguments += " " + diaroot + "\output\ 0 " + TextBox5.Text + " " + TextBox6.Text + " 0"
                            Dim myprocess As New Process
                            If Not CheckBox1.Checked Then
                                mystartinfo.UseShellExecute = False
                                mystartinfo.RedirectStandardOutput = True
                                mystartinfo.RedirectStandardError = True
                                mystartinfo.CreateNoWindow = True
                                AddHandler myprocess.OutputDataReceived, AddressOf outputrec
                                AddHandler myprocess.ErrorDataReceived, AddressOf outputrec
                            End If
                            myprocess.StartInfo = mystartinfo
                            myprocess.Start()
                            If Not CheckBox1.Checked Then
                                    myprocess.BeginOutputReadLine()
                                    myprocess.BeginErrorReadLine()
                                End If
                                While Not myprocess.HasExited
                                    If Not isrunning Then
                                        myprocess.Kill()
                                        Environment.Exit(Environment.ExitCode)
                                    End If
                                    Application.DoEvents()
                                End While
                                TextBox7.Text += Environment.NewLine
                                If (Not CheckBox1.Checked AndAlso curdata.Contains("Finished")) OrElse CheckBox1.Checked Then
                                Dim myitemname As String
                                If File.Exists(diaroot + "\output\resultAB.png") Then
                                    If Not CheckBox5.Checked Then
                                        myitemname = Path.GetFileNameWithoutExtension(myitem.Name)
                                    Else
                                        i += 1
                                        myitemname = i.ToString()
                                    End If
                                    Dim myoutput As String
                                    myoutput = TextBox2.Text + "/" + myitemname
                                    If File.Exists(myoutput + ".png") Then
                                        Dim myoutputcunt As Integer
                                        myoutputcunt = 2
                                        myoutput = TextBox2.Text + "/" + myitemname + "_" + myoutputcunt.ToString()
                                        While File.Exists(myoutput + ".png")
                                            myoutputcunt += 1
                                            myoutput = TextBox2.Text + "/" + myitemname + "_" + myoutputcunt.ToString()
                                        End While
                                    End If
                                    If CheckBox2.Checked AndAlso Not CheckBox5.Checked Then
                                        Using mypreview As New FileStream(diaroot + "\output\resultAB.png", FileMode.Open)
                                            PictureBox1.Image = New Bitmap(mypreview)
                                        End Using
                                    End If
                                    File.Move(diaroot + "\output\resultAB.png", myoutput + ".png")
                                    If CheckBox5.Checked AndAlso Not CheckBox6.Checked Then
                                        myinputs.Add(New FileInfo(myoutput + ".png"))
                                        Dim mybitmap6969 As Bitmap
                                        Using myztreamtm As New FileStream(myoutput + ".png", FileMode.Open)
                                            mybitmap6969 = New Bitmap(myztreamtm)
                                        End Using
                                        mybitmap6969 = CropRect(mybitmap6969, GetRekt69(mybitmap6969.Size, mybitmap6969))
                                        mybitmap6969.Save(myoutput + ".png")
                                    End If
                                    If File.Exists(myoutput + ".png") Then
                                        Dim myoutputcunt As Integer
                                        myoutputcunt = 2
                                        myoutput = TextBox2.Text + "/" + myitemname + "_" + myoutputcunt.ToString()
                                        While File.Exists(myoutput + ".png")
                                            myoutputcunt += 1
                                            myoutput = TextBox2.Text + "/" + myitemname + "_" + myoutputcunt.ToString()
                                        End While
                                    End If
                                    If CheckBox2.Checked AndAlso Not CheckBox5.Checked Then
                                        Using mypreview As New FileStream(diaroot + "\output\resultBA.png", FileMode.Open)
                                            PictureBox2.Image = New Bitmap(mypreview)
                                        End Using
                                    End If
                                    File.Move(diaroot + "\output\resultBA.png", myoutput + ".png")
                                    If CheckBox5.Checked AndAlso CheckBox6.Checked Then
                                        myinputs.Add(New FileInfo(myoutput + ".png"))
                                        Dim mybitmap6767 As Bitmap
                                        Using myztreamtm As New FileStream(myoutput + ".png", FileMode.Open)
                                            mybitmap6767 = New Bitmap(myztreamtm)
                                        End Using
                                        mybitmap6767 = CropRect(mybitmap6767, GetRekt69(mybitmap6767.Size, mybitmap6767))
                                        mybitmap6767.Save(myoutput + ".png")
                                    End If
                                    sucessfull = True
                                    sucessfull2 = True
                                Else
                                    If DoubleParse(TextBox5.Text) - DoubleParse(TextBox4.Text) > DoubleParse(TextBox9.Text) Then
                                        TextBox5.Text = DoubleParse(TextBox5.Text) - DoubleParse(TextBox4.Text)
                                    Else
                                        sucessfull = True
                                    End If
                                End If
                            End If
                        Else
                            lastskipped = myitem
                                sucessfull = True
                            End If
                            If Not sucessfull Then
                                If mytries >= Integer.Parse(TextBox10.Text) Then
                                    skippedinputs.Add(myitem)
                                    Label10.Text = "Skipped " + skippedinputs.Count.ToString()
                                    sucessfull = True
                                End If
                            End If
                        End While
                        Label9.Text = i.ToString() + "/" + myinputs.Count.ToString()
                    If Not CheckBox5.Checked Then
                        i += 1
                    End If
                End While
                    myinputs = New List(Of FileInfo)(skippedinputs)
                End While
            End If
        Button1.ForeColor = Color.Red
        Button1.BackColor = Color.Orange
        Button1.Text = "Process"
        Button1.Enabled = True
    End Sub

    Private Function DoubleTryParse(text As String, v As Integer) As Boolean
        Return Double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, v)
    End Function

    Private Function DoubleParse(text As String) As Integer
        Return Double.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture)
    End Function

    Private Sub outputrec(sender As Object, e As DataReceivedEventArgs)
        curdata += e.Data + Environment.NewLine
        SetText("{" + Date.Now.ToString("HH:mm:ss") + "} " + e.Data + Environment.NewLine, TextBox7, True, True)
    End Sub

    Private Sub SetText(ByVal text As String, ByVal tbox As TextBox, ByVal addto As Boolean, ByVal scrolltoend As Boolean)
        If tbox.InvokeRequired Then
            Dim d As SetTextDelegate = New SetTextDelegate(AddressOf SetText)
            Me.Invoke(d, New Object() {text, tbox, addto, scrolltoend})
        Else
            If Not addto Then
                tbox.Text = text
            Else
                tbox.Text += text
            End If
            If scrolltoend Then
                tbox.SelectionStart = tbox.Text.Length
                tbox.SelectionLength = 0
                tbox.ScrollToCaret()
            End If
        End If
    End Sub

    Delegate Sub SetTextDelegate(ByVal text As String, ByVal tbox As TextBox, ByVal addto As Boolean, ByVal scrolltoend As Boolean)

    Private Function RemoveFileZnaks(myitem As FileInfo, znaks As String()) As FileInfo
        Dim myname16 As String
        Dim myname17 As String
        myname17 = Path.GetFileNameWithoutExtension(myitem.FullName)
        For Each myznak In znaks
            myname17 = myname17.Replace(myznak, "")
        Next
        myname16 = myname17
        Dim myext16 As String
        myext16 = myitem.Extension.ToLower().Replace(".", "")
        If File.Exists(Path.GetDirectoryName(myitem.FullName) + "/" + myname16 + "." + myext16) Then
            Dim mycount As Integer
            mycount = 2
            myname16 = myname17 + mycount.ToString()
            While File.Exists(Path.GetDirectoryName(myitem.FullName) + "/" + myname16 + "." + myext16)
                mycount += 1
                myname16 = myname17 + mycount.ToString()
            End While
        End If
        File.Move(myitem.FullName, Path.GetDirectoryName(myitem.FullName) + "/" + myname16 + "." + myext16)
        Return New FileInfo(Path.GetDirectoryName(myitem.FullName) + "/" + myname16 + "." + myext16)
    End Function

    Private Sub TextBox1_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox8.DragEnter, TextBox3.DragEnter, TextBox2.DragEnter, TextBox1.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub TextBox1_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox8.DragDrop, TextBox3.DragDrop, TextBox2.DragDrop, TextBox1.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        DirectCast(sender, TextBox).Text = files(0)
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        GroupBox2.Enabled = CheckBox2.Checked
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        TextBox11.Enabled = CheckBox3.Checked
        TextBox12.Enabled = CheckBox3.Checked
        TextBox13.Enabled = CheckBox3.Checked
        CheckBox4.Enabled = CheckBox3.Checked
    End Sub

    Private Sub TextBox15_TextChanged(sender As Object, e As EventArgs)
        If TextBox15.Enabled Then
            TextBox16.Enabled = Not TextBox15.Text = "1"
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs)
        TextBox14.Enabled = CheckBox4.Checked
        TextBox15.Enabled = CheckBox4.Checked
        TextBox16.Enabled = CheckBox4.Checked
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox2.Text = TextBox2.Text.Replace("/", "\")
        While TextBox2.Text.EndsWith("\")
            TextBox2.Text = TextBox2.Text.Substring(0, TextBox2.Text.Length - 1)
        End While
        If Not Directory.Exists(TextBox2.Text) Then
            MessageBox.Show("Destination directory doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            Dim myinfos As FileInfo()
            myinfos = New DirectoryInfo(TextBox2.Text).GetFiles()
            If Not Directory.Exists(TextBox2.Text + "/1") Then
                Directory.CreateDirectory(TextBox2.Text + "/1")
            End If
            If Not Directory.Exists(TextBox2.Text + "/2") Then
                Directory.CreateDirectory(TextBox2.Text + "/2")
            End If
            For Each myitem113 In myinfos
                If Not myitem113.Name.Contains("_2") Then
                    File.Move(myitem113.FullName, TextBox2.Text + "/1/" + myitem113.Name)
                Else
                    File.Move(myitem113.FullName, TextBox2.Text + "/2/" + myitem113.Name)
                End If
            Next
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Text = "Show" Then
            Dim cancontinue As Boolean
            cancontinue = True
            TextBox18.Text = TextBox18.Text.Replace(",", ".")
            TextBox19.Text = TextBox19.Text.Replace(",", ".")
            TextBox20.Text = TextBox20.Text.Replace(",", ".")
            If Not DoubleTryParse(TextBox18.Text, 1) Then
                MessageBox.Show("Zoom-in ratio isn't a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            End If
            If Not DoubleTryParse(TextBox19.Text, 1) Then
                MessageBox.Show("Replay after secondz isn't a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            End If
            If Not DoubleTryParse(TextBox20.Text, 1) Then
                MessageBox.Show("FPS isn't a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            End If
            Dim toloadpath As String
            Dim myfilez81 As List(Of FileInfo)
            If Not Directory.Exists(TextBox2.Text) Then
                MessageBox.Show("Destination directory doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                cancontinue = False
            Else
                Dim myfilez69 As New DirectoryInfo(TextBox2.Text)
                myfilez81 = myfilez69.GetFiles().OrderBy(Function(x) PadNumbers(x.Name)).Where(Function(x) imageext.Contains(x.Extension.Replace(".", "").ToLower())).ToList()
                If Not myfilez81.Count = 0 Then
                    toloadpath = myfilez81(myfilez81.Count - 1).FullName
                Else
                    If File.Exists(TextBox1.Text) Then
                        toloadpath = TextBox1.Text
                    Else
                        cancontinue = False
                    End If
                End If
                End If
            If cancontinue Then
                Button3.Text = "Stop"
                If DoubleParse(TextBox20.Text) > 0 Then
                    Timer1.Interval = 1000 / DoubleParse(TextBox20.Text)
                End If
                didalready = False
                Using mytempbit As New FileStream(toloadpath, FileMode.Open)
                    mytempbit69 = New Bitmap(mytempbit)
                End Using
                PictureBox3.Image = New Bitmap(mytempbit69)
                    dastopwač.Reset()
                    dastopwač.Start()
                    Timer1.Enabled = True
                End If
            Else
            Button3.Text = "Show"
            Timer1.Enabled = False
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Not didalready Then
            didalready = True
            If DoubleTryParse(TextBox19.Text, 1) AndAlso dastopwač.ElapsedMilliseconds > DoubleParse(TextBox19.Text) * 1000 Then
                PictureBox3.Image = New Bitmap(mytempbit69)
                dastopwač.Reset()
                dastopwač.Start()
            Else
                If DoubleTryParse(TextBox18.Text, 1) Then
                    PictureBox3.Image = DoMoving(PictureBox3.Image)
                End If
            End If
                didalready = False
        End If
    End Sub

    Private Function DoMoving(mybmp As Bitmap) As Bitmap
        Return CropRect(mybmp, GetRekt69(mybmp.Size, mybmp))
    End Function

    Private Function GetRekt69(sz As Size, mybit911 As Bitmap) As Rectangle
        Dim myxtm As Integer
        Dim myytm As Integer
        myxtm = Math.Floor(mybit911.Width / DoubleParse(TextBox18.Text))
        myytm = Math.Floor(mybit911.Height / DoubleParse(TextBox18.Text))
        Dim xxtm As Integer
        Dim yytm As Integer
        Select Case ListBox1.SelectedIndex
            Case 0
                xxtm = myxtm / 4
                yytm = myytm / 4
            Case 1
                xxtm = 0
                yytm = 0
            Case 2
                xxtm = myxtm / 4
                yytm = 0
            Case 3
                xxtm = myxtm / 2
                yytm = 0
            Case 4
                xxtm = 0
                yytm = myytm / 4
            Case 5
                xxtm = myxtm / 2
                yytm = myytm / 4
            Case 6
                xxtm = 0
                yytm = myytm / 2
            Case 7
                xxtm = myxtm / 4
                yytm = myytm / 2
            Case 8
                xxtm = myxtm / 2
                yytm = myytm / 2
        End Select
        Return New Rectangle(xxtm, yytm, mybit911.Width - (myxtm / 2), mybit911.Height - (myytm / 2))
    End Function

    Public Function CropRect(ByVal bmp As Bitmap, ByVal rct As Rectangle) As Bitmap
        Dim mbm As Bitmap = New Bitmap(bmp.Width, bmp.Height)
        myg = Graphics.FromImage(mbm)
        myg.DrawImage(bmp, New Rectangle(0, 0, bmp.Width, bmp.Height), rct, GraphicsUnit.Pixel)
        Dim bmp69 As Bitmap
        If False AndAlso rct.X < 0 Then
            bmp69 = New Bitmap(bmp)
            Dim myx As Integer
            myx = rct.X
            While myx < 0
                bmp69.RotateFlip(RotateFlipType.RotateNoneFlipX)
                myg.DrawImage(bmp69, New Rectangle(0, 0, bmp.Width, bmp.Height), New Rectangle(myx - bmp.Width, rct.Y, rct.Width, rct.Height), GraphicsUnit.Pixel)
                myx += bmp.Width
            End While
        End If
        Return mbm
    End Function

    Public Shared Function PadNumbers(ByVal input As String) As String
        Return Regex.Replace(input, "[0-9]+", Function(match) match.Value.PadLeft(10, "0"))
    End Function

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        CheckBox6.Enabled = CheckBox5.Checked
    End Sub
End Class
