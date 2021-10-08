from puzzle import Puzzle
from sys import maxsize

Iterations: int = 1
States_In_Memory: int = 1
Death_Nodes: int = 0

def recursive_best_first_search(initial_state):
    start_node = Puzzle(state=initial_state, parent=None, move=None, path_cost=0, needs_heuristic=True)
    node = RBFS_search(start_node, f_limit=maxsize)
    node = node[0]
    return node.find_solution()

def RBFS_search(node, f_limit):
    successors = []
    global Iterations, States_In_Memory, Death_Nodes
    if node.goal_test():
        return node, None
    children = node.generate_child()
    if not len(children):
        return None, maxsize
    count =- 1
    for child in children:
        count += 1
        child_info = (child.evaluation_function, count, child)
        successors.append(child_info)
        States_In_Memory += len(children)
    while len(successors):
        Iterations += 1
        successors.sort()
        best_node = successors[0][2]
        if best_node.evaluation_function > f_limit:
            Death_Nodes += 1
            return None, best_node.evaluation_function
        alternative = successors[1][0]
        result, best_node.evaluation_function = RBFS_search(best_node, min(f_limit, alternative))
        successors[0] = (best_node.evaluation_function, successors[0][1], best_node)
        if result != None:
            break
    return result, None