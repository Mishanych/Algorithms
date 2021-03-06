import random
import functools
from сhromosome import Chromosome


number_of_vertex = 300
chromosome_length = number_of_vertex

weights = [[0] * number_of_vertex] * number_of_vertex

quite = False


class GeneNetwork(object):
    def __init__(self, dim, weights, chromosome_length, source, destination):
        """

        :param dim: problem dimension
        :param weights:
        :param chromosome_length: length of chromosome
        :param source: source node
        :param destination: destination node
        :return:
        """
        if source >= dim or destination >= dim:
            raise ValueError
        self.chromosome_length = chromosome_length
        self.dim = dim
        self.weights = weights
        self.source = source
        self.destination = destination
        self.population = []
        self.population_size = 0
        self.results = []
        self.best = None

    def start(self, gen_max, pop_size):
        """

        :param gen_max: maximum number of generations
        :param pop_size: initial population size
        :return: best solution found
        """
        gen = 1  # from first generation
        self.generate_population(pop_size)  # generate initial population
        self.population_size = pop_size
        if not quite:
            pretty_print('Initital:')
            self.print_chromosomes(self.population)

        while gen <= gen_max:
            gen += 1
            p = 1
            new_population = list()
            while p <= self.population_size:
                p += 1
                parents = random.sample(range(self.population_size), 2)
                newbie = self.crossover(self.population[parents[0]], self.population[parents[1]])
                newbie.mutate()
                fit = self.fitness(newbie)
                self.results.append((newbie, fit))
                new_population.append(newbie)
                if self.best is None or self.best[1] > fit:
                    self.best = (newbie, fit)
            if not quite:
                pretty_print('%dth generation (after crossover, mutations): ' % gen)
                self.print_chromosomes(new_population)
            self.selection(self.population, new_population)
            if not quite:
                pretty_print('After selection: ')
                self.print_chromosomes(new_population)
        return self.best

    def selection(self, prev, now):
        """

        :param prev: previous generation
        :param now: new generation
        :return:
        """
        prev.extend(now)
        keyfunc = functools.cmp_to_key(lambda x, y: (self.fitness(x) - self.fitness(y)))
        prev.sort(key=keyfunc)
        self.population = prev[:self.population_size]

    def generate_population(self, n):
        """

        :param n: number of chromosomes
        :return:
        """
        chromosomes = list()
        for i in range(n):
            chromosomes.append(self._gen_chromosome())
        self.population = chromosomes

    def _gen_chromosome(self):
        """

        :return: random path from source to destination
        """
        chromosome = random.sample(list(set(range(self.dim)) - {self.source, self.destination}),
                                   self.chromosome_length - 2)
        chromosome.insert(0, self.source)
        chromosome.append(self.destination)
        return Chromosome(chromosome)

    def crossover(self, mother, father):
        """

        :param mother: first parent
        :param father: second parent
        :return: crossing over child
        """
        mother_list = mother.get()
        father_list = father.get()
        cut = random.randint(0, self.chromosome_length - 1)
        child = mother_list[0:cut] + father_list[cut:]
        return Chromosome(child)

    def fitness(self, chromosome):
        chromosome_list = chromosome.get()
        return sum([self.weights[i][j] for i, j in zip(chromosome_list[:-1], chromosome_list[1:])])

    def print_chromosomes(self, chromosomes):
        for chromosome in chromosomes:
            print(str(chromosome) + ' ' + str(self.fitness(chromosome)))


def pretty_print(to_print, hint=''):
    print('')
    print('==================')
    print(hint + str(to_print))
    print('==================')


if __name__ == "__main__":

    weights = [[random.randint(5, 150) for x in range(0, number_of_vertex)] for y in range(0, number_of_vertex)]

    for i in range(0, number_of_vertex):
        for j in range(0, number_of_vertex):
            if i == j:
                weights[i][j] = 0
    gene_network = GeneNetwork(number_of_vertex, weights, chromosome_length, 0, 299)
    res = gene_network.start(1000, 50)  # start with 1000 generations and 10 initial chromosomes
    pretty_print(res, 'Solution: ')