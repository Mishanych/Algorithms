import pygame
from copy import deepcopy
import time

# constants
WIDTH, HEIGHT = 800, 800
ROWS, COLS = 8, 8
SQUARE_SIZE = HEIGHT//ROWS

# colours
RED = (255, 0, 0)
WHITE = (255, 255, 255)
BLACK = (0, 0, 0)
BLUE = (0, 0, 255)
BG = (75, 100, 130)
GOLD = (255, 215, 0)

# main window
window = pygame.display.set_mode((WIDTH, HEIGHT))
pygame.display.set_caption('Hare and Wolfs')


class Board:
    def __init__(self):
        self.board = []
        self.board_debug = []
        self.create_board()
        self.harePos = (7, 0)
        self.prevHarePos = (0, 0)

    # draw table
    def draw_board(self, window):
        window.fill(BLACK)
        for row in range(ROWS):
            for col in range(row % 2, COLS, 2):
                pygame.draw.rect(window, WHITE, (row * SQUARE_SIZE, col * SQUARE_SIZE, SQUARE_SIZE, SQUARE_SIZE))

    def evaluate(self):
        if len(self.get_valid_moves(self.get_piece(self.harePos[0], self.harePos[1]))) == 0:
            return 10000000
        return -(7 - self.harePos[0]) * 10 - len(self.get_valid_moves(self.get_piece(self.harePos[0], self.harePos[1]))) * 4 - self.distance_to_hare() * 8 - self.distance_between_wolfs() * 6 - self.will_win()

    # hare evaluation depends on height, distance from wolfs, number of moves
    def alternate_evaluate(self):
        return 8 ** abs(self.harePos[0] - 7) + self.distance_to_hare() * 2 + 4 ** abs(4 - self.harePos[1]) - self.evaluate()

    def get_all_pieces(self, color):
        pieces = []
        for row in self.board:
            for piece in row:
                if piece != 0 and piece.color == color:
                    pieces.append(piece)
        # print(pieces)
        return pieces

    def distance_to_hare(self):
        distance = 0
        for piece in self.get_all_pieces(BLACK):
            distance += abs(self.harePos[0] - piece.row) + abs(self.harePos[1] - piece.col)
        return distance

    def distance_between_wolfs(self):
        minX = 8
        maxX = 0
        minY = 8
        maxY = 0
        for piece in self.get_all_pieces(BLACK):
            if piece.row > maxY:
                maxY = piece.row
            if piece.col > maxX:
                maxX = piece.col
            if piece.row < minY:
                minY = piece.row
            if piece.col < minX:
                minX = piece.row
        return (maxX - minX) * 2 + (maxY - minY) * 3 + maxX * 2

    # check if hare can win
    def will_win(self):
        for move in self.get_valid_moves(self.get_piece(self.harePos[0], self.harePos[1])):
            if move[0] == 0:
                return 1000000
        return 0

    def move(self, piece, row, col):
        self.board[piece.row][piece.col], self.board[row][col] = self.board[row][col], self.board[piece.row][piece.col]
        self.board_debug[piece.row][piece.col], self.board_debug[row][col] = self.board_debug[row][col], self.board_debug[piece.row][piece.col]
        if piece.color == RED:
            self.harePos = (row, col)
            self.prevHarePos = (piece.row, piece.col)
        piece.move(row, col)

    # check end state
    def winner(self):
        piece = self.get_piece(self.harePos[0], self.harePos[1])
        if self.harePos[0] == 0:
            return 'Hare'
        elif not self.get_valid_moves(piece):
            return 'Wolfs'
        else:
            counter = 0
            for piece in self.get_all_pieces(BLACK):
                if len(self.get_valid_moves(piece)) == 0:
                    counter += 1
            if counter == 4:
                return 'Hare'
        return None

    def get_piece(self, row, col):
        return self.board[row][col]

    # 0 - none of pieces
    def create_board(self):
        for row in range(ROWS):
            self.board.append([])
            for col in range(COLS):
                if col % 2 == ((row + 1) % 2):
                    if row == 0:
                        self.board[row].append(Piece(row, col, BLACK))
                    elif row == 7 and col == 0:
                        self.board[row].append(Piece(row, col, RED))
                    else:
                        self.board[row].append(0)
                else:
                    self.board[row].append(0)
            # print(self.board[row][0].__repr__)

    def _init_board_debug(self):
        for row in range(ROWS):
            self.board_debug.append([])
            for col in range(COLS):
                if isinstance(self.board[row][col], Piece):
                    if str(self.board[row][col]) == str(RED):
                        self.board_debug[row].append("Hare")
                    else:
                        self.board_debug[row].append("Wolf")
                else:
                    if row % 2 == 0:
                        if col % 2 == 1:
                            self.board_debug[row].append("BLACK")
                        else:
                            self.board_debug[row].append("WHITE")
                    else:
                        if col%2 == 0:
                            self.board_debug[row].append("BLACK")
                        else:
                            self.board_debug[row].append("WHITE")
        return self.board_debug

    def debug_print(self):
        if self.board_debug == []:
            self._init_board_debug()
        for row in range(ROWS):
            print(self.board_debug[row])

    # draw pieces
    def draw(self, window):
        self.draw_board(window)
        for row in range(ROWS):
            for col in range(COLS):
                piece = self.board[row][col]
                if piece != 0:
                    piece.draw_piece(window)

    def get_valid_moves(self, piece):
        moves = {}
        left = piece.col - 1
        right = piece.col + 1
        row = piece.row

        if piece.color == RED:
            moves.update(self._traverse_left(row - 1, max(row - 3, -1), -1, left))
            moves.update(self._traverse_right(row - 1, max(row - 3, -1), -1, right))
            moves.update(self._traverse_left(row + 1, min(row + 3, ROWS), 1, left))
            moves.update(self._traverse_right(row + 1, min(row + 3, ROWS), 1, right))

        if piece.color == BLACK:
            moves.update(self._traverse_left(row + 1, min(row + 3, ROWS), 1, left))
            moves.update(self._traverse_right(row + 1, min(row + 3, ROWS), 1, right))
        return moves

    # diagonal move
    def _traverse_left(self, start, stop, step, left, skipped=[]):
        moves = {}
        last = []
        for r in range(start, stop, step):
            if left < 0:
                break

            current = self.board[r][left]
            if current == 0:
                if skipped and not last:
                    break
                else:
                    moves[(r, left)] = last
                break
            elif current.color == RED or current.color == BLACK:
                break
            else:
                last = [current]

            left -= 1

        return moves

    def _traverse_right(self, start, stop, step, right, skipped=[]):
        moves = {}
        last = []
        for r in range(start, stop, step):
            if right >= COLS:
                break

            current = self.board[r][right]
            if current == 0:
                if skipped and not last:
                    break
                else:
                    moves[(r, right)] = last
                break
            elif current.color == RED or current.color == BLACK:
                break
            else:
                last = [current]

            right += 1

        return moves

    def __repr__(self):
        return self.board


class Piece:
    PADDING = 10
    OUTLINE = 2

    def __init__(self, row, col, color):
        self.row = row
        self.col = col
        self.color = color

        self.x = 0
        self.y = 0
        self.pos()

    def pos(self):
        self.x = SQUARE_SIZE * self.col + SQUARE_SIZE // 2
        self.y = SQUARE_SIZE * self.row + SQUARE_SIZE // 2

    def move(self, row, col):
        self.row = row
        self.col = col
        self.pos()

    def draw_piece(self, window):
        radius = SQUARE_SIZE//2 - self.PADDING
        pygame.draw.circle(window, BG, (self.x, self.y), radius + self.OUTLINE)
        pygame.draw.circle(window, self.color, (self.x, self.y), radius)

    def __repr__(self):
        return str(self.color)


class Game:
    def __init__(self, window):
        self.selected = None
        self._init()
        self.window = window

    # update board and check win state
    def update(self):
        self.board.draw(self.window)
        if self.selected:
            self.draw_valid_moves(self.valid_moves)
        if self.board.winner() == 'Hare':
            pygame.draw.circle(self.window, GOLD, (self.board.harePos[1] * SQUARE_SIZE + SQUARE_SIZE // 2, self.board.harePos[0] * SQUARE_SIZE + SQUARE_SIZE // 2), 50)
            end_screen(RED, 'HARE')
        elif self.board.winner() == 'Wolfs':
            for piece in self.board.get_all_pieces(BLACK):
                pygame.draw.circle(self.window, GOLD, (piece.col * SQUARE_SIZE + SQUARE_SIZE // 2, piece.row * SQUARE_SIZE + SQUARE_SIZE // 2), 50)
            end_screen(BG, 'WOLFS')
        else:
            pass
        pygame.display.update()

    def _init(self):
        self.board = Board()
        self.turn = RED
        self.valid_moves = {}
        self.board.debug_print()

    # show piece and its valid moves
    def select(self, row, col):
        if self.selected:
            result = self.move(row, col)
            # invalid move
            if not result:
                self.selected = None
                self.select(row, col)

        piece = self.board.get_piece(row, col)

        if piece != 0 and piece.color == self.turn:
            self.selected = piece
            self.valid_moves = self.board.get_valid_moves(piece)
            return True
        return False

    def move(self, row, col):
        piece = self.board.get_piece(row, col)
        if self.selected and piece == 0 and (row, col) in self.valid_moves:
            self.board.move(self.selected, row, col)
            self.board.debug_print()
            self.change_turn()
        else:
            return False
        return True

    # show valid moves
    def draw_valid_moves(self, moves):
        for move in moves:
            row, col = move
            pygame.draw.circle(self.window, BLUE, (col * SQUARE_SIZE + SQUARE_SIZE//2, row * SQUARE_SIZE + SQUARE_SIZE//2), 20)

    def change_turn(self):
        self.valid_moves = []
        if self.turn == RED:
            self.turn = BLACK
            print("Wolfs to move")
        else:
            print("Hare to move")
            self.turn = RED

    def get_board(self):
        return self.board

    def ai_move(self, board):
        self.board = board
        self.change_turn()


def end_screen(colour, label):
    time.sleep(2)
    pygame.draw.rect(window, colour, (0, 0, 800, 800))
    pygame.init()
    font = pygame.font.Font('freesansbold.ttf', 64)
    text = font.render('!!!      ' + label + ' WON      !!!', True, GOLD)
    window.blit(text, (0, 350))
    pygame.display.update()
    time.sleep(2)



def minimax(position, depth, max_player):
    if depth == 0 or position.winner() != None:
        return position.evaluate(), position

    if max_player:
        maxEval = float('-inf')
        best_move = None
        for move in get_all_moves(position, BLACK):
            evaluation = minimax(move, depth - 1, False)[0]
            maxEval = max(maxEval, evaluation)
            if maxEval == evaluation:
                best_move = move

        return maxEval, best_move
    else:
        minEval = float('inf')
        best_move = None
        for move in get_all_moves(position, RED):
            evaluation = minimax(move, depth - 1, True)[0]
            minEval = min(minEval, evaluation)
            if minEval == evaluation:
                best_move = move

        return minEval, best_move


def minimax_hare(position, depth, max_player):
    if depth == 0 or position.winner() != None:
        return position.alternate_evaluate(), position

    if max_player:
        maxEval = float('-inf')
        best_move = None
        for move in get_all_moves(position, RED):
            evaluation = minimax_hare(move, depth - 1, False)[0]
            maxEval = max(maxEval, evaluation)
            if maxEval == evaluation:
                best_move = move

        return maxEval, best_move
    else:
        minEval = float('inf')
        best_move = None
        for move in get_all_moves(position, BLACK):
            evaluation = minimax_hare(move, depth - 1, True)[0]
            minEval = min(minEval, evaluation)
            if minEval == evaluation:
                best_move = move

        return minEval, best_move


# function helper for minimax
def simulate_move(piece, move, board):
    board.move(piece, move[0], move[1])
    return board


# all possible moves
def get_all_moves(board, color):
    moves = []
    for piece in board.get_all_pieces(color):
        valid_moves = board.get_valid_moves(piece)
        for move in valid_moves.keys():
            temp_board = deepcopy(board)
            temp_piece = temp_board.get_piece(piece.row, piece.col)
            new_board = simulate_move(temp_piece, move, temp_board)
            moves.append(new_board)

    return moves


# mouse pos on board
def get_pos_mouse(pos):
    x, y = pos
    row = y // SQUARE_SIZE
    col = x // SQUARE_SIZE
    return row, col


# game mode menu
def game_intro():
    while True:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                quit()

        window.fill(BG)

        title = pygame.image.load("title.png")
        titleText = window.blit(title, title.get_rect())  # title is an image
        titleText.center = ((WIDTH / 2), (HEIGHT / 2))

        wolfsVSai = pygame.image.load("wolfsVSai.png")
        playerVSplayer = pygame.image.load("playerVSplayer.png")
        hareVSai = pygame.image.load("hareVsai.png")
        aiVSai = pygame.image.load("aiVSai.png")

        button(300, 200, 195, 100, playerVSplayer, two_players)
        button(300, 320, 195, 100, hareVSai, depth_selector_hare_ai)
        button(300, 440, 195, 100, wolfsVSai, depth_selector_ai_wolfs)
        button(300, 560, 195, 100, aiVSai, depth_selector_ai_ai)

        pygame.display.update()


# button logic
def button(x, y, w, h, inactive, action=None):
    mouse = pygame.mouse.get_pos()
    click = pygame.mouse.get_pressed()

    if x + w > mouse[0] > x and y + h > mouse[1] > y:
        window.blit(inactive, (x, y))
        if click[0] == 1 and action is not None:
            action()
    else:
        window.blit(inactive, (x, y))


# choose depth of level: player vs ai
def depth_selector_hare_ai():
    pygame.init()
    while True:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                quit()

        window.fill(BG)

        depth3 = pygame.image.load("Easy.png")
        depth5 = pygame.image.load("Medium.png")
        depth7 = pygame.image.load("Hard.png")

        button(300, 200, 195, 100, depth3, hare_vs_ai_depth3)
        button(300, 315, 195, 100, depth5, hare_vs_ai_depth5)
        button(300, 440, 195, 100, depth7, hare_vs_ai_depth7)

        pygame.display.update()


def depth_selector_ai_wolfs():
    pygame.init()
    while True:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                quit()

        window.fill(BG)

        depth3 = pygame.image.load("Easy.png")
        depth5 = pygame.image.load("Medium.png")
        depth7 = pygame.image.load("Hard.png")

        button(300, 200, 195, 100, depth3, wolfs_vs_ai_depth3)
        button(300, 315, 195, 100, depth5, wolfs_vs_ai_depth5)
        button(300, 440, 195, 100, depth7, wolfs_vs_ai_depth7)

        pygame.display.update()


# choose depth of level: ai vs ai
def depth_selector_ai_ai():
    pygame.init()
    while True:
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
                quit()

        window.fill(BG)

        depth3 = pygame.image.load("Easy.png")
        depth5 = pygame.image.load("Medium.png")
        depth7 = pygame.image.load("Hard.png")

        button(300, 200, 195, 100, depth3, ai_vs_ai_depth3)
        button(300, 315, 195, 100, depth5, ai_vs_ai_depth5)
        button(300, 440, 195, 100, depth7, ai_vs_ai_depth7)

        pygame.display.update()


# 2 players
def two_players():
    game = Game(window)
    pygame.init()

    while True:

        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                pygame.quit()
            if event.type == pygame.MOUSEBUTTONDOWN:
                pos = pygame.mouse.get_pos()
                row, col = get_pos_mouse(pos)
                game.select(row, col)

        game.update()

    pygame.quit()


# hare vs ai
def hare_vs_ai(depth):
    running = True
    game = Game(window)
    pygame.init()

    while running:

        if game.turn == BLACK:
            value, new_board = minimax(game.get_board(), depth, True)
            game.ai_move(new_board)

        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False
            if event.type == pygame.MOUSEBUTTONDOWN:
                pos = pygame.mouse.get_pos()
                row, col = get_pos_mouse(pos)
                game.select(row, col)
        game.update()
    pygame.quit()


def hare_vs_ai_depth3():
    hare_vs_ai(3)


def hare_vs_ai_depth5():
    hare_vs_ai(5)


def hare_vs_ai_depth7():
    hare_vs_ai(7)


def wolfs_vs_ai(depth):
    running = True
    game = Game(window)
    pygame.init()

    while running:

        if game.turn == RED:
            value, new_board = minimax(game.get_board(), depth, False)
            game.ai_move(new_board)

        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False
            if event.type == pygame.MOUSEBUTTONDOWN:
                pos = pygame.mouse.get_pos()
                row, col = get_pos_mouse(pos)
                game.select(row, col)
        game.update()
    pygame.quit()


def wolfs_vs_ai_depth3():
    wolfs_vs_ai(3)


def wolfs_vs_ai_depth5():
    wolfs_vs_ai(5)


def wolfs_vs_ai_depth7():
    wolfs_vs_ai(7)


# ai vs ai
def ai_vs_ai(depth):
    running = True
    game = Game(window)
    pygame.init()
    while running:

        if game.turn == BLACK:
            value, new_board = minimax(game.get_board(), depth, True)
            game.ai_move(new_board)
        else:
            value, new_board = minimax_hare(game.get_board(), depth, True)
            game.ai_move(new_board)

        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False
        game.update()
    pygame.quit()


def ai_vs_ai_depth3():
    ai_vs_ai(3)


def ai_vs_ai_depth5():
    ai_vs_ai(5)


def ai_vs_ai_depth7():
    ai_vs_ai(7)


# run
game_intro()
