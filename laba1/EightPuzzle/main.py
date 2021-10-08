import random
import numpy as np

import BFS_search
import RBFS_search
from BFS_search import breadth_first_search
from RBFS_search import recursive_best_first_search
from puzzle import Puzzle

def getInvCount(arr):
    inv_count = 0
    empty_value = 0
    for i in range(0, 9):
        for j in range(i + 1, 9):
            if arr[j] != empty_value and arr[i] != empty_value and arr[i] > arr[j]:
                inv_count += 1
    return inv_count

def isSolvable(puzzle):
    inv_count = getInvCount([j for sub in puzzle for j in sub])
    return (inv_count % 2 == 0)




state = []
false_states = 0
is_solvable = False

while not is_solvable:
    puzzle = random.sample(range(0, 9), 9)
    false_states += 1
    puzzle_array = np.array(puzzle).reshape(-1, 3)
    is_solvable = isSolvable(puzzle_array)


Puzzle.total_states = 0
#puzzle = [1, 2, 4, 3, 0, 5, 7, 6, 8]
#puzzle = [8,5,6,7,2,3,4,1,0]
puzzle = [3,7,1,4,8,2,6,5,0]
for i in range(0, 9):
    if i % 3 == 0:
        print()
        print(puzzle[i], end =" ")
    else:
        print(puzzle[i], end =" ")
print()
print()
bfs = breadth_first_search(puzzle)
print('BFS:', bfs)
print('Total iterations:', BFS_search.Iterations)
print('Total deathnodes', BFS_search.Death_Nodes)
print('Total states:', Puzzle.total_states)
print('States in memory:', BFS_search.States_In_Memory)

print()

Puzzle.total_states = 0
RBFS = recursive_best_first_search(puzzle)


print('RBFS:', RBFS)
print('Total iterations:', RBFS_search.Iterations)
print('Total deathnodes', RBFS_search.Death_Nodes)
print('Total states:', Puzzle.total_states)
print('States in memory:', RBFS_search.States_In_Memory)
print()
print()
print('------------------------------------------')

