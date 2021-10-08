class Puzzle:
    goal_state = [0, 1, 2, 3, 4, 5, 6, 7, 8]
    heuristic = None
    evaluation_function = None
    needs_heuristic = False
    total_states = 0
    def __init__(self, state, parent, move, path_cost, needs_heuristic = False):
        self.parent = parent
        self.state = state
        self.move = move
        if parent:
            self.path_cost = parent.path_cost + path_cost
        else:
            self.path_cost = path_cost
        if needs_heuristic:
            self.needs_heuristic = True
            self.generate_heuristic()
            self.evaluation_function = self.heuristic + self.path_cost
        Puzzle.total_states += 1

    def __str__(self):
        return str(self.state[0:3]) + '\n'+str(self.state[3:6]) + '\n'+str(self.state[6:9])

    def generate_heuristic(self):
        self.heuristic = 0
        for num in range(1, 9):
            distance = abs(self.state.index(num) - self.goal_state.index(num))
            i = int(distance / 3)
            j = int(distance % 3)
            self.heuristic = self.heuristic + i + j

    def goal_test(self):
        if self.state == self.goal_state:
            return True
        return False

    @staticmethod
    def find_available_moves(i, j):
        available_move = ['U', 'D', 'L', 'R']
        if i == 0:
            available_move.remove('U')
        elif i == 2:
            available_move.remove('D')
        if j == 0:
            available_move.remove('L')
        elif j == 2:
            available_move.remove('R')
        return available_move

    def generate_child(self):
        children = []
        x = self.state.index(0)
        i = int(x / 3)
        j = int(x % 3)
        available_move = self.find_available_moves(i, j)

        for move in available_move:
            new_state = self.state.copy()
            if move == 'U':
                new_state[x], new_state[x-3] = new_state[x-3], new_state[x]
            elif move == 'D':
                new_state[x], new_state[x+3] = new_state[x+3], new_state[x]
            elif move == 'L':
                new_state[x], new_state[x-1] = new_state[x-1], new_state[x]
            elif move == 'R':
                new_state[x], new_state[x+1] = new_state[x+1], new_state[x]
            children.append(Puzzle(new_state, self, move, 1, self.needs_heuristic))
        return children

    def find_solution(self):
        solution = []
        solution.append(self.move)
        path = self
        while path.parent != None:
            path = path.parent
            solution.append(path.move)
        solution = solution[:-1]
        solution.reverse()
        return solution


    def printPuzzle(self):
        self.__str__(self)