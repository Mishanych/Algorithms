from queue import Queue
from puzzle import Puzzle

Iterations: int = 1
States_In_Memory: int = 1
Death_Nodes: int = 0

def breadth_first_search(initial_state):
    global Iterations, States_In_Memory, Death_Nodes
    start_node = Puzzle(initial_state, None, None, 0)
    if start_node.goal_test():
        return start_node.find_solution()
    q = Queue()
    q.put(start_node)
    explored = []
    while not(q.empty()):
        Iterations += 1
        node = q.get()
        explored.append(node.state)
        Death_Nodes = len(explored)

        children = node.generate_child()
        for child in children:
            if child.state not in explored:
                if child.goal_test():
                    return child.find_solution()
                q.put(child)
                States_In_Memory += 1
    return