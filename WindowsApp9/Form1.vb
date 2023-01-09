Imports System.Drawing
Imports System.Threading

Public Class Form1
    ' The direction that the snake is moving
    Private Enum Direction
        Up
        Down
        Left
        Right
    End Enum

    ' The current direction of the snake
    Private snakeDirection As Direction = Direction.Right

    ' The size of each cell in the grid
    Private Const CellSize As Integer = 20

    ' The size of the grid
    Private Const GridSize As Integer = 20

    ' The snake as a list of points
    Private snake As New List(Of Point)()

    ' The food for the snake
    Private food As Point

    ' A random number generator
    Private rand As New Random()

    ' Whether the game is over
    Private gameOver As Boolean = False

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Start the game
        StartGame()
    End Sub

    Private Sub StartGame()
        ' Set the game over flag to false
        gameOver = False

        ' Clear the snake list
        snake.Clear()

        ' Add the initial snake position
        snake.Add(New Point(GridSize \ 2, GridSize \ 2))

        ' Generate the initial food position
        food = New Point(rand.Next(0, GridSize), rand.Next(0, GridSize))

        ' Set the initial direction
        snakeDirection = Direction.Right
    End Sub

    Private Sub UpdateGame()
        ' Move the snake in the current direction
        Dim head As Point = snake(0)
        Dim newHead As Point

        Select Case snakeDirection
            Case Direction.Up
                newHead = New Point(head.X, head.Y - 1)
            Case Direction.Down
                newHead = New Point(head.X, head.Y + 1)
            Case Direction.Left
                newHead = New Point(head.X - 1, head.Y)
            Case Direction.Right
                newHead = New Point(head.X + 1, head.Y)
        End Select

        ' Check for collision with walls
        If newHead.X < 0 OrElse newHead.X >= GridSize OrElse
           newHead.Y < 0 OrElse newHead.Y >= GridSize Then
            gameOver = True
            Return
        End If

        ' Check for collision with snake body
        If snake.Contains(newHead) Then
            gameOver = True
            Return
        End If

        ' Add the new head
        snake.Insert(0, newHead)

        ' Check for collision with food
        If newHead = food Then
            ' Generate a new food position
            food = New Point(rand.Next(0, GridSize), rand.Next(0, GridSize))
        Else
            ' Remove the tail
            snake.RemoveAt(snake.Count - 1)
        End If
    End Sub

    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        ' Set up the graphics object
        Dim g As Graphics = e.Graphics
        g.Clear(Color.White)
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        ' Draw the grid
        For i As Integer = 0 To GridSize - 1
            For j As Integer = 0 To GridSize - 1
                g.DrawRectangle(Pens.LightGray, i * CellSize, j * CellSize, CellSize, CellSize)
            Next
        Next

        ' Draw the food
        g.FillEllipse(Brushes.Red, food.X * CellSize, food.Y * CellSize, CellSize, CellSize)

        ' Draw the snake
        For Each p As Point In snake
            g.FillEllipse(Brushes.Green, p.X * CellSize, p.Y * CellSize, CellSize, CellSize)
        Next

        ' Draw the game over message, if necessary
        If gameOver Then
            Dim message As String = "Game Over"
            Dim font As New Font("Arial", 36)
            Dim size As SizeF = g.MeasureString(message, font)
            g.DrawString(message, font, Brushes.Red, (ClientSize.Width - size.Width) \ 2, (ClientSize.Height - size.Height) \ 2)
        End If
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        ' Update the snake direction based on the key press
        Select Case e.KeyCode
            Case Keys.Up
                If snakeDirection <> Direction.Down Then
                    snakeDirection = Direction.Up
                End If
            Case Keys.Down
                If snakeDirection <> Direction.Up Then
                    snakeDirection = Direction.Down
                End If
            Case Keys.Left
                If snakeDirection <> Direction.Right Then
                    snakeDirection = Direction.Left
                End If
            Case Keys.Right
                If snakeDirection <> Direction.Left Then
                    snakeDirection = Direction.Right
                End If
        End Select
    End Sub

    Private Sub GameLoop()
        While True
            ' Update the game
            UpdateGame()

            ' Redraw the screen
            Invalidate()

            ' Sleep for a short time
            Thread.Sleep(1100)
        End While
    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        ' Start the game loop on a separate thread
        Dim t As New Thread(AddressOf GameLoop)
        t.Start()
    End Sub
End Class
