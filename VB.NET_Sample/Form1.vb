Public Class AS400_Sample
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress

        If (e.KeyChar < "0"c OrElse "9"c < e.KeyChar) AndAlso e.KeyChar <> ControlChars.Back AndAlso Control.ModifierKeys <> Keys.Control Then
            '0～9と、バックスペースとコントロール以外の時は、イベントをキャンセルする
            e.Handled = True
        End If
    End Sub

    Private Sub AS400_Sample_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ToolTip1.SetToolTip(TextBox1, "フィールド名を読み込む場所です")
    End Sub

    Private Sub MenuStrip1_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub



    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click

    End Sub

    Private Sub 情報ToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles 情報ToolStripMenuItem.Click
        '情報部分
        Form2.Show()
    End Sub

    Private Sub 設定ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 設定ToolStripMenuItem.Click
        '設定部分
        Form3.Show()
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        'テキストボックスが全部入った状態でENTER

        If e.KeyCode = Keys.Enter Then
            Dim db As ADODB.Connection
            Dim rs As ADODB.Recordset

            Dim file As String '設定ファイル用
            Dim strsql
            Dim TEST_Text As String
            Dim BANGOU1 As String
            Dim BANGOU2 As String
            file = My.Settings.file
            TEST_Text = TextBox1.Text


            db = New ADODB.Connection
            Try
                'AS400の接続
                db.ConnectionString = "Provider=MSDASQL;" &
                       "Driver={iSeries Access ODBC Driver};" &
                       "Data Source=XXXXXXX;" &'ソースを入れる
                       "server=XXX.XXX.X.X;" &'サーバーのIPアドレス入れる
                       "UID=XXXXXX;" &'IDを入れる
                       "PWD=XXXXXX;" 'パスワードを入れる
                db.Open()

                'SQL

                strsql = "SELECT * " &
                         "FROM テーブル名 LEFT JOIN テーブル名 ON テーブル名.フィールド名 = テーブル名.フィールド名 " &
                         "WHERE フィールド名 = '" & TEST_Text & "'"

                rs = New ADODB.Recordset

                rs.Open(strsql, db)


                If Not rs.EOF Then
                    BANGOU1 = rs.Fields("フィールド名").Value 'フィールド名で抽出
                    BANGOU2 = rs.Fields(1).Value 'もしくはフィールド番号で抽出



                    '例　PDFファイルを開く
                    Dim strFilePath1 As String = file & BANGOU1 & ".pdf"
                    Dim boolFile_Exists1 As Boolean
                    boolFile_Exists1 = System.IO.File.Exists(strFilePath1)



                    If boolFile_Exists1 = True Then
                        Label1.Text = BANGOU1
                        CreateObject("Shell.Application").ShellExecute(strFilePath1)
                    Else
                        MessageBox.Show(“ファイルがありません”)

                    End If

                End If
                'AS400遮断
                rs.Close()
                rs = Nothing
                db.Close()
                db = Nothing

                TextBox1.Text = ""

            Catch ex As Exception
                MessageBox.Show(“AS400接続エラー”)
            End Try



        End If 'KEY ENTER用
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged

    End Sub
End Class
